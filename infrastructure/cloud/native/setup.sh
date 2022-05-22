# Heavily inspired by: https://medium.com/the-glitcher/mongodb-sharding-9c5357a95ec1


GROUP='database-exam'
VMSIZE='Standard_B1ls'
#IMG='Canonical:UbuntuServer:16_04_0-lts-gen2:16.04.202109280'
IMG='ubuntults'
LOCATION='northeurope'
NSG='database-exam-nsg'

GATEWAY='gw'
GATEWAYDNS='db-exam-'$GATEWAY

USER=$USERNAME


az group delete --name $GROUP --yes
az group create --name $GROUP --location northeurope 

az network vnet create --name 'exam-vnet' --resource-group $GROUP
az network nsg create -n $NSG -g $GROUP
az network nsg rule create -n 'mongo' -g $GROUP --nsg-name $NSG --priority 700   \
--direction Inbound --access Allow --protocol Tcp --description "Allow mongo traffic" --destination-port-ranges '27017' --source-address-prefixes "*"

az network nsg rule create -n 'redis' -g $GROUP --nsg-name $NSG --priority 710   \
--direction Inbound --access Allow --protocol Tcp --description "Allow redis traffic" --destination-port-ranges '6379' --source-address-prefixes "*"

az network nsg rule create -n 'postgres' -g $GROUP --nsg-name $NSG --priority 720   \
--direction Inbound --access Allow --protocol Tcp --description "Allow postgres traffic" --destination-port-ranges '6379' --source-address-prefixes "*"

az network nsg rule create -n 'neo4j' -g $GROUP --nsg-name $NSG --priority 730   \
--direction Inbound --access Allow --protocol Tcp --description "Allow neo4j traffic" --destination-port-ranges '6379' --source-address-prefixes "*"

az network nsg rule create -n 'ssh' -g $GROUP --nsg-name $NSG --priority 750   \
--direction Inbound --access Allow --protocol Tcp --description "Allow ssh traffic" --destination-port-ranges '22' --source-address-prefixes "*"

az network nsg rule create -n 'http' -g $GROUP --nsg-name $NSG --priority 770   \
--direction Inbound --access Allow --protocol Tcp --description "Allow http traffic" --destination-port-ranges '80'

az network nsg rule create -n 'https' -g $GROUP --nsg-name $NSG --priority 780   \
--direction Inbound --access Allow --protocol Tcp --description "Allow https traffic" --destination-port-ranges '443'

az vm create -g $GROUP -n $GATEWAY --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address $GATEWAYDNS --generate-ssh-keys --nsg $NSG --public-ip-sku Basic --storage-sku Standard_LRS --os-disk-size-gb 30

az vm create -g $GROUP -n mongo-shard1-1 --image $IMG --size $VMSIZE \
--location $LOCATION  --public-ip-address "" --generate-ssh-keys --nsg $NSG --storage-sku Standard_LRS --os-disk-size-gb 30

az vm create -g $GROUP -n mongo-shard1-2 --image $IMG --size $VMSIZE \
--location $LOCATION  --public-ip-address "" --generate-ssh-keys --nsg $NSG --storage-sku Standard_LRS --os-disk-size-gb 30

az vm create -g $GROUP -n mongo-shard2-1 --image $IMG --size $VMSIZE \
--location $LOCATION  --public-ip-address "" --generate-ssh-keys --nsg $NSG --storage-sku Standard_LRS --os-disk-size-gb 30

az vm create -g $GROUP -n mongo-shard2-2 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address "" --generate-ssh-keys --nsg $NSG --storage-sku Standard_LRS --os-disk-size-gb 30


az network public-ip update --resource-group $GROUP --name $GATEWAYDNS --dns-name $GATEWAYDNS


################# Setup config server ############

echo "Start configuration"

az vm run-command invoke -g $GROUP -n $GATEWAY --command-id RunShellScript \
--scripts "@mongo/config-setup-1.sh" --parameters $USER

echo "Setup base-1-1"
az vm run-command invoke -g $GROUP -n mongo-shard1-1 --command-id RunShellScript \
--scripts "@mongo/shard-setup-base.sh" --parameters $USER mongors1

echo "Setup base-1-2"
az vm run-command invoke -g $GROUP -n mongo-shard1-2 --command-id RunShellScript \
--scripts "@mongo/shard-setup-base.sh" --parameters $USER mongors1

echo "Setup shard 1"
az vm run-command invoke -g $GROUP -n mongo-shard1-1 --command-id RunShellScript \
--scripts "@mongo/shard-setup-1.sh" --parameters $USER mongo-shard1-1:27018 mongo-shard1-2:27018 mongors1

echo "Setup shard 2"
### Shard 2 ###

echo "Setup base-2-1"
az vm run-command invoke -g $GROUP -n mongo-shard2-1 --command-id RunShellScript \
--scripts "@mongo/shard-setup-base.sh" --parameters $USER mongors2

echo "Setup base-2-2"
az vm run-command invoke -g $GROUP -n mongo-shard2-2 --command-id RunShellScript \
--scripts "@mongo/shard-setup-base.sh" --parameters $USER mongors2

echo "Setup shard 2"
az vm run-command invoke -g $GROUP -n mongo-shard2-1 --command-id RunShellScript \
--scripts "@mongo/shard-setup-1.sh" --parameters $USER mongo-shard2-1:27018 mongo-shard2-2:27018 mongors2

##### Post config ###

echo "Start configuration pt2"
az vm run-command invoke -g $GROUP -n $GATEWAY --command-id RunShellScript \
--scripts "@mongo/config-setup-2.sh" --parameters $USER
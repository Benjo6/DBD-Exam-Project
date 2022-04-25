# Heavily inspired by: https://medium.com/the-glitcher/mongodb-sharding-9c5357a95ec1


GROUP='mongo'
VMSIZE='Standard_B1ls'
#IMG='Canonical:UbuntuServer:16_04_0-lts-gen2:16.04.202109280'
IMG='ubuntults'
LOCATION='northeurope'
NSG='mongo-nsg'
USER=$USERNAME



az group create --name $GROUP --location northeurope 

az network vnet create --name 'mongo-vnet' --resource-group $GROUP
az network nsg create -n $NSG -g $GROUP
az network nsg rule create -n 'mongo' -g $GROUP --nsg-name $NSG --priority 700   \
--direction Inbound --access Allow --protocol Tcp --description "Allow mongo traffic" --destination-port-ranges '27017' --source-address-prefixes "213.32.241.166"

az network nsg rule create -n 'ssh' -g $GROUP --nsg-name $NSG --priority 750   \
--direction Inbound --access Allow --protocol Tcp --description "Allow ssh traffic" --destination-port-ranges '22' --source-address-prefixes "213.32.241.166"

az network nsg rule create -n 'http' -g $GROUP --nsg-name $NSG --priority 770   \
--direction Inbound --access Allow --protocol Tcp --description "Allow http traffic" --destination-port-ranges '80,443'

az vm create -g $GROUP -n mongo-config --image $IMG --size Standard_B1ms \
--location $LOCATION --public-ip-address db-ass-mongo-config --generate-ssh-keys --nsg $NSG --public-ip-sku Basic

az vm create -g $GROUP -n mongo-shard1-1 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address db-ass-mongo-shard1-1 --generate-ssh-keys --nsg $NSG --public-ip-sku Basic

az vm create -g $GROUP -n mongo-shard1-2 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address db-ass-mongo-shard1-2 --generate-ssh-keys --nsg $NSG --public-ip-sku Basic

az vm create -g $GROUP -n mongo-shard2-1 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address db-ass-mongo-shard2-1 --generate-ssh-keys --nsg $NSG --public-ip-sku Basic

az vm create -g $GROUP -n mongo-shard2-2 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address db-ass-mongo-shard2-2 --generate-ssh-keys --nsg $NSG --public-ip-sku Basic


az network public-ip update --resource-group $GROUP --name db-ass-mongo-config --dns-name db-ass-mongo-config
az network public-ip update --resource-group $GROUP --name db-ass-mongo-shard1-1 --dns-name db-ass-mongo-shard1-1
az network public-ip update --resource-group $GROUP --name db-ass-mongo-shard1-2 --dns-name db-ass-mongo-shard1-2
az network public-ip update --resource-group $GROUP --name db-ass-mongo-shard2-1 --dns-name db-ass-mongo-shard2-1
az network public-ip update --resource-group $GROUP --name db-ass-mongo-shard2-2 --dns-name db-ass-mongo-shard2-2

################# Setup config server ############

echo "Start configuration"

az vm run-command invoke -g $GROUP -n mongo-config --command-id RunShellScript \
--scripts "@config-setup-1.sh" --parameters $USER


################### Setup shards ##################

echo "Setup shard 1"
### Shard 1 ###

echo "Setup base-1-1"
az vm run-command invoke -g $GROUP -n mongo-shard1-1 --command-id RunShellScript \
--scripts "@shard-setup-base.sh" --parameters $USER mongors1

echo "Setup base-1-2"
az vm run-command invoke -g $GROUP -n mongo-shard1-2 --command-id RunShellScript \
--scripts "@shard-setup-base.sh" --parameters $USER mongors1

echo "Setup shard 1-1"
az vm run-command invoke -g $GROUP -n mongo-shard1-1 --command-id RunShellScript \
--scripts "@shard-setup-1-1.sh"

echo "Setup shard 2"
### Shard 2 ###

echo "Setup base-2-1"
az vm run-command invoke -g $GROUP -n mongo-shard2-1 --command-id RunShellScript \
--scripts "@shard-setup-base.sh" --parameters $USER mongors2

echo "Setup base-2-2"
az vm run-command invoke -g $GROUP -n mongo-shard2-2 --command-id RunShellScript \
--scripts "@shard-setup-base.sh" --parameters $USER mongors2

echo "Setup shard 2-1"
az vm run-command invoke -g $GROUP -n mongo-shard2-1 --command-id RunShellScript \
--scripts "@shard-setup-2-1.sh"

##### Post config ###

echo "Start configuration pt2"
az vm run-command invoke -g $GROUP -n mongo-config --command-id RunShellScript \
--scripts "@config-setup-2.sh"


#db.setSecondary(ok) -> usereadpref
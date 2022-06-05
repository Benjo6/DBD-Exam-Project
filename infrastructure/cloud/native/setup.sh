# Heavily inspired by: https://medium.com/the-glitcher/mongodb-sharding-9c5357a95ec1


ADMIN_USER=fedtnisse
ADMIN_PASSWORD=$1

DB_USER=dbexam
DB_PASSWORD=$2


GROUP='database-exam'
VMSIZE='Standard_B1ls'
#IMG='Canonical:UbuntuServer:16_04_0-lts-gen2:16.04.202109280'
IMG='ubuntults'
LOCATION='northeurope'
NSG='database-exam-nsg'

GATEWAY='gw'
GATEWAYDNS='db-exam-'$GATEWAY

echo "Delete res-group"
az group delete --name $GROUP --yes --force-deletion-types Microsoft.Compute/virtualMachines

echo "Res-group exists"
echo $(az group exists -n 'database-exam')

echo "Create res-group"
az group create --name $GROUP --location northeurope

az network vnet create --name 'exam-vnet' --resource-group $GROUP
az network nsg create -n $NSG -g $GROUP
az network nsg rule create -n 'mongo' -g $GROUP --nsg-name $NSG --priority 700   \
--direction Inbound --access Allow --protocol Tcp --description "Allow mongo traffic" --destination-port-ranges '27017' --source-address-prefixes "*"

az network nsg rule create -n 'postgres' -g $GROUP --nsg-name $NSG --priority 720   \
--direction Inbound --access Allow --protocol Tcp --description "Allow postgres traffic" --destination-port-ranges '5432' --source-address-prefixes "*"

az network nsg rule create -n 'neo4j' -g $GROUP --nsg-name $NSG --priority 730   \
--direction Inbound --access Allow --protocol Tcp --description "Allow neo4j traffic" --destination-port-ranges '7474' --source-address-prefixes "*"

az network nsg rule create -n 'ssh' -g $GROUP --nsg-name $NSG --priority 750   \
--direction Inbound --access Allow --protocol Tcp --description "Allow ssh traffic" --destination-port-ranges '22' --source-address-prefixes "*"

az network nsg rule create -n 'http' -g $GROUP --nsg-name $NSG --priority 770   \
--direction Inbound --access Allow --protocol Tcp --description "Allow http traffic" --destination-port-ranges '80'

az network nsg rule create -n 'https' -g $GROUP --nsg-name $NSG --priority 780   \
--direction Inbound --access Allow --protocol Tcp --description "Allow https traffic" --destination-port-ranges '443'

az vm create -g $GROUP -n $GATEWAY --image $IMG --size Standard_B2s \
--location $LOCATION --public-ip-address $GATEWAYDNS \
--nsg $NSG --public-ip-sku Basic --storage-sku Standard_LRS --os-disk-size-gb 30 \
--admin-username $ADMIN_USER --admin-password $ADMIN_PASSWORD

az vm create -g $GROUP -n vm-a-1 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address "" --admin-username $ADMIN_USER --admin-password $ADMIN_PASSWORD --nsg $NSG --storage-sku Standard_LRS --os-disk-size-gb 30

az vm create -g $GROUP -n vm-a-2 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address "" --admin-username $ADMIN_USER --admin-password $ADMIN_PASSWORD --nsg $NSG --storage-sku Standard_LRS --os-disk-size-gb 30

az vm create -g $GROUP -n vm-b-1 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address "" --admin-username $ADMIN_USER --admin-password $ADMIN_PASSWORD --nsg $NSG --storage-sku Standard_LRS --os-disk-size-gb 30

az vm create -g $GROUP -n vm-b-2 --image $IMG --size $VMSIZE \
--location $LOCATION --public-ip-address "" --admin-username $ADMIN_USER --admin-password $ADMIN_PASSWORD --nsg $NSG --storage-sku Standard_LRS --os-disk-size-gb 30


az network public-ip update --resource-group $GROUP --name $GATEWAYDNS --dns-name $GATEWAYDNS


################# Setup config server ############

echo "Start configuration"

az vm run-command invoke -g $GROUP -n $GATEWAY --command-id RunShellScript \
--scripts "@mongo/config-setup-1.sh" --parameters "${ADMIN_USER}" ${ADMIN_PASSWORD} "$DB_USER" $DB_PASSWORD "${GATEWAY}"

echo "Setup base-1-1"
az vm run-command invoke -g $GROUP -n vm-a-1 --command-id RunShellScript \
--scripts "@mongo/shard-setup-base.sh" --parameters "$ADMIN_USER" mongors1

echo "Setup base-1-2"
az vm run-command invoke -g $GROUP -n vm-a-2 --command-id RunShellScript \
--scripts "@mongo/shard-setup-base.sh" --parameters $ADMIN_USER mongors1

echo "Setup shard 1"
az vm run-command invoke -g $GROUP -n vm-a-1 --command-id RunShellScript \
--scripts "@mongo/shard-setup-1.sh" --parameters $ADMIN_USER vm-a-1:27018 vm-a-2:27018 mongors1

### Shard 2 ###

echo "Setup base-2-1"
az vm run-command invoke -g $GROUP -n vm-b-1 --command-id RunShellScript \
--scripts "@mongo/shard-setup-base.sh" --parameters $ADMIN_USER mongors2

echo "Setup base-2-2"
az vm run-command invoke -g $GROUP -n vm-b-2 --command-id RunShellScript \
--scripts "@mongo/shard-setup-base.sh" --parameters $ADMIN_USER mongors2

echo "Setup shard 2"
az vm run-command invoke -g $GROUP -n vm-b-1 --command-id RunShellScript \
--scripts "@mongo/shard-setup-1.sh" --parameters $ADMIN_USER vm-b-1:27018 vm-b-2:27018 mongors2

##### Post config ###

echo "Start configuration pt2"
az vm run-command invoke -g $GROUP -n $GATEWAY --command-id RunShellScript \
--scripts "@mongo/config-setup-2.sh" --parameters "${ADMIN_USER}" ${ADMIN_PASSWORD} "$DB_USER" $DB_PASSWORD "${GATEWAY}"


### Start Applications ###

az vm run-command invoke -g $GROUP -n $GATEWAY --command-id RunShellScript \
--scripts "@compose-setup.sh" --parameters $ADMIN_USER $ADMIN_PASSWORD $DB_USER $DB_PASSWORD


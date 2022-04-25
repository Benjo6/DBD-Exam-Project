GROUP=mongo

az vm run-command invoke -g $GROUP -n mongo-config --command-id RunShellScript \
--scripts "@deploy-application.sh" --parameters $USERNAME $TWITTER
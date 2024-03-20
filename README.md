# weather-app

## infra

remote:

- heroku postgres backend

# Pick a unique app name

export APP_NAME=my-terraform-backend

# Create the database

heroku login
heroku create $APP_NAME
heroku addons:create heroku-postgresql:essential-2 --app $APP_NAME

# wait unitl provisioned

# On each machine where it's used, initialize Terraform

# with the database credentials

export DATABASE_URL=`heroku config:get DATABASE_URL --app $APP_NAME`
terraform init -backend-config="conn_str=$DATABASE_URL"

heroku authorizations:create --description weather-app

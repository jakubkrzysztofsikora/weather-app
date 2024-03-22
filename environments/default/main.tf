terraform {
  backend "pg" {
  }
}

module "default" {
  source            = "../../infrastructure"
  api_app_name      = "circit-weather-api"
  frontend_app_name = "circit-weather-frontend"
  location          = "eu"
  weather_api_key   = var.weather_api_key
}

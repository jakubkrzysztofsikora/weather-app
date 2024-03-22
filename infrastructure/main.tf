terraform {
  required_providers {
    heroku = {
      source  = "heroku/heroku"
      version = "~> 5.0"
    }
  }
}

resource "heroku_app" "api" {
  name   = var.api_app_name
  region = var.location

  config_vars = {
    WeatherApiKey = var.weather_api_key
  }
}

resource "heroku_app" "frontend" {
  name   = var.frontend_app_name
  region = var.location

  config_vars = {
    API_URL = heroku_app.api.web_url
  }
}

output "api_url" {
  value = heroku_app.api.web_url
}

output "frontend_url" {
  value = heroku_app.frontend.web_url
}

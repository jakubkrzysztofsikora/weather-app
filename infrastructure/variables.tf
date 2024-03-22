variable "api_app_name" {
  description = "The name of the API App"
}

variable "frontend_app_name" {
  description = "The name of the Frontend App"
}

variable "location" {
  description = "The location/region of the resources"
  default     = "eu"
}

variable "weather_api_key" {
  description = "The API key for the weather API"
}
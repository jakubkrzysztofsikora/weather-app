# Weather App

## Description

A simple web application that displays the current weather for cities around the world. The frontend is built with Vue.js (3.4.X) and TypeScript, while the backend API is developed using .NET 8. This project also includes a minimal proxy server for serving static assets and providing environment configuration to the frontend, written in Node.js with express.js.

## Table of Contents

- [Weather App](#weather-app)
  - [Description](#description)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
    - [Prerequisites](#prerequisites)
    - [Getting Started](#getting-started)
  - [Usage](#usage)
    - [Development](#development)
    - [Deploying](#deploying)
      - [Pre-deployed Application](#pre-deployed-application)
      - [Heroku with Terraform](#heroku-with-terraform)
      - [Testing](#testing)
      - [Docker](#docker)
  - [License](#license)

## Installation

### Prerequisites

- .NET 8 SDK
- Node.js and npm
- Docker (optional, for containerization)
- Terraform (for infrastructure deployment)
- Heroku CLI (for infrastructure deployment)

### Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/jakubkrzysztofsikora/weather-app.git
   ```
2. Install dependencies:
   1. For the API:
   ```
   cd api
   dotnet restore
   ```
   2. For the frontend:
   ```
   cd webapp/frontend
   npm install
   ```

## Usage

This section provides detailed instructions on how to use the weather application, including development, testing, and running the application using Docker.

#### Development

To run the application in development mode, you will need to start both the .NET API and the Vue.js frontend separately. This allows for hot reloading and easier debugging.

- **Running the API:**

  1.  Navigate to the API directory:\
      `cd api`
  2.  Run the .NET API:\
      `dotnet run`\
      This starts the API on 3001, allowing the frontend to communicate with it.

- **Running the Frontend:**

  1.  Navigate to the frontend directory:\
      `cd webapp/frontend`
  2.  Install the necessary npm packages (if you haven't already):\
      `npm install`
  3.  Start the Vue.js development server:\
      `npm run dev`\
      This command serves the frontend on a local web server, available at `http://localhost:3000`. The application will automatically reload if you change any of the source files.

You can modify env variables by changing `/webapp/frontend/.env` file. When running frontend in development mode by `npm run dev` the variables are taken directly from the file. When running the app via server-proxy, the variables are also taken from a file, but served to the frontend from the node.js API endpoint.

### Deploying

Deploying the weather application to Heroku using Terraform is a structured process that involves setting up your infrastructure and configuring environment variables. This guide assumes you have basic familiarity with Heroku, Terraform, and your terminal.

#### Pre-deployed Application

A version of the application is already deployed and available for testing [here](https://circit-weather-frontend-934e50417407.herokuapp.com). Please note that this deployment is available only until the end of March 2024 (just saving some cash on the infrastructure).

#### Heroku with Terraform

1. **Prerequisites:**

   - Ensure you have the Heroku CLI and Terraform installed on your machine.

2. **Heroku Authentication:**

   - Login to Heroku using the CLI: `heroku login`

3. **Terraform Configuration:**

   - The project contains an `/infrastructure` directory in the root directory with environment-agnostic module definitions for the infrastructure. These modules are designed to be used by configurations in the `/environments` directory.
   - You can use the default environment, you just have to be logged in with the heroku cli
   - To add a new environment, create a new directory under `/environments` and include `main.tf` and `variables.tf`, similar to the existing `/environments/default` structure.

4. **(Optional) Setting Up Your Environment:**

   - Navigate to your environment directory, for example, `/environments/default`.
   - Configure the module in `environemnts/{YOUR_ENV}/main.tf` with the necessary parameters:
     ```
     module "default" {
       source            = "../../infrastructure"
       api_app_name      = "{FILL_THIS_UP}"
       frontend_app_name = "{FILL_THIS_UP}"
       location          = "{FILL_THIS_UP}"
       weather_api_key   = var.weather_api_key
     }
     ```
   - In `variables.tf`, define the `weather_api_key`:
     ```
     variable "weather_api_key" {
       description = "Value of the Weather API key"
     }
     ```

5. **Create a Heroku App for terraform backend/state:**

   - Set a unique name for your app. Replace `my-weather-app` with your desired app name:`export APP_NAME=my-weather-app`
   - Create the Heroku app: `heroku create $APP_NAME`
   - Add a Heroku Postgres addon to your app: `heroku addons:create heroku-postgresql:essential-2 --app $APP_NAME`
   - Wait a few moments for the database to provision.

6. **Get Database URL:**

   - Retrieve the database URL for Terraform: `export DATABASE_URL=$(heroku config:get DATABASE_URL --app $APP_NAME)`

7. **Initialize Terraform:**

   - Navigate to your environment Terraform configuration directory.
   - Initialize Terraform with your Heroku database: `terraform init -backend-config="conn_str=$DATABASE_URL"`

8. **Deploying with Terraform:**

   - Before deploying, ensure the `WeatherApiKey` is passed as a variable to Terraform. This can be done through a `.tfvars` file or command-line flags.
   - Initialize Terraform in your environment directory:`terraform init`
   - Apply your Terraform configuration, passing in the `weather_api_key` as needed: `terraform apply -var "weather_api_key=YOUR_API_KEY_HERE"`

   Replace `YOUR_API_KEY_HERE` with your actual Weather API key.

9. **a) Automated Deployment with GitHub Actions:**

Upon each commit to the main branch, a GitHub Actions workflow defined in `.github/workflows/cicd.yml` automates the process of building all components, running tests, and deploying both the frontend and backend applications. This CI/CD pipeline simplifies the deployment process, ensuring that only successfully tested versions of the application are deployed.

To leverage this automated deployment process through GitHub Actions, it's necessary to configure several variables and secrets within your GitHub repository's settings. These include:

- `HEROKU_API_KEY`: Your Heroku API key, used for authentication with Heroku CLI commands within the GitHub Actions workflow.
- `HEROKU_APP_NAME_API`: The name of your Heroku application for the backend API. This name is used to direct the deployment to the correct Heroku app.
- `HEROKU_APP_NAME_WEBAPP`: The name of your Heroku application for the frontend. Similar to the API app name, this ensures the frontend is deployed to the right place.
- `HEROKU_EMAIL`: The email associated with your Heroku account. This is required for authentication purposes in the GitHub Actions workflow.

These secrets ensure secure and seamless integration between your GitHub repository and Heroku, enabling automatic deployment without manual intervention.

10. **b) Manual Deployment Option:**

For those preferring or requiring manual deployment, both the frontend and backend applications can be deployed to Heroku using the Heroku CLI, leveraging the Dockerfiles provided in the project. This method gives you more control over the deployment process and allows for custom configurations or troubleshooting as needed. [Learn more](https://devcenter.heroku.com/categories/deploying-with-docker).

#### Testing

Ensuring that both the API and the frontend work as expected is crucial. Here's how you can run tests for both components:

- **API Tests:**

  1.  Navigate to the root directory.
  2.  Execute the tests using the .NET CLI:\
      `dotnet test`\
      This runs all unit tests within the `api-test` directory (or any other test projects that would be defined in `.sln` file in the root), ensuring that the backend logic functions correctly.

- **Frontend Tests:**

  1.  Navigate to the frontend directory:\
      `cd webapp/frontend`
  2.  Run the Vue.js tests:\
      `npm run test`\
      This executes all tests related to the frontend, verifying that the Vue.js components and logic are working as intended.

#### Docker

Here's how you can use Docker to run the application:

- **Building and Running with Docker Compose:**
  1.  Ensure Docker is installed on your machine.
  2.  You should have `WeatherApiKey` env variable set locally.
  3.  At the root of the project, run:\
      `docker-compose up`\
      This command builds the Docker images for both the API and the proxy server (if not already built) and starts the containers. The application should then be accessible via the ports specified in the `docker-compose.yml` file.

## License

This project is licensed under the MIT License - see the LICENSE file in the root dir for details.

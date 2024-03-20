# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# production stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as production-stage
ARG PORT
ENV PORT ${PORT}
EXPOSE ${PORT}
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "api.dll"]
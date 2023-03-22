FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY . ./
RUN dotnet publish "src/Clients/BlazorWebApp/WebApp/WebApp.csproj" -c Release -o /app/build

FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build-env /app/build/wwwroot .
COPY src/Clients/BlazorWebApp/WebApp/nginx.conf /etc/nginx/nginx.conf
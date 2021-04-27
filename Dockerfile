FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env
WORKDIR /app

# Copy everything else and build
COPY ./src ./
RUN dotnet restore && dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

COPY /certs/https.pfx /certs/

EXPOSE 5000 5001

# Generate certificate with
# openssl req -x509 -sha256 -nodes -days 10365 -newkey rsa:2048 -keyout https.key -out https.crt
# openssl pkcs12 -inkey https.key -in https.crt -export -out https.pfx

ENV ASPNETCORE_URLS=https://0.0.0.0:5001;http://0.0.0.0:5000 \
    ASPNETCORE_Kestrel__Certificates__Default__Password="demo" \
    ASPNETCORE_Kestrel__Certificates__Default__Path="/certs/https.pfx"

ENTRYPOINT ["dotnet", "NotakeyOidcDemo.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY . /src
WORKDIR /src

RUN dotnet restore
RUN dotnet publish "./src/presentation/Unistream.Transaction.HttpApi/Unistream.Transaction.HttpApi.csproj" -c Release -o ./publish --no-restore
# --------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /src/publish /app

VOLUME ./appsettings ./app/appsettings
VOLUME ./logs ./app/logs

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "/app/Unistream.Transaction.HttpApi.dll"]
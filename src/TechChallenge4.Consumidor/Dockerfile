FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app

# Instala bibliotecas necessárias para o SQL Server
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY ./publish .
ENTRYPOINT ["dotnet", "TechChallenge4.Consumidor.dll"]
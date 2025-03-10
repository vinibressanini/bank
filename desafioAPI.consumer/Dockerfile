FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["desafioAPI.consumer/desafioAPI.consumer.csproj", "desafioAPI.consumer/"]
COPY ["desafioAPI/desafioAPI.csproj", "desafioAPI/"]
RUN dotnet restore "./desafioAPI.consumer/desafioAPI.consumer.csproj"
COPY . .
WORKDIR "/src/desafioAPI.consumer"
RUN dotnet build "./desafioAPI.consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./desafioAPI.consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

ENV RabbitMQ_Host=localhost
ENV RabbitMQ_Username=guest
ENV RabbitMQ_Password=guest

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "desafioAPI.consumer.dll"]
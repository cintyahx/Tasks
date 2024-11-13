#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Miotto.Tasks/Miotto.Tasks.API.csproj", "Miotto.Tasks/"]
COPY ["Miotto.Tasks.Infra/Miotto.Tasks.Infra.csproj", "Miotto.Tasks.Infra/"]
COPY ["Miotto.Tasks.Domain/Miotto.Tasks.Domain.csproj", "Miotto.Tasks.Domain/"]
COPY ["Miotto.Tasks.Service/Miotto.Tasks.Service.csproj", "Miotto.Tasks.Service/"]
RUN dotnet restore "./Miotto.Tasks/Miotto.Tasks.API.csproj"
COPY . .
WORKDIR "/src/Miotto.Tasks"
RUN dotnet build "./Miotto.Tasks.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Miotto.Tasks.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Miotto.Tasks.API.dll"]
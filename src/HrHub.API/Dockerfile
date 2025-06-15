FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./src/ ./
WORKDIR /src/HrHub.API

RUN dotnet restore "HrHub.API.csproj"
RUN dotnet build "HrHub.API.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "HrHub.API.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_EnableDiagnostics=1
ENTRYPOINT ["dotnet", "HrHub.API.dll"]

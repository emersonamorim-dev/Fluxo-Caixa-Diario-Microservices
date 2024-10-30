FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8088

# Fase de build do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj e restaurar as dependências
COPY ["FluxoCaixaDiarioMicroservice/FluxoCaixaDiarioMicroservice.csproj", "FluxoCaixaDiarioMicroservice/"]
RUN dotnet restore "FluxoCaixaDiarioMicroservice/FluxoCaixaDiarioMicroservice.csproj"

# Copiar todos os arquivos do projeto e compilar
COPY . .
WORKDIR "/src/FluxoCaixaDiarioMicroservice"
RUN dotnet --version
RUN dotnet build "FluxoCaixaDiarioMicroservice.csproj" -c Release -o /app/build

# Fase de publicação
FROM build AS publish
RUN dotnet publish "FluxoCaixaDiarioMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Fase final para execução do aplicativo
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxoCaixaDiarioMicroservice.dll"]




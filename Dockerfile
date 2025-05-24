# Etapa base con runtime de ASP.NET Core 9.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build con SDK de .NET 9.0
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de proyecto y restaura dependencias
COPY ["PlataformaNoticias.csproj", "./"]
RUN dotnet restore "PlataformaNoticias.csproj"

# Copia todo el código y compila en modo Release
COPY . .
RUN dotnet publish "PlataformaNoticias.csproj" -c Release -o /app/publish

# Imagen final para correr la app
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PlataformaNoticias.dll"]

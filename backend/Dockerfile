# 1. Build Aşaması
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Tüm solution'ı kopyala
COPY . .

# WebAPI projesine git ve publish yap
RUN dotnet restore WebAPI/WebAPI.csproj
RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o /app/out

# 2. Runtime Aşaması
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebAPI.dll"]
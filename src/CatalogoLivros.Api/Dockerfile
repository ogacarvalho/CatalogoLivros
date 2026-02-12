# Dockerfile (multi-stage) pra .NET 8

# Ideia:
# 1) Usar a imagem SDK só pra compilar/publicar (mais pesada)
# 2) Copiar o resultado pra imagem Runtime (mais leve e + seguro) e rodar

# Passo a passo:
# - Copia só os .csproj primeiro (pra aproveitar cache do Docker)
# - Roda restore (baixa dependências)
# - Copia o resto do código
# - Roda publish (gera arquivos finais)
# - Monta a imagem final só com o publish
# - Sobe a API na porta 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/CatalogoLivros.Dominio/CatalogoLivros.Dominio.csproj src/CatalogoLivros.Dominio/
COPY src/CatalogoLivros.Aplicacao/CatalogoLivros.Aplicacao.csproj src/CatalogoLivros.Aplicacao/
COPY src/CatalogoLivros.Infraestrutura/CatalogoLivros.Infraestrutura.csproj src/CatalogoLivros.Infraestrutura/
COPY src/CatalogoLivros.Api/CatalogoLivros.Api.csproj src/CatalogoLivros.Api/

RUN dotnet restore src/CatalogoLivros.Api/CatalogoLivros.Api.csproj

COPY . .

RUN dotnet publish src/CatalogoLivros.Api/CatalogoLivros.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CatalogoLivros.Api.dll"]

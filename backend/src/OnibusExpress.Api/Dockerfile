#  Atualiza para a versão 10.0 do SDK para casar com o seu projeto
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

#  Copia TODOS os arquivos de projeto da sua Clean Architecture
COPY *.sln ./
COPY src/OnibusExpress.Api/*.csproj ./src/OnibusExpress.Api/
COPY src/OnibusExpress.Infrastructure/*.csproj ./src/OnibusExpress.Infrastructure/
COPY src/OnibusExpress.Application/*.csproj ./src/OnibusExpress.Application/
COPY src/OnibusExpress.Domain/*.csproj ./src/OnibusExpress.Domain/

# Restaura as dependências (agora ele vai achar todas as camadas)
RUN dotnet restore src/OnibusExpress.Api/OnibusExpress.Api.csproj

# Copia o restante do código e compila a aplicação
COPY . ./
RUN dotnet publish src/OnibusExpress.Api/OnibusExpress.Api.csproj -c Release -o out

#  Atualiza também o Runtime para a versão 10.0
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "OnibusExpress.Api.dll"]
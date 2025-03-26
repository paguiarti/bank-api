# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os .csproj files
COPY ["src/BankAPI.API/BankAPI.API.csproj", "src/BankAPI.API/"]
COPY ["src/BankAPI.Application/BankAPI.Application.csproj", "src/BankAPI.Application/"]
COPY ["src/BankAPI.Core/BankAPI.Core.csproj", "src/BankAPI.Core/"]
COPY ["src/BankAPI.Infrastructure/BankAPI.Infrastructure.csproj", "src/BankAPI.Infrastructure/"]

# Restaura as dependências
RUN dotnet restore "src/BankAPI.API/BankAPI.API.csproj"

# Copia todo o código fonte
COPY . .

# Publica a aplicação
WORKDIR "/src/src/BankAPI.API"
RUN dotnet publish "BankAPI.API.csproj" -c Release -o /app/publish

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Instala pacotes necessários para configurar o locale
RUN apt-get update && apt-get install -y locales && \
    sed -i '/en_US.UTF-8/s/^# //g' /etc/locale.gen && \
    locale-gen

# Define as variáveis de ambiente para o locale
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US:en
ENV LC_ALL en_US.UTF-8

WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BankAPI.API.dll"]
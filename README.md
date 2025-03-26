# Sistema Caixa de Banco

API em .NET para gerenciamento de contas bancárias, transferências e histórico de transações.

Obs: Todos os horários são registrados em UTC para simplificação do ambiente.

## Projeto

Este projeto é uma aplicação em **.NET 8.0** utilizando **Arquitetura Limpa**, **SOLID** e **DDD (Domain-Driven Design)**. A aplicação usa **Entity Framework** para interação com o banco de dados **SQL Server**.

## Funcionalidades


- Cadastro de contas bancárias
  
A API permite o cadastro de contas bancárias para novos clientes, onde cada conta começa com um saldo inicial de R$1000 como bonificação.

- Consulta de conta bancária
  
Os usuários podem consultar os detalhes de uma conta bancária, incluindo o saldo atual e o status da conta (ativa ou inativa).

- Desativação de conta
  
A API permite a desativação de contas bancárias. Uma conta desativada não poderá realizar mais transações.

- Transferência de valores
  
A API permite a transferência de valores entre duas contas bancárias, desde que ambas as contas estejam ativas e a conta de origem tenha saldo suficiente para a transação.

## Tecnologias / Padrões utilizados

- **.NET 8.0**
- **Entity Framework**
- **SQL Server**
- **Arquitetura Limpa**
- **SOLID**
- **DDD**

## Pré-requisitos

Antes de rodar o projeto, certifique-se de ter os seguintes pré-requisitos instalados:

- **Docker**
- **Docker Compose**
- **Visual Studio 2022 ou superior**
- **SQL Server**

## Como Rodar o Projeto

### 1. Rodando o Projeto com Docker

1. Clone o repositório:

    ```bash
    git clone https://github.com/paguiarti/bank-api.git
    cd seu-projeto
    ```

2. Construa e inicie os containers Docker com o Docker Compose:

    ```bash
    docker-compose up --build
    ```

3. Acesse a aplicação em: `http://localhost:8080/swagger/index.html`

#### Comandos Docker úteis:

- Para rodar os containers em segundo plano:

    ```bash
    docker-compose up -d
    ```

- Para parar os containers:

    ```bash
    docker-compose down
    ```

- Para visualizar os logs:

    ```bash
    docker-compose logs
    ```

### 2. Rodando o Projeto no Visual Studio

1. Abra o **Visual Studio 2022 ou superior**.
2. Selecione **Open Project** e abra o arquivo `.sln` do projeto.
3. Clique em **Build > Build Solution** para compilar o projeto.
4. Após a compilação, clique em **Debug > Start Without Debugging** ou pressione `Ctrl + F5` para rodar a aplicação.

#### Comandos úteis para .NET:

- Para rodar o projeto via CLI:

    ```bash
    dotnet run
    ```

- Para rodar os testes unitários:

    ```bash
    dotnet test
    ```

- Para restaurar pacotes NuGet:

    ```bash
    dotnet restore
    ```

## Estrutura do Projeto

A arquitetura do projeto segue os princípios de **Arquitetura Limpa** e **DDD**. A aplicação está dividida em camadas, cada uma com sua responsabilidade:

- **Core (Domain)**: Entidades, agregados e interfaces do domínio.
- **Application**: Lógica de aplicação, casos de uso e serviços.
- **Infrastructure**: Implementações de acesso a dados e integrações externas.
- **WebAPI**: Interface HTTP, controllers e configuração de rotas.

## Banco de Dados

A aplicação utiliza **SQL Server** como banco de dados. A interação é feita via **Entity Framework Core**.

### Configuração do Banco de Dados

A string de conexão está configurada no arquivo `appsettings.json`. Caso esteja rodando localmente, certifique-se de ter o SQL Server configurado ou use o Docker para rodar o banco de dados.

### Migrations

Para aplicar migrações no banco de dados, use o comando:

```bash
dotnet ef database update

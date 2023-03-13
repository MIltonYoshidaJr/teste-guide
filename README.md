# teste-guide

Para construção da aplicação foi utilizado a versão 7 do asp.net core. A mesma pode ser instalada a partir do link:
https://dotnet.microsoft.com/en-us/learn/aspnet/hello-world-tutorial/install


# Banco de dados
A aplicação já está configurada para utilização de um banco de dados MS SQL Server, cujo qual está disponível para utilização nos testes. Os dados de conexão podem ser vistos no arquivo:
appsettings.json -> ConnectionStrings

Caso deseje utilizar um banco de dados próprio, mude as configurações da string de conexão do MS SQL Server no arquivo citado acima e execute os scripts abaixo. Caso opte por utilizar o banco já configurado, vá para a execução da aplicação.


## Criação da tabela de usuários
IF EXISTS (SELECT TOP 1 1  FROM sys.objects WHERE name = 'Users' AND type = 'U')
	BEGIN
		DROP TABLE dbo.Users
	END

CREATE TABLE dbo.Users (
	Id				INT IDENTITY(1, 1) NOT NULL,
	Username		VARCHAR(MAX) NOT NULL,
	Password		VARCHAR(MAX) NOT NULL,
CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


## Criação da tabela dos valores dos pregões
IF EXISTS (SELECT TOP 1 1  FROM sys.objects WHERE name = 'Ativos' AND type = 'U')
	BEGIN
		DROP TABLE dbo.Ativos
	END

CREATE TABLE dbo.Ativos (
	AtivoId			INT IDENTITY(1, 1) NOT NULL,
	Data			DATETIME NOT NULL,
	Indicador		DECIMAL(18, 15) NOT NULL,
CONSTRAINT [PK_Ativos] PRIMARY KEY CLUSTERED 
(
	[AtivoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


# Execução da aplicação
Para execução da aplicação será necessário os sdks do link acima. Após instalado executar:

dotnet restore
dotnet run --urls="https://localhost:5001"

Uma vez iniciada a aplicação, ela estará disponível no endereço: https://localhost:5001

Caso deseje visualizar o Swagger, basta acessar o endereço fornecido na execução: https://localhost:5001/swagger


# Endpoints disponíveis e suas respectivas chamadas.

## Auth/Create - Endpoint para criação de um usuário. E um ambiente produtivo, este endpoint deveria estar protegido, porém como estamos trabalhando com a hipótese de ser criado um banco de dados novo. Foi deixado aberto para que seja possível a criação de um novo usuário para aquisição do token.

url: https://localhost:5001/Auth/create
método: POST
Body json:
{
  "username": "<nome usuário>",
  "password": "<senha usuário>"
}

Caso esteja utilizando a base de dados disponível, este passo não será obrigatório.


## Auth/Login - Endpoint para login e retorno do token a ser utilizado nas demais chamadas.

url: https://localhost:5001/Auth/login
método: POST
Body json:
{
  "username": "<nome usuário>",
  "password": "<senha usuário>"
}

Caso esteja utilizando a base de dados fornecida, já existe um usuário disponível com as seguintes credenciais:

username: TesteGuide
password: Teste@Guide2023


## Ativos/UpdateDatabase - Endpoint para busca/atualização das informações disponibilizadas pelo Yahoo Finance. Caso seja a primeira execução, é interessante chamar este endpoint para que os dados sejam atualizados na base de dados.


url: https://localhost:5001/Ativos/UpdateDatabase
método: PATCH
headers: Authotization = Bearer {token obtido no endpoint de login}


## Ativos/GetAtivos - Endpoint para retorno dos dados bem como os cálculos das variáções em d-1 e relativo ao primeiro valor.

url: https://localhost:5001/Ativos/GetAtivos
método: GET
headers: Authotization = Bearer {token obtido no endpoint de login}

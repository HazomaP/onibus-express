# OniBus Express - API de Reservas

Este projeto consiste no desenvolvimento do backend do **OniBus Express**, uma plataforma de gerenciamento e reserva de passagens de ônibus. A aplicação foi projetada com foco em alta escalabilidade, isolamento de componentes e regras de negócio robustas, estando totalmente preparada para ambientes de produção conteinerizados.

---

##  Tecnologias Utilizadas

* **Runtime:** .NET 10.0 (C#)
* **Mapeamento Objeto-Relacional (ORM):** Entity Framework Core 10
* **Banco de Dados:** PostgreSQL 15
* **Conteinerização:** Docker & Docker Compose
* **Documentação:** Swagger UI (OpenAPI)

---

##  Arquitetura do Sistema

O projeto adota os princípios da **Clean Architecture** combinados com **Domain-Driven Design (DDD)**, garantindo a separação estrita de responsabilidades em quatro camadas bem definidas:

1. **OnibusExpress.Domain:** Contém as entidades de negócio (`Rota`, `Viagem`, `Reserva`, `Passageiro`), agregados e validações essenciais. É uma camada livre de dependências externas.
2. **OnibusExpress.Application:** Implementa os casos de uso do sistema, como a lógica coordenada do serviço de criação e gestão de reservas de assentos.
3. **OnibusExpress.Infrastructure:** Responsável pela persistência de dados, incluindo a configuração do `DbContext` e repositórios mapeados para o PostgreSQL.
4. **OnibusExpress.Api:** Ponto de entrada do sistema contendo os controladores REST, mapeamentos de rotas e configurações do Swagger.

---

##  Regras de Negócio Implementadas

* **Validação Estrutural:** Implementação de um validador algorítmico customizado para CPFs na camada de recepção de dados, impedindo o processamento de documentos inválidos.
* **Consistência de Estado (Garantia de Assentos):** Mecanismo de bloqueio concorrente que impede a reserva duplicada do mesmo assento na mesma viagem.
* **Trava de Segurança de Cancelamento:** Uma reserva só pode ser cancelada se a operação for realizada com uma antecedência mínima de **2 horas** em relação ao horário de partida agendado para a viagem.

---

##  Como Executar o Projeto via Docker

A infraestrutura foi totalmente automatizada utilizando Docker Compose. Não é necessário ter o SDK do .NET ou o PostgreSQL instalados localmente na máquina hospedeira.

### Pré-requisitos
* Docker e Docker Compose instalados e em execução.

### Passo a Passo

1. Clone o repositório para o seu ambiente local.
2. Abra o terminal na pasta raiz do projeto (onde o arquivo `docker-compose.yml` está localizado).
3. Execute o comando para compilar as imagens e subir os serviços:

```bash
docker-compose up --build

# Documentação da API

Assim que o terminal indicar que a aplicação foi iniciada com sucesso, a documentação interativa e os endpoints para testes estarão acessíveis através do Swagger:

URL de Acesso: http://localhost:5277/swagger

Endpoints Disponíveis:
GET /api/viagens - Lista todas as viagens e rotas disponíveis.

GET /api/viagens/{id} - Consulta os detalhes de uma viagem específica.

POST /api/reservas - Realiza a reserva de um assento em uma viagem.

GET /api/reservas/{codigo} - Consulta o status e informações de uma reserva através do código localizador.

DELETE /api/reservas/{codigo} - Cancela uma reserva ativa (sujeito à regra de antecedência de 2 horas).
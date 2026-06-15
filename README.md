# OniBus Express - API de Reservas

Este repositório contém a solução de backend para o desafio técnico do OniBus Express. A API foi projetada para gerenciar rotas, viagens, passageiros e reservas de assentos, respeitando regras de negócio e garantindo a consistência dos dados.

---

## Tecnologias e Bibliotecas Utilizadas

* **.NET 10 (C#):** Escolhido por sua alta performance, tipagem forte e maturidade para a construção de APIs corporativas seguras e escaláveis.
* **Entity Framework Core 10:** ORM adotado para agilizar o mapeamento objeto-relacional, garantindo um controle seguro das migrations e do data seeding automático do banco de dados.
* **PostgreSQL 15:** Banco de dados relacional robusto, escolhido por sua confiabilidade em transações e consistência de dados (ACID).
* **Docker & Docker Compose:** Utilizados para garantir a paridade entre ambientes, eliminando divergências de configuração entre o desenvolvimento local e a máquina de avaliação.
* **Swashbuckle (Swagger):** Adotado para gerar a documentação interativa da API de forma automatizada, facilitando a validação e os testes manuais dos endpoints.
* **xUnit:** Framework utilizado para a implementação da suíte de testes unitários e de comportamento das regras de domínio.

---

## Decisões de Arquitetura Relevantes

1. **Clean Architecture e DDD:** O código C# foi estruturado em camadas (Domain, Application, Infrastructure, Api). Isso isola as regras de negócio vitais dos detalhes de infraestrutura e frameworks web, facilitando a manutenção e a criação de testes isolados.
2. **Tratamento Universal de Fuso Horário (UTC):** Para evitar bugs de concorrência e divergência de relógios locais entre a API e o PostgreSQL, toda a persistência e a comunicação de datas ocorrem estritamente no formato UTC (ISO 8601).
3. **Consistência de Estado (Garantia de Assentos):** A lógica de ocupação de assentos é validada diretamente no servidor no momento da inserção, prevenindo race conditions (evitando que dois usuários consigam reservar o mesmo assento na mesma fração de segundo).

---

## O que foi implementado 

**O que foi implementado (MVP Backend Completo):**
* [x] API RESTful completa conectada ao PostgreSQL.
* [x] Infraestrutura Docker conteinerizada (Banco de Dados e Aplicação).
* [x] Execução automática de Migrations e Data Seeding ao iniciar o container.
* [x] Algoritmo nativo de validação de CPF na camada de Domínio/Aplicação.
* [x] Regra de negócio: Trava de segurança que proíbe o cancelamento de uma reserva com menos de 2 horas de antecedência para a partida.
* [x] Suíte de testes automatizados integrada para validação das regras de cancelamento.
* [x] Documentação interativa via Swagger UI.



---

## Como rodar a aplicação localmente

### Pré-requisitos Gerais
* Git instalado para clonagem do repositório.

---

### Opção 1: Execução com Docker (Recomendado)
Esta é a maneira mais rápida e isolada. Não é necessário ter o SDK do .NET ou o PostgreSQL instalados na máquina hospedeira.

**Pré-requisitos adicionais:** Docker e Docker Compose instalados e em execução.

**Passo a Passo:**

1. Abra o terminal na pasta raiz do projeto (onde está localizado o arquivo `docker-compose.yml`).

2. Execute o comando para compilar as imagens e subir os serviços:

   ```bash
   docker-compose up --build
Aguarde o terminal indicar que o banco de dados está pronto e que a API iniciou (Application started.). As migrations e o banco de testes serão configurados automaticamente nesta etapa.

Acesse a documentação interativa e realize os testes dos endpoints diretamente no navegador através da URL:
http://localhost:5277/swagger

### Opção 2: Execução sem Docker (Modo Local)
Caso queira debugar o código fonte diretamente no ambiente de desenvolvimento local.

Pré-requisitos adicionais: SDK do .NET 10 instalado e um servidor PostgreSQL ativo (porta default 5432).

Passo a Passo:

Abra o arquivo appsettings.Development.json localizado em src/OnibusExpress.Api/.

Certifique-se de ajustar a string de conexão DefaultConnection com o usuário, senha e host do seu PostgreSQL local.

No terminal, navegue até a pasta raiz do backend (onde se encontra o arquivo de solução .sln).

Execute o comando para aplicar as migrations e criar a estrutura do banco de dados local:

Bash
dotnet ef database update --project src/OnibusExpress.Infrastructure --startup-project src/OnibusExpress.Api

Inicie a aplicação:

Bash
dotnet run --project src/OnibusExpress.Api
Acesse o Swagger no navegador: http://localhost:5277/swagger

Como rodar os testes automatizados

A suíte de testes valida o comportamento das regras de negócio sem depender do banco de dados físico, garantindo execuções rápidas e isoladas.

Abra o terminal e certifique-se de estar na pasta raiz do backend (a pasta que contém o arquivo OnibusExpress.sln).

Execute o comando de testes do .NET CLI:

Bash
dotnet test

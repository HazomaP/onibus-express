#  OniBus Express - API de Reservas

Este repositório contém a solução de backend para o desafio técnico do **OniBus Express**. A API foi projetada para gerenciar rotas, viagens, passageiros e reservas de assentos, respeitando regras rigorosas de negócio e garantindo a consistência dos dados em um ambiente altamente concorrente.

---

##  Tecnologias e Bibliotecas Utilizadas 

* **.NET 10 (C#):** Escolhido por sua alta performance, tipagem forte e maturidade para a construção de APIs corporativas seguras e escaláveis.
* **Entity Framework Core 10:** ORM adotado para agilizar o mapeamento objeto-relacional, garantindo um controle seguro das *migrations* e do *data seeding* do banco de dados.
* **PostgreSQL 15:** Banco de dados relacional robusto, escolhido por sua confiabilidade em transações e consistência de dados (ACID).
* **Docker & Docker Compose:** Utilizados para garantir a paridade entre ambientes (o código roda da mesma forma no meu ambiente de desenvolvimento e na máquina do avaliador), eliminando o problema clássico de "na minha máquina funciona".
* **Swashbuckle (Swagger):** Adotado para gerar a documentação interativa da API de forma automatizada, facilitando os testes manuais dos endpoints.

---

##  Decisões de Arquitetura Relevantes

1. **Clean Architecture e DDD:** O código C# foi estruturado em camadas (`Domain`, `Application`, `Infrastructure`, `Api`). Isso isola as regras de negócio vitais (como a validação de CPF e bloqueio de cancelamento) dos detalhes de tecnologia (como banco de dados e frameworks web), facilitando a manutenção e a criação de testes.
2. **Tratamento Universal de Fuso Horário (UTC):** Para evitar bugs de concorrência e divergência de relógios locais, foi decidido que a persistência e a comunicação de datas na API e no PostgreSQL ocorrem estritamente no formato UTC.
3. **Consistência de Estado (Garantia de Assentos):** A lógica de ocupação de assentos é validada diretamente no servidor no momento da inserção, prevenindo *race conditions* (evitando que dois usuários consigam reservar o mesmo assento na mesma fração de segundo).

---

## ✅O que foi implementado 

**O que foi implementado (MVP Backend Completo):
*  API RESTful completa conectada ao PostgreSQL.
* Infraestrutura Docker conteinerizada (Banco de Dados e Aplicação).
*  Algoritmo nativo de validação de CPF na camada de Domínio/Aplicação.
*  Regra de negócio: Trava de segurança que proíbe o cancelamento de uma reserva com menos de 2 horas de antecedência para a partida.
*  Documentação interativa via Swagger UI.



---

##  Como rodar o projeto localmente

### Opção 1: COM Docker (Recomendado)
Esta é a maneira mais rápida. Você não precisa ter o SDK do .NET ou o PostgreSQL instalados na sua máquina.

1. Clone o repositório e abra o terminal na pasta raiz do projeto (onde está o arquivo `docker-compose.yml`).

2. Execute o comando para compilar a imagem e subir a infraestrutura:
   ```bash
   docker-compose up --build
O serviço de banco de dados subirá e o Entity Framework aplicará as migrations e o seed de dados automaticamente.

Acesse a documentação interativa para testar os endpoints em: http://localhost:5277/swagger

Opção 2: SEM Docker (Modo de Desenvolvimento Local)
Caso queira debugar o código fonte diretamente no IDE (ex: Visual Studio ou VS Code):

Certifique-se de ter um servidor PostgreSQL rodando localmente (porta 5432).

Atualize a chave DefaultConnection dentro do arquivo appsettings.Development.json (localizado em src/OnibusExpress.Api) com as credenciais do seu banco de dados local.

No terminal, navegue até a pasta principal da API e inicie a aplicação:

Bash
cd src/OnibusExpress.Api
dotnet run
Acesse http://localhost:5277/swagger no seu navegador.

🧪 Como rodar os testes
A arquitetura foi construída pensando na testabilidade das regras de domínio e casos de uso. Para executar a suíte de testes automatizados do .NET:

Abra o terminal na pasta raiz do repositório.

Execute o comando padrão do .NET CLI:

Bash
dotnet test
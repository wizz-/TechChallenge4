**Validações**

O ContatoAppService é uma classe responsável por gerenciar as operações relacionadas aos contatos. Ele utiliza um repositório de contatos (IContatoRepository) para acessar os dados e um mapeador (IContatoAppMapper) para converter entre as entidades de domínio e os Data Transfer Objects (DTOs).

![image.png](/.attachments/image-774bda60-375d-4f3a-99ba-a3918b1c29dd.png)

- Serviço de Contato

- Obter Todos os Contatos: Usa o repositório para buscar e mapear contatos para DTOs.

- Inserir Novo Contato: Mapeia o DTO para uma entidade de domínio e insere no repositório.

- Obter Contato por ID: Busca a entidade pelo ID no repositório e mapeia para um DTO.

- Atualizar Contato: Busca a entidade pelo ID, atualiza os dados e salva no repositório.

- Excluir Contato: Busca a entidade pelo ID e exclui do repositório.

- Obter Contatos por DDD: Busca as entidades de domínio pelo DDD no repositório, mapeia as entidades para DTOs.


**Persistência de Dados** 

Dapper é um micro ORM (Object-Relational Mapper) para .NET, conhecido por sua eficiência e performance ao mapear objetos do banco de dados para objetos de aplicação. Escolhemos usar Dapper na nossa API devido à sua confiabilidade e por ser uma excelente camada de armazenamento de dados. Ele permite operações rápidas e eficazes com o banco de dados, garantindo uma integração simples e robusta entre nossa aplicação e a persistência de dados.

Em referencia ao repositório- Infradatadapper

![image.png](/.attachments/image-8555ad50-a3a2-473c-ba84-b10504365d94.png)

- TechChallenge1.Infra.Data: Este projeto usa o framework net8.0 e depende de pacotes como Dapper, Microsoft.Data.SqlClient e 

- Microsoft.Extensions.Configuration.Abstractions. Ele também faz referência ao projeto TechChallenge1.Domain.
TechChallenge1.Domain: Este projeto também usa o framework net8.0 e não tem dependências listadas especificamente no código fornecido.





CatalogoLivros API

Projeto simples de API para cadastro de livros com .NET 8.

Tambem tem uma funcionalidade extra para upload de capa no AWS S3.

Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker + Docker Compose
- AWS S3

O que a API faz

- Criar livro
- Listar livros
- Buscar livro por id
- Atualizar livro
- Deletar livro
- Enviar capa de livro para o S3

Como rodar local

 1) Clonar o projeto

```bash
git clone <url-do-repo>
cd CatalogoLivros
```

 2) Criar arquivo `.env`

Crie um arquivo `.env` na raiz, usando como base o `.env.example`.

Exemplo:

```env
AWS_ACCESS_KEY_ID=YOUR_AWS_ACCESS_KEY_ID
AWS_SECRET_ACCESS_KEY=YOUR_AWS_SECRET_ACCESS_KEY
AWS_REGION=us-east-1
Storage__S3__BucketName=your-bucket-name
```

 3) Subir com Docker

```bash
docker compose up --build
```

API sobe em:

- `http://localhost:8080`

Swagger:

- `http://localhost:8080/swagger`

Testando os endpoints

 Criar livro

```bash
curl -X POST http://localhost:8080/api/livros \
  -H "Content-Type: application/json" \
  -d '{"titulo":"Clean Code","autor":"Robert C. Martin","anoLancamento":2008}'
```

 Listar livros

```bash
curl http://localhost:8080/api/livros
```

 Enviar capa (S3)

Troque `{id}` pelo id do livro e ajuste o caminho da imagem:

```bash
curl -X POST "http://localhost:8080/api/livros/{id}/capa" \
  -F "arquivo=@/caminho/da/imagem.jpg"
```

Regras do upload de capa

- So aceita arquivo de imagem (`image/*`)
- Tamanho maximo: 5 MB
- Salva no S3 na pasta `capas/`

Sobre acesso da imagem no S3

Se a `urlCapa` abrir com `AccessDenied`, precisa liberar leitura no bucket para `capas/*`
ou mudar a estrategia para URL assinada (presigned URL).

Observacoes importantes

- O arquivo `.env` esta no `.gitignore` (nao sobe para o GitHub)
- Nao colocar chaves AWS no codigo
- Este projeto usa `EnsureCreated` no banco para facilitar ambiente local


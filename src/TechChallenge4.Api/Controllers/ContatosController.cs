using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using Prometheus;
using System.Text.Json;
using TechChallenge4.Api.Dtos;
using TechChallenge4.Api.Interfaces.Mappers;
using TechChallenge4.Api.Interfaces.Services;
using TechChallenge4.Application.Contatos.Dto;
using TechChallenge4.Application.Contatos.Interfaces;

namespace TechChallenge4.Api.Controllers
{
    public class ContatosController : ICarterModule
    {
        private static readonly Counter RequestCounter = Metrics.CreateCounter("http_requests_total", "Total number of HTTP requests", new[] { "method", "endpoint", "status" });

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var grupo = app.MapGroup("/api/contatos");

            grupo.MapGet("", ObterContatos)
                .WithSummary("Obtém contatos")
                .WithDescription("Retorna todos os contatos ou contatos filtrados por DDD do telefone cadastrados no banco de dados");

            grupo.MapGet("{id:int}", ObterContatoPorId)
                .WithSummary("Obtém um contato pelo Id")
                .WithDescription("Retorna um contatos cadastrado no banco de dados pelo Id");

            grupo.MapPost("", CriarContato)
                .WithSummary("Grava um novo contato")
                .WithDescription("Grava um novo contato no banco de dados");

            grupo.MapPut("{id}", AtualizarContatoCompleto)
                .WithSummary("Atualiza um contato inteiro")
                .WithDescription("Atualiza um contato inteiro no banco de dados");

            grupo.MapPatch("{id}", AtualizarContatoParcialmente)
                .WithSummary("Atualiza um contato parcialmente")
                .WithDescription("Atualiza um contato parcialmente no banco de dados");

            grupo.MapDelete("{id}", ExcluirContato)
                .WithSummary("Exclui um contato")
                .WithDescription("Exclui um contato no banco de dados");
        }

        public Results<Ok<IEnumerable<ContatoAppDto>>, NotFound<ErroDto>> ObterContatos(IContatoAppService contatoAppService, string ddd)
        {
            IEnumerable<ContatoAppDto> retorno;

            if (!string.IsNullOrEmpty(ddd))
            {
                retorno = contatoAppService.ObterPorDdd(ddd);
            }
            else
            {
                retorno = contatoAppService.ObterTodos();
            }

            if (!retorno.Any())
            {
                return TypedResults.NotFound(new ErroDto { Mensagem = "Nenhum contato encontrado." });
            }

            return TypedResults.Ok(retorno);
        }

        public Results<Ok<ContatoAppDto>, NotFound<ErroDto>> ObterContatoPorId(int id, IContatoAppService contatoAppService)
        {
            var retorno = contatoAppService.ObterPorId(id);
            if (retorno == null) return TypedResults.NotFound(new ErroDto() { Mensagem = "Nenhum contato encontrado." });
            return TypedResults.Ok(retorno);
        }

        public IResult CriarContato(ContatoDto contatoDto, IMensageriaDeContatosService contatoService)
        {
            contatoService.EnviarContatoParaGravacao("FilaParaInserir", contatoDto);
            return TypedResults.Ok("Contato enviado para processamento.");
        }

        public IResult AtualizarContatoCompleto(int id, ContatoDto contatoDto, IMensageriaDeContatosService contatoService)
        {
            contatoDto.Id = id;
            contatoService.EnviarContatoParaGravacao("FilaParaAlterar", contatoDto);
            return TypedResults.Ok("Contato enviado para processamento.");
        }

        public Results<Ok<ContatoAppDto>, BadRequest<ErroDto>, NotFound<ErroDto>> AtualizarContatoParcialmente(
            int id,
            IContatoAppService contatoAppService,
            JsonElement dadosParaAlterar)
        {
            if (id <= 0) return TypedResults.BadRequest(new ErroDto() { Mensagem = "Para incluir novo contato, utilize o método HTTP POST." });

            try
            {
                var contatoBd = contatoAppService.ObterPorId(id);
                if (contatoBd == null) return TypedResults.NotFound(new ErroDto() { Mensagem = "Contato não encontrado." });

                foreach (var prop in dadosParaAlterar.EnumerateObject())
                {
                    switch (prop.Name.ToLower())
                    {
                        case "nome":
                            contatoBd.Nome = prop.Value.GetString();
                            break;
                        case "email":
                            contatoBd.Email = prop.Value.GetString();
                            break;
                        case "telefone":
                            contatoBd.Telefone = prop.Value.GetString();
                            break;
                        default:
                            return TypedResults.BadRequest(new ErroDto { Mensagem = $"A propriedade {prop.Name} não pertence ao contato." });
                    }
                }

                contatoAppService.Atualizar(contatoBd);
                return TypedResults.Ok(contatoBd);
            }
            catch (ArgumentException erro)
            {
                return TypedResults.BadRequest(new ErroDto() { Mensagem = "Erro ao atualizar contato", Detalhes = erro.Message });
            }
        }

        public Results<Ok<string>, BadRequest<ErroDto>> ExcluirContato(int id, IMensageriaDeContatosService contatoService)
        {
            if (id <= 0) return TypedResults.BadRequest(new ErroDto() { Mensagem = "Informe um id válido." });

            contatoService.EnviarMensagemDeExclusao("FilaParaExcluir", id);
            return TypedResults.Ok("Contato enviado para processamento.");
        }
    }
}

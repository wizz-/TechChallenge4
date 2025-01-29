using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechChallenge4.Api;
using TechChallenge4.Api.Dtos;
using TechChallenge4.Domain.Entities;
using TechChallenge4.Infra.Data.Ef.Context;

namespace TechChallenge4.Tests.Integration
{
    public class ContatoIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public ContatoIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    RemoverSqlContext(services);
                    AdicionarInMemorianContext(services);
                });

            });

            _client = _factory.CreateClient();
        }

        private void RemoverSqlContext(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<Contexto>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        private void AdicionarInMemorianContext(IServiceCollection services)
        {
            services.AddDbContext<Contexto>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        }

        private void InicializarDatabase()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Contexto>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetContatos_DeveRetornarNotFoundQuandoNaoHouverNadaNoBD()
        {
            // Arrange
            InicializarDatabase();

            // Act
            var response = await _client.GetAsync("/api/contatos");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetContatos_DeveRetornarUmContatoPorId()
        {
            // Arrange
            InicializarDatabase();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Contexto>();
            var contatoDb = context.Set<Contato>();
            contatoDb.Add(new Contato("João Silva", "11998974613", "joao.silva@postech.com.br"));
            contatoDb.Add(new Contato("Gabriela Amarantus", "21998974332", "gabi.amarantus@postech.com.br"));
            context.SaveChanges();

            // Act
            var response = await _client.GetAsync("/api/contatos/1");
            var conteudo = await response.Content.ReadAsStringAsync();
            var contato = JsonConvert.DeserializeObject<Contato>(conteudo);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("João Silva", contato?.Nome);
        }

        [Fact]
        public async Task GetContatos_DeveRetornarAListaDeTodosContatosCadastrados()
        {
            // Arrange
            InicializarDatabase();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Contexto>();
            var contatoDb = context.Set<Contato>();            
            contatoDb.Add(new Contato("João Silva", "11998974613", "joao.silva@postech.com.br"));
            contatoDb.Add(new Contato("Gabriela Amarantus", "21998974332", "gabi.amarantus@postech.com.br"));            
            context.SaveChanges();

            // Act
            var response = await _client.GetAsync("/api/contatos");
            var conteudo = await response.Content.ReadAsStringAsync();
            var listaDaResponse = JsonConvert.DeserializeObject<List<Contato>>(conteudo);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, listaDaResponse?.Count);
        }

        [Fact]
        public async Task GetContatos_DeveRetornarContatosFiltradosPorDdd()
        {
            // Arrange
            InicializarDatabase();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Contexto>();
            var contatoDb = context.Set<Contato>();
            contatoDb.Add(new Contato("João Silva", "11998974613", "joao.silva@postech.com.br"));
            contatoDb.Add(new Contato("Gabriela Amarantus", "21998974332", "gabi.amarantus@postech.com.br"));
            contatoDb.Add(new Contato("Naofumi Iwatani", "11928574232", "naofumi@postech.com.br"));
            context.SaveChanges();

            // Act
            var response = await _client.GetAsync("/api/contatos?ddd=11");
            var conteudo = await response.Content.ReadAsStringAsync();
            var listaDaResponse = JsonConvert.DeserializeObject<List<Contato>>(conteudo);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, listaDaResponse?.Count);
        }

        [Fact]
        public async Task GetContatos_DeveRetornarNotFoundQuandoNaoEncontrarUmContatoComDddPesquisado()
        {
            // Arrange
            InicializarDatabase();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Contexto>();
            var contatoDb = context.Set<Contato>();
            contatoDb.Add(new Contato("João Silva", "11998974613", "joao.silva@postech.com.br"));
            contatoDb.Add(new Contato("Gabriela Amarantus", "21998974332", "gabi.amarantus@postech.com.br"));
            contatoDb.Add(new Contato("Naofumi Iwatani", "11928574232", "naofumi@postech.com.br"));
            context.SaveChanges();

            // Act
            var response = await _client.GetAsync("/api/contatos?ddd=82");            

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostContatos_DeveCadastrarUmContatoValido()
        {
            // Arrange
            InicializarDatabase();
            var contato = new Contato("João Silva","11998974613","joao.silva@fiaptech.com.br");
            var content = new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/contatos", content);
            var conteudo = await response.Content.ReadAsStringAsync();
            var contatoRetornado = JsonConvert.DeserializeObject<Contato>(conteudo);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(contatoRetornado?.Nome, contato.Nome);
        }

        [Fact]
        public async Task PostContatos_DeveRejeitarUmContatoInvalido()
        {
            // Arrange
            InicializarDatabase();
            var contato = new { ID = 0, Nome = "João Silva", Telefone = "998974613", Email = "joao.silva@postech.com.br" };
            var content = new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/contatos", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutContatos_DeveAlterarUmContato()
        {
            // Arrange
            InicializarDatabase();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Contexto>();
            var contatoDb = context.Set<Contato>();
            contatoDb.Add(new Contato("João Silva", "11998974613", "joao.silva@fiaptech.com.br"));
            context.SaveChanges();

            var contatoAlterar = new { Nome = "João da Silva", Telefone = "11998912313", Email = "joao.silva2@fiaptech.com.br" };
            var content = new StringContent(JsonConvert.SerializeObject(contatoAlterar), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/contatos/1", content);
            var conteudo = await response.Content.ReadAsStringAsync();
            var contatoRetornado = JsonConvert.DeserializeObject<Contato>(conteudo);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contatoAlterar.Nome, contatoRetornado?.Nome);
            Assert.Equal(contatoAlterar.Telefone, contatoRetornado?.Telefone);
            Assert.Equal(contatoAlterar.Email, contatoRetornado?.Email);
        }

        [Fact]
        public async Task DeleteContatos_DeveExcluirUmContato()
        {
            // Arrange
            InicializarDatabase();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Contexto>();
            var contatoDb = context.Set<Contato>();
            contatoDb.Add(new Contato("João Silva", "11998974613", "joao.silva@fiaptech.com.br"));
            context.SaveChanges();

            // Act
            var response = await _client.DeleteAsync("/api/contatos/1");
            var responseGet = await _client.GetAsync("/api/contatos/1");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
        }
    }
}

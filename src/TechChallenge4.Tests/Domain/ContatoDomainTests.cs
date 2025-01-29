using TechChallenge4.Domain.Entities;
using TechChallenge4.Infra.Exception.Repository;

namespace TechChallenge4.Tests.Domain
{
    public class ContatoDomainTests
    {
        [Fact]
        public void Contato_Deve_Sucesso_CriarUmContatoValido()
        {
            // Arrange            
            string nomeValido = "Thiago Rosa";
            string telefoneValido = "11998075544";
            string emailValido = "thirosa@gmail.com";

            // Act  
            var contato = new Contato(nomeValido, telefoneValido, emailValido);

            //Assert
            Assert.True(contato != null);
        }

        [Theory]
        // Teste com Telefone Celulares
        [InlineData("Thiago Rosa", "11998075544", "eu@eu.com")]
        [InlineData("Thiago Rosa", "(11)998075544", "eu@eu.com")]
        [InlineData("Thiago Rosa", "(11) 99807-5544", "eu@eu.com")]
        [InlineData("Thiago Rosa", "(11)99807-5544", "eu@eu.com")]
        // Teste com Telefone Fixos
        [InlineData("Thiago Rosa", "2133992266", "eu@eu.com")]
        [InlineData("Thiago Rosa", "(21)33992266", "eu@eu.com")]
        [InlineData("Thiago Rosa", "(21) 3399-2266", "eu@eu.com")]
        [InlineData("Thiago Rosa", "(21)3399-2266", "eu@eu.com")]
        public void Contato_Deve_Sucesso_CriarUmContatoValido_VariasFormatosDeTelefone(string nome, string telefone, string email)
        {
            // Arrange

            // Act      
            var action = new Action(() => { new Contato(nome, telefone, email); });

            //Assert
            var exception = Record.Exception(action);
            Assert.Null(exception);
            Assert.NotEqual(typeof(ArgumentException), exception?.GetType());
            Assert.NotEqual(MessageDefaultExceptionRepository.Argument_IncorretFormatPhone, exception?.Message);
            Assert.NotEqual(string.Format(MessageDefaultExceptionRepository.Argument_InvalidDDD, "ddd"), exception?.Message);
        }

        [Theory]
        [InlineData("Thiago Rosa", "11998075544", "eu@eu.com")]
        [InlineData("Thiago Rosa", "11998075544", "eu.sobrenome@eu.com")]
        [InlineData("Thiago Rosa", "11998075544", "eu.primeirosobrenome.segundosobrenome@eu.com")]
        [InlineData("Thiago Rosa", "11998075544", "eu@eu.com.br")]
        [InlineData("Thiago Rosa", "11998075544", "eu.sobrenome@eu.com.br")]
        [InlineData("Thiago Rosa", "11998075544", "eu.primeirosobrenome.segundosobrenome@eu.com.br")]
        [InlineData("Thiago Rosa", "11998075544", "eu_%+-_%+-@eu.com")]
        [InlineData("Thiago Rosa", "11998075544", "eu_%+-_%+-@eu.com.br")]
        public void Contato_Deve_Sucesso_CriarUmContatoValido_VariosFormatosDeEmail(string nome, string telefone, string email)
        {
            // Arrange

            // Act    
            var action = new Action(() => { new Contato(nome, telefone, email); });

            //Assert
            var exception = Record.Exception(action);
            Assert.Null(exception);
            Assert.NotEqual(typeof(ArgumentException), exception?.GetType());
            Assert.NotEqual(MessageDefaultExceptionRepository.Argument_IncorretFormatEmail, exception?.Message);
            Assert.NotEqual(string.Format(MessageDefaultExceptionRepository.Argument_IsNotNullParameterContato, nameof(email)), exception?.Message);

        }

        [Theory]
        [InlineData(null, "11998075544", "eu@eu.com")] // Validando com campo null
        [InlineData("", "11998075544", "eu@eu.com")] // Validando com campo vazio
        public void Contato_Deve_RetornarArgumentEmptyOrWhiteSpaceExceptionQuandoTentarCriarContatoSemNome(string nome, string telefone, string email)
        {
            // Arrange
           
            // Act            
            var action = new Action(() => { new Contato(nome, telefone, email); });

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal(string.Format(MessageDefaultExceptionRepository.Argument_IsNotNullParameterContato, nameof(nome)), exception.Message);
        }

        [Theory]
        [InlineData("Yasmin Vit�ria", null, "eu@eu.com")] // Validando com campo null
        [InlineData("Giuliano Tabata", "", "eu@eu.com")] // Validando com campo vazio
        public void Contato_Deve_RetornarArgumentNullExceptionQuandoTentarCriarContatoSemTelefone(string nome, string telefone, string email)
        {
            // Arrange

            // Act            
            var action = new Action(() => { new Contato(nome, telefone, email); });

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal(string.Format(MessageDefaultExceptionRepository.Argument_IsNotNullParameterContato, nameof(telefone)), exception.Message);
        }

        [Theory]
        [InlineData("Thiago Rosa", "998075544", "thirosa@gmail.com")] // Telefone com quantidade numeros insuficiente
        [InlineData("Giuliano Tabata", "(017) 98584-2620", "giuliano_pongeluppe@hotmail.com")] // Telefone com um '0' a mais no DDD
        [InlineData("Yasmin Vit�ria", "(14) 75700-9424", "dinizyasminvitoria@gmail.com")] // Telefone celular que n�o come�a com 9
        public void Contato_Deve_RetornarArgumentInvalidExceptionQuandoTentarCriarContatoComTelefoneIncorreto(string nome, string telefone, string email)
        {
            // Arrange

            // Act            
            var action = new Action(() => { new Contato(nome, telefone, email); });

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal(MessageDefaultExceptionRepository.Argument_IncorretFormatPhone, exception.Message);
        }

        [Theory]
        [InlineData("Giuliano Tabata", "(72) 98584-2620", "giuliano_pongeluppe@hotmail.com", "72")] // Telefone com um DDD que n�o existe, '72' n�o existe      
        [InlineData("Junio Vicente", "40 4803-6623", "junio_vicente@outlook.com", "40")] // Telefone com um DDD que n�o existe, '40' n�o existe
        public void Contato_Deve_RetornarArgumentInvalidExceptionQuandoTentarCriarContatoComDDDNaoExiste(string nome, string telefone, string email, string dddInvalido)
        {
            // Arrange

            // Act            
            var action = new Action(() => { new Contato(nome, telefone, email); });

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal(string.Format(MessageDefaultExceptionRepository.Argument_InvalidDDD, dddInvalido), exception.Message);
        }

        [Theory]
        [InlineData("Giuliano Tabata", "11998075544", null)] // Validando com campo null
        [InlineData("Junio Vicente", "11998075544", "")] // Validando com campo vazio
        public void Contato_Deve_RetornarArgumentEmptyOrWhiteSpaceExceptionQuandoTentarCriarContatoSemEmail(string nome, string telefone, string email)
        {
            // Arrange

            // Act            
            var action = new Action(() => { new Contato(nome, telefone, email); });

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal(string.Format(MessageDefaultExceptionRepository.Argument_IsNotNullParameterContato, nameof(email)), exception.Message);
        }

        [Theory]
        [InlineData("Thiago Rosa", "(17) 98584-2620", "thirosagmail.com")] // E-mail sem o '@'
        [InlineData("Giuliano Tabata", "(17) 98584-2620", "giuliano_pongeluppe@hotmail")] // E-mail sem o complemento
        [InlineData("Yasmin Vit�ria", "(17) 98584-2620", "@gmail.com")] // E-mail sem o prefixo
        [InlineData("Junio Vicente", "(17) 98584-2620", "junio_vicente@@outlook.com")] // E-mail com dois '@@'
        public void Contato_Deve_RetornarArgumentInvalidExceptionQuandoTentarCriarContatoComEmailInvalidos(string nome, string telefone, string email)
        {
            // Arrange

            // Act            
            var action = new Action(() => { new Contato(nome, telefone, email); });

            //Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal(MessageDefaultExceptionRepository.Argument_IncorretFormatEmail, exception.Message);
        }


    }
}
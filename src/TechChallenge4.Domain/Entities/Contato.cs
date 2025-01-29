
using System.Text.RegularExpressions;
using TechChallenge4.Domain.Enums;
using TechChallenge4.Infra.Exception.Repository;

namespace TechChallenge4.Domain.Entities
{
    public class Contato
    {
        private const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }

        protected Contato() { }

        public Contato(string nome, string telefone, string email)
        {
            ValidarNome(nome);
            ValidarEmail(email);

            Nome = nome;
            Telefone = ObterTelefoneValidado(telefone);
            Email = email;
        }

        public void AtualizarDados(string nome, string telefone, string email)
        {
            ValidarNome(nome);
            ValidarEmail(email);

            Nome = nome;
            Telefone = ObterTelefoneValidado(telefone);
            Email = email;
        }

        private void ValidarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome)) throw new ArgumentException(string.Format(MessageDefaultExceptionRepository.Argument_IsNotNullParameterContato, nameof(nome)));
        }

        private string ObterTelefoneValidado(string telefone)
        {
            if (string.IsNullOrEmpty(telefone)) throw new ArgumentException(string.Format(MessageDefaultExceptionRepository.Argument_IsNotNullParameterContato, nameof(telefone)));

            var match = Regex.Match(telefone, @"^\(?(?<DDD>\d{2})\)?[ ]?(?<Numero>[\d-]{8,10})$");

            if (!match.Success) throw new ArgumentException(MessageDefaultExceptionRepository.Argument_IncorretFormatPhone);

            var ddd = match.Groups["DDD"].Value;
            var numero = match.Groups["Numero"].Value.Replace("-", "");

            if (!Enum.IsDefined(typeof(CodigosDeAreaDoBrasil), Convert.ToInt32(ddd))) throw new ArgumentException(string.Format(MessageDefaultExceptionRepository.Argument_InvalidDDD, ddd));
            if (numero.Length == 9 && !numero.StartsWith("9")) throw new ArgumentException(MessageDefaultExceptionRepository.Argument_IncorretFormatPhone);

            return $"{ddd}{numero}";
        }

        private void ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException(string.Format(MessageDefaultExceptionRepository.Argument_IsNotNullParameterContato, nameof(email)));
            if (!Regex.IsMatch(email, emailPattern)) throw new ArgumentException(MessageDefaultExceptionRepository.Argument_IncorretFormatEmail);
        }
    }
}

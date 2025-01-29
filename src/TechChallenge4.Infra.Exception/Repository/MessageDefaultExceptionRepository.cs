using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallenge4.Infra.Exception.Repository
{
    public class MessageDefaultExceptionRepository
    {
        
        public const string Argument_EmptyOrWhiteSpaceString = "The value cannot be an empty string. (Parameter '{0}')";        
        public const string Argument_IncorretFormatPhone = "O telefone não é válido. O formato esperado é '99 99999-9999', podendo o telefone ter de 8 à 9 dígitos.";
        public const string Argument_IncorretFormatEmail = "O e-mail não é valido. O formato esperado é 'prefixo@provedor.n_complementos'.";
        public const string Argument_InvalidDDD = "O DDD {0} é inválido";        
        public const string Argument_IsNotNullParameterContato = "É necessário um {0} para criar um contato.";
    }
}

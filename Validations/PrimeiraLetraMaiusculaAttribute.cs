using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations;

//Criando atributos personalizados
public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var stringValue = value.ToString();

        if (!string.IsNullOrEmpty(stringValue))
        {

            var primeiraLetra = stringValue.ToString()[0].ToString();

            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                return new ValidationResult("A primeira letra do nome deve ser maiúscula.");
            }
        }

        return ValidationResult.Success;
    }
}
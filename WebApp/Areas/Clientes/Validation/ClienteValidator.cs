using ApplicationCore.Entities;
using FluentValidation;

namespace WebApp.Areas.Clientes.Validation
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Nombre).NotNull().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Nombre).Length(3, 50).WithMessage("Nombre debe tener entre 3 y 50 caracteres.");
        }
    }
}

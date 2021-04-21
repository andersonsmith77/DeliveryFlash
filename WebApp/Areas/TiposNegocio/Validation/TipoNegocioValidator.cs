using ApplicationCore.Entities;
using FluentValidation;

namespace WebApp.Areas.TiposNegocio.Validation
{
    public class TipoNegocioValidator : AbstractValidator<TipoNegocio>
    {
        public TipoNegocioValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Nombre).NotNull().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Nombre).Length(3, 50).WithMessage("Nombre debe tener entre 3 y 50 caracteres.");
        }
    }
}

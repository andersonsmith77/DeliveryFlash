using ApplicationCore.Entities;
using FluentValidation;
using Infraestructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Productos.Validation
{
    public class NegocioValidator : AbstractValidator<Producto>
    {
        private readonly MyRepository<Negocio> _negocioRepository;
        public NegocioValidator(MyRepository<Negocio> negocioRepository)
        {
            _negocioRepository = negocioRepository;

            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.NegocioId).NotNull().WithMessage("Negocio es requerido.");
            RuleFor(x => x.NegocioId).MustAsync(async (id, cancellation) =>
            {
                return await _negocioRepository.GetByIdAsync(id) == null ? false : true;
            }).WithMessage("Debe ingresar un negocio válido");

            RuleFor(x => x.Nombre).NotNull().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Estado).NotNull().WithMessage("Estado es requerido.");
            RuleFor(x => x.Precio).NotNull().WithMessage("Precio es requerida.");
            RuleFor(x => x.Nombre).Length(3, 50).WithMessage("Nombre debe tener entre 3 y 50 caracteres.");
        }
    }
}
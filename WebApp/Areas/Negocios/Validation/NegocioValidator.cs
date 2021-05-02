using ApplicationCore.Entities;
using FluentValidation;
using Infraestructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Negocios.Validation
{
    public class NegocioValidator : AbstractValidator<Negocio>
    {
        private readonly MyRepository<Categoria> _categoriaRepository;
        public NegocioValidator(MyRepository<Categoria> categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;

            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.CategoriaId).NotNull().WithMessage("Categoria es requerida.");
            RuleFor(x => x.CategoriaId).MustAsync(async (id, cancellation) =>
            {
                return await _categoriaRepository.GetByIdAsync(id) == null ? false : true;
            }).WithMessage("Debe ingresar una categoria válida");

            RuleFor(x => x.Nombre).NotNull().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Activo).NotNull().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Direccion).NotNull().WithMessage("Dirección es requerida.");
            RuleFor(x => x.Telefono).NotNull().WithMessage("Teléfono es requerido.");
            RuleFor(x => x.Email).NotNull().WithMessage("Email es requerido.");
            RuleFor(x => x.Nombre).Length(3, 50).WithMessage("Nombre debe tener entre 3 y 50 caracteres.");
        }
    }
}

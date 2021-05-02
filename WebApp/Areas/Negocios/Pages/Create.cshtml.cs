using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specification;
using AspNetCoreHero.ToastNotification.Abstractions;
using Infraestructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace WebApp.Areas.Negocios.Pages
{
    public class CreateModel : PageModel
    {
        private readonly MyRepository<Negocio> _repository;
        private readonly MyRepository<Categoria> _categoriaRepository;
        private readonly IAppLogger<CreateModel> _logger;
        public INotyfService _notifyService { get; }

        public CreateModel(MyRepository<Negocio> repository, MyRepository<Categoria> categoriaRepository, IAppLogger<CreateModel> logger, INotyfService notifyService)
        {
            _repository = repository;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _notifyService = notifyService;
        }

        [BindProperty]
        public Negocio Negocio { get; set; } 
        public Categoria Categoria { get; set; }

        public async Task<IActionResult> OnGet(int categoriaId)
        {
            try
            {
                Categoria = await _categoriaRepository.GetByIdAsync(categoriaId);
                if (Categoria == null)
                {
                    _notifyService.Warning($"No se ha encontrado la categoria con el id {categoriaId}");
                    return RedirectToPage("Index", new { area = "Categorias" });
                }

                Negocio = new Negocio
                {
                    CategoriaId = categoriaId
                };

                return Page();
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex.Message);
                return RedirectToPage("Index", new { area = "Categorias" });
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var existeNegocio = await _repository.GetBySpecAsync(new NegocioPorNombreSpec(Negocio.Nombre, Negocio.CategoriaId));
                if (existeNegocio != null)
                {
                    _notifyService.Warning($"Ya existe un negocio con el nombre {Negocio.Nombre}.");
                    return RedirectToPage("Create", new { categoriaId = Negocio.CategoriaId });
                }

                if (ModelState.IsValid)
                {
                    await _repository.AddAsync(Negocio);
                    _notifyService.Success("Negocio agregada exitosamente");
                }
                else
                {
                    _notifyService.Warning("El formulario no cumple las reglas de negocio");
                    return RedirectToPage("Create", new { categoriaId = Negocio.CategoriaId });
                }

                return RedirectToPage("Index", new { categoriaId = Negocio.CategoriaId });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                _notifyService.Error("Ocurrio un error en el servidor, intente nuevamente");
                return RedirectToPage("Index");
            }
        }
    }
}
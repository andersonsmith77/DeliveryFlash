using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specification;
using AspNetCoreHero.ToastNotification.Abstractions;
using Infraestructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace WebApp.Areas.Productos.Pages
{
    public class CreateModel : PageModel
    {
        private readonly MyRepository<Producto> _repository;
        private readonly MyRepository<Negocio> _negocioRepository;
        private readonly IAppLogger<CreateModel> _logger;
        public INotyfService _notifyService { get; }

        public CreateModel(MyRepository<Producto> repository, MyRepository<Negocio> negocioRepository, IAppLogger<CreateModel> logger, INotyfService notifyService)
        {
            _repository = repository;
            _negocioRepository = negocioRepository;
            _logger = logger;
            _notifyService = notifyService;
        }

        [BindProperty]
        public Producto Producto { get; set; }
        public Negocio Negocio { get; set; }

        public async Task<IActionResult> OnGet(int negocioId)
        {
            try
            {
                Negocio = await _negocioRepository.GetByIdAsync(negocioId);
                if (Negocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado el negocio con el id {negocioId}");
                    return RedirectToPage("Index", new { area = "Negocios" });
                }

                Producto = new Producto
                {
                    NegocioId = negocioId
                };

                return Page();
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex.Message);
                return RedirectToPage("Index", new { area = "Negocios" });
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var existeNegocio = await _repository.GetBySpecAsync(new ProductoPorNombreSpec(Producto.Nombre, Producto.NegocioId));
                if (existeNegocio != null)
                {
                    _notifyService.Warning($"Ya existe un negocio con el nombre {Producto.Nombre}.");
                    return RedirectToPage("Create", new { negocioId = Producto.NegocioId });
                }

                if (ModelState.IsValid)
                {
                    await _repository.AddAsync(Producto);
                    _notifyService.Success("Producto agregada exitosamente");
                }
                else
                {
                    _notifyService.Warning("El formulario no cumple las reglas de negocio");
                    return RedirectToPage("Create", new { negocioId = Producto.NegocioId });
                }

                return RedirectToPage("Index", new { negocioId = Producto.NegocioId });
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

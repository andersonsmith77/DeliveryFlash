using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using Infraestructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace WebApp.Areas.Productos.Pages
{
    public class EditModel : PageModel
    {
        private readonly MyRepository<Producto> _repository;
        private readonly MyRepository<Negocio> _negocioRepository;

        private readonly IAppLogger<EditModel> _logger;
        public INotyfService _notifyService { get; }

        public EditModel(MyRepository<Producto> repository, INotyfService notifyService, MyRepository<Negocio> negocioRepository, IAppLogger<EditModel> logger)
        {
            _repository = repository;
            _notifyService = notifyService;
            _negocioRepository = negocioRepository;
            _logger = logger;
        }

        [BindProperty]
        public Producto Producto { get; set; }
        public Negocio Negocio { get; set; }

        public async Task<IActionResult> OnGet(int id, int negocioId)
        {
            try
            {
                Negocio = await _negocioRepository.GetByIdAsync(negocioId);
                if (Negocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado la zona con el id {negocioId}");
                    return RedirectToPage("Index", new { area = "Negocios" });

                }

                var producto = await _repository.GetByIdAsync(id);
                if (producto == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {id}");
                    return RedirectToPage("Index", new { negocioId = negocioId });
                }
                Producto = producto;

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return RedirectToPage("Index", new { area = "Negocios" });
            }

        }

        public async Task<IActionResult> OnPost(int id, int zonaId)
        {
            try
            {
                var producto = await _repository.GetByIdAsync(id);
                if (producto == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {id}");
                    return RedirectToPage("Index", new { zonaId = zonaId });
                }

                if (ModelState.IsValid)
                {
                    producto.Nombre = producto.Nombre;
                    producto.NegocioId = producto.NegocioId;
                    producto.Precio = producto.Precio;
                    producto.Estado = producto.Estado;

                    await _repository.UpdateAsync(producto);
                    _notifyService.Information("producto editado exitosamente");
                }
                else
                {
                    _notifyService.Warning("El formulario no cumple las reglas de negocio");
                    return Page();
                }

                return RedirectToPage("Index", new { zonaId = producto.NegocioId });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                _notifyService.Error("Ocurrio un error en el servidor, intente nuevamente");
                return RedirectToPage("Index", new { zonaId = zonaId });
            }
        }
    }
}

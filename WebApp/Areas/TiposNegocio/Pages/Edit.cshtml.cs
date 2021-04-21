using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using Infraestructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.TiposNegocio.Pages
{
    public class EditModel : PageModel
    {
        private readonly MyRepository<TipoNegocio> _repository;
        private readonly IAppLogger<EditModel> _logger;
        public INotyfService _notifyService { get; }

        public EditModel(MyRepository<TipoNegocio> repository, IAppLogger<EditModel> logger, INotyfService notifyService)
        {
            _repository = repository;
            _logger = logger;
            _notifyService = notifyService;
        }

        [BindProperty]
        public TipoNegocio TipoNegocio { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                var tipoNegocio = await _repository.GetByIdAsync(id);
                if (tipoNegocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {id}");
                    return RedirectToPage("Index");
                }
                TipoNegocio = tipoNegocio;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }

        }

        public async Task<IActionResult> OnPost(int id)
        {
            try
            {
                var tipoNegocio = await _repository.GetByIdAsync(id);
                if (tipoNegocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {id}");
                    return RedirectToPage("Index");
                }

                if (ModelState.IsValid)
                {
                    tipoNegocio.Nombre = TipoNegocio.Nombre;
                    tipoNegocio.Activo = TipoNegocio.Activo;

                    await _repository.UpdateAsync(tipoNegocio);
                    _notifyService.Information("Tipo de Negocio editado exitosamente");
                }
                else
                {
                    _notifyService.Warning("El formulario no cumple las reglas de negocio");
                    return Page();
                }

                return RedirectToPage("Index");
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

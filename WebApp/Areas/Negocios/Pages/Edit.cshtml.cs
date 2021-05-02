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

namespace WebApp.Areas.Negocios.Pages
{
    public class EditModel : PageModel
    {
        private readonly MyRepository<Negocio> _repository;
        private readonly MyRepository<Categoria> _categoriaRepository;
        
        private readonly IAppLogger<EditModel> _logger;
        public INotyfService _notifyService { get; }

        public EditModel(MyRepository<Negocio> repository, MyRepository<Categoria> categoriaRepository, IAppLogger<EditModel> logger, INotyfService notifyService)
        {
            _repository = repository;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _notifyService = notifyService;
        }

        [BindProperty]
        public Negocio Negocio { get; set; }

        public Categoria Categoria { get; set; }

        public async Task<IActionResult> OnGet(int Id, int categoriaId) 
        {
            try
            {
                Categoria = await _categoriaRepository.GetByIdAsync(categoriaId);
                if (Categoria == null) 
                {
                    _notifyService.Warning($"No se ha encontrado la categoria con el id {categoriaId}");
                    return RedirectToPage("Index", new { area = "Categorias" });
                }
                var negocio = await _repository.GetByIdAsync(Id);
                if (negocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {Id}");
                    return RedirectToPage("Index", new { categoriaId = categoriaId });
                }
                Negocio = negocio;

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return RedirectToPage("Index", new { area = "Categorias" });
            }        
        }

        public async Task<IActionResult> OnPost(int id, int categoriaId)
        {
            try
            {
                var negocio = await _repository.GetByIdAsync(id);
                if (negocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {id}");
                    return RedirectToPage("Index", new { categoriaId = categoriaId });
                }

                if (ModelState.IsValid)
                {
                    negocio.Nombre = Negocio.Nombre;
                    negocio.Activo = Negocio.Activo;
                    negocio.CategoriaId = Negocio.CategoriaId;

                    await _repository.UpdateAsync(negocio);
                    _notifyService.Information("Negocio editado exitosamente");
                }
                else
                {
                    _notifyService.Warning("El formulario no cumple las reglas de esta categoria");
                    return Page();
                }

                return RedirectToPage("Index", new { categoriaId = Negocio.CategoriaId});
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                _notifyService.Error("Ocurrio un error en el servidor, intente nuevamente");
                return RedirectToPage("Index", new { categoriaId = categoriaId});
            }
        }
    }
}

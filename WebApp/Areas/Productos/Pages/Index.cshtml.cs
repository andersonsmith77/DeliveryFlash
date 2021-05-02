using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specification;
using ApplicationCore.Specification.Filters;
using AspNetCoreHero.ToastNotification.Abstractions;
using Infraestructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Productos.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyRepository<Producto> _repository;
        private readonly MyRepository<Negocio> _negocioRepository;
        private readonly IAppLogger<IndexModel> _logger;
        public INotyfService _notifyService { get; }

        public IndexModel(MyRepository<Producto> repository, INotyfService notifyService, MyRepository<Negocio> negocioRepository, IAppLogger<IndexModel> logger)
        {
            _repository = repository;
            _notifyService = notifyService;
            _negocioRepository = negocioRepository;
            _logger = logger;
        }

        public Negocio Negocio { get; set; }
        public List<Producto> Productos { get; set; }
        public UIPaginationModel UIPagination { get; set; }

        public async Task<IActionResult> OnGetAsync(int? negocioId, string searchString, int? currentPage, int? sizePage)
        {
            try
            {
                if (!negocioId.HasValue)
                {
                    _notifyService.Warning($"Debe seleccionar una zona para ver sus productos");
                    return RedirectToPage("Index", new { area = "Negocios" });
                }

                Negocio = await _negocioRepository.GetByIdAsync(negocioId.Value);
                if (Negocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado la zona con el id {negocioId.Value}");
                    return RedirectToPage("Index", new { area = "Negocios" });
                }

                var totalItems = await _repository.CountAsync(new ProductoSpec(
                    new ProductoFilter 
                    { 
                        NegocioId = negocioId.Value, 
                        Nombre = searchString, LoadChildren = false, 
                        IsPagingEnabled = true 
                    }));

                UIPagination = new UIPaginationModel(currentPage, searchString, sizePage, totalItems);

                Productos = await _repository.ListAsync(new ProductoSpec(
                    new ProductoFilter
                    {
                        IsPagingEnabled = true,
                        Nombre = UIPagination.SearchString,
                        NegocioId = negocioId.Value,
                        PageSize = UIPagination.GetPageSize,
                        Page = UIPagination.GetCurrentPage
                    })
                 );

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return RedirectToPage("Index", new { area = "Negocios" });
            }

        }

        public async Task<JsonResult> OnPostDelete(int id)
        {
            try
            {
                var producto = await _repository.GetByIdAsync(id);
                if (producto == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {id}");
                    return new JsonResult(new { deleted = false });
                }

                await _repository.DeleteAsync(producto);
                return new JsonResult(new { deleted = true });

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                _notifyService.Error("Ocurrio un error en el servidor, intente nuevamente");
                return new JsonResult(new { deleted = false });
            }
        }
    }
}
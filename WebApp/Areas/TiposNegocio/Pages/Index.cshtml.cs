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
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.TiposNegocio.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyRepository<TipoNegocio> _repository;
        public INotyfService _notifyService { get; }
        private readonly IAppLogger<IndexModel> _logger;

        public IndexModel(MyRepository<TipoNegocio> repository, INotyfService notifyService, IAppLogger<IndexModel> logger)
        {
            _repository = repository;
            _notifyService = notifyService;
            _logger = logger;
        }

        public List<TipoNegocio> TiposNegocio { get; set; }
        public UIPaginationModel UIPagination { get; set; }
        public async Task OnGet(string searchString, int? currentPage, int? sizePage)
        {
            try
            {
                var totalItems = await _repository.CountAsync(new TipoNegocioSpec(
                    new TipoNegocioFilter
                    {
                        Nombre = searchString,
                        LoadChildren = false,
                        IsPagingEnabled = true
                    }));

                UIPagination = new UIPaginationModel(currentPage, searchString, sizePage, totalItems);
                TiposNegocio = await _repository.ListAsync(new TipoNegocioSpec(
                    new TipoNegocioFilter
                    {
                        IsPagingEnabled = true,
                        Nombre = UIPagination.SearchString,
                        PageSize = UIPagination.GetPageSize,
                        Page = UIPagination.GetCurrentPage,
                        LoadChildren = false
                    }
                ));

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }
    
        public async Task<JsonResult> OnPostDelete(int id)
        {
            try
            {
                var tipoNegocio = await _repository.GetByIdAsync(id);
                if(tipoNegocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {id}");
                    return new JsonResult(new { deleted = false });
                }

                await _repository.DeleteAsync(tipoNegocio);
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

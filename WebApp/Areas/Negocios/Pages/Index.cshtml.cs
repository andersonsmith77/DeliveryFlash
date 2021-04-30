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

namespace WebApp.Areas.Negocios.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyRepository<Negocio> _repository;
        public INotyfService _notifyService { get; }
        private readonly IAppLogger<IndexModel> _logger;

        public IndexModel(MyRepository<Negocio> repository, INotyfService notifyService, IAppLogger<IndexModel> logger)
        {
            _repository = repository;
            _notifyService = notifyService;
            _logger = logger;
        }

        public List<Negocio> Negocios { get; set; }
        public UIPaginationModel UIPagination { get; set; }
        public async Task OnGet(string searchString, int? currentPage, int? sizePage)
        {
            try
            {
                var totalItems = await _repository.CountAsync(new NegocioSpec(
                    new NegocioFilter
                    {
                        Nombre = searchString,
                        LoadChildren = false,
                        IsPagingEnabled = true
                    }));

                UIPagination = new UIPaginationModel(currentPage, searchString, sizePage, totalItems);
                Negocios = await _repository.ListAsync(new NegocioSpec(
                    new NegocioFilter
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
                var Negocio = await _repository.GetByIdAsync(id);
                if (Negocio == null)
                {
                    _notifyService.Warning($"No se ha encontrado el registro con el id {id}");
                    return new JsonResult(new { deleted = false });
                }

                await _repository.DeleteAsync(Negocio);
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


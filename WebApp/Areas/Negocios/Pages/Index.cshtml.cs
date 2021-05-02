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
        private readonly MyRepository<Categoria> _categoriaRepository;
        public INotyfService _notifyService { get; }
        private readonly IAppLogger<IndexModel> _logger;

        public IndexModel(MyRepository<Negocio> repository, INotyfService notifyService, MyRepository<Categoria> categoriaRepository, IAppLogger<IndexModel> logger)
        {
            _repository = repository;
            _notifyService = notifyService;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
        }

        public List<Negocio> Negocios { get; set; }
        public Categoria Categoria { get; set; }
        public UIPaginationModel UIPagination { get; set; }

        public async Task<IActionResult> OnGetAsync(int? categoriaId, string searchString, int? currentPage, int? sizePage) 
        {
            try
            {
                if (!categoriaId.HasValue)
                {
                    _notifyService.Warning($"Debe selecionar una categoria para ver sus negocios");
                    return RedirectToPage("Index", new { area = "Categorias" });
                }

                Categoria = await _categoriaRepository.GetByIdAsync(categoriaId.Value);
                if (Categoria == null) 
                {
                    _notifyService.Warning($"No se ha encontrado una categoria con el id {categoriaId.Value}");
                    return RedirectToPage("Index", new { area = "Categorias" });
                }

                var totalItems = await _repository.CountAsync(new NegocioSpec(
                    new NegocioFilter
                    {
                        CategoriaId = categoriaId.Value,
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
                        CategoriaId = categoriaId.Value,
                        PageSize = UIPagination.GetPageSize,
                        Page = UIPagination.GetCurrentPage,
                        LoadChildren = true
                    })
                );
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return RedirectToPage("Index", new { area = "Categorias"});
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


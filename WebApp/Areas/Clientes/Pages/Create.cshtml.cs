using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using Infraestructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace WebApp.Areas.Clientes.Pages
{
    public class CreateModel : PageModel
    {
        private readonly MyRepository<Cliente> _repository;
        private readonly IAppLogger<CreateModel> _logger;
        public INotyfService _notifyService { get; }

        public CreateModel(MyRepository<Cliente> repository, IAppLogger<CreateModel> logger, INotyfService notifyService)
        {
            _repository = repository;
            _logger = logger;
            _notifyService = notifyService;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.AddAsync(Cliente);
                    _notifyService.Success("Cliente agregado exitosamente");
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


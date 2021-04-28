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

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyRepository<Categoria> _repository;
        public INotyfService _notifyService { get; }
        private readonly IAppLogger<IndexModel> _logger;

        public IndexModel(MyRepository<Categoria> repository, INotyfService notifyService, IAppLogger<IndexModel> logger)
        {
            _repository = repository;
            _notifyService = notifyService;
            _logger = logger;
        }

        public List<Categoria> Categorias { get; set; }

        public async Task OnGet()
        {
            Categorias = await _repository.ListAsync();
        }
    }
}

﻿using ApplicationCore.Entities;
using ApplicationCore.Specification.Filters;
using ApplicationCore.Specification.Helpers;
using Ardalis.Specification;

namespace ApplicationCore.Specification
{
    public class CategoriaSpec : Specification<Categoria>
    {
        public CategoriaSpec(CategoriaFilter filter)
        {
            Query.OrderBy(x => x.Nombre).ThenByDescending(x => x.Id);

            if (filter.IsPagingEnabled)
                Query.Skip(PaginationHelper.CalculateSkip(filter))
                     .Take(PaginationHelper.CalculateTake(filter));

            if (filter.LoadChildren)
                Query.Include(x => x.Negocios);

            if (!string.IsNullOrEmpty(filter.Nombre))
                Query.Search(x => x.Nombre.ToLower(), "%" + filter.Nombre.ToLower() + "%");
        }
    }
}
using ApplicationCore.Entities;
using ApplicationCore.Specification.Filters;
using ApplicationCore.Specification.Helpers;
using Ardalis.Specification;

namespace ApplicationCore.Specification
{
    public class TipoNegocioSpec : Specification<TipoNegocio>
    {
        public TipoNegocioSpec(TipoNegocioFilter filter)
        {
            Query.OrderBy(x => x.Nombre).ThenByDescending(x => x.Id);

            if (filter.IsPagingEnabled)
                Query.Skip(PaginationHelper.CalculateSkip(filter))
                     .Take(PaginationHelper.CalculateTake(filter));

            if (!string.IsNullOrEmpty(filter.Nombre))
                Query.Search(x => x.Nombre.ToLower(), "%" + filter.Nombre.ToLower() + "%");
        }
    }
}

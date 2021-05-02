using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specification
{
    public class ProductoPorNombreSpec : Specification<Producto>, ISingleResultSpecification
    {
        public ProductoPorNombreSpec(string nombre, int productoId)
        {
            Query.Where(x => x.NegocioId == productoId);
            Query.Where(x => x.Nombre.ToLower() == nombre.Trim().ToLower());
        }
    }
}
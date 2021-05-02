using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specification
{
    public class NegocioPorNombreSpec : Specification<Negocio>, ISingleResultSpecification
    {
        public NegocioPorNombreSpec(string nombre, int negocioId)
        {
            Query.Where(x => x.CategoriaId == negocioId);
            Query.Where(x => x.Nombre.ToLower() == nombre.Trim().ToLower());
        }
    }
}
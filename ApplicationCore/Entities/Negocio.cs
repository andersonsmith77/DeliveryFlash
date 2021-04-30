using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Negocio
    {
        //Siman, Buffalo Wings, EPA, Freund, Vidri, Jose N Batarse
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public List<Producto> Productos { get; set; }
    }
}

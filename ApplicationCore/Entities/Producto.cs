using ApplicationCore.Enum;

namespace ApplicationCore.Entities
{
    public class Producto
    {
        //Hamburgesa, Recarga, Camisa
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public string Imagen { get; set; }
        public int CategoriaId { get; set; }
        //public Categoria Categoria { get; set; }
        public bool EsServicio { get; set; }
        public Estado Estado { get; set; }
    }
}

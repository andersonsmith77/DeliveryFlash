using ApplicationCore.Enum;

namespace ApplicationCore.Entities
{
    public class Producto
    {
        //Hamburgesa, Recarga, Camisa
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public Estado Estado { get; set; }
        public int NegocioId { get; set; }
        public Negocio Negocio { get; set; }
        //public string Imagen { get; set; }
    }
}

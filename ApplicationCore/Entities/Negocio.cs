namespace ApplicationCore.Entities
{
    public class Negocio
    {
        //Siman, Buffalo Wings, EPA, Freund, Vidri, Jose N Batarse
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public int TipoNegocioId { get; set; }
        public TipoNegocio Tipo { get; set; }
    }
}

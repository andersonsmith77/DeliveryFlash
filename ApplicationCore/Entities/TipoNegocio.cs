namespace ApplicationCore.Entities
{
    public class TipoNegocio
    {
        // Restaurantes, Farmacias, Ferreterias, Boutiques, Electronicos.
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}

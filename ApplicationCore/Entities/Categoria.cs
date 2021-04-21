namespace ApplicationCore.Entities
{
    public class Categoria
    {
        //Comida, Medicinas, Ropa, Cervezas, Pago de Recibos
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}

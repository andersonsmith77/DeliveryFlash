namespace ApplicationCore.Entities
{
    public class DetalleOrden
    {
        /// <summary>
        /// 1, 2, 3, Hamburgesa con Queso, 3.25, 3.25
        /// </summary>
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }
        public int Cantidad { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public double Total { get; set; }
    }
}

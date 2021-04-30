using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Categoria
    {
        //Comida, Medicinas, Ropa, Cervezas, Pago de Recibos
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public List<Negocio> Negocios { get; set; }
    }
}

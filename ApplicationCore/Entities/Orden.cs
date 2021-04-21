using ApplicationCore.Enum;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Orden
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int NegocioId { get; set; }
        public Negocio Negocio { get; set; }
        public EstadoOrden Estado { get; set; }
        public List<DetalleOrden> Detalle { get; set; }
    }
}

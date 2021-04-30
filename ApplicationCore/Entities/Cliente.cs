using ApplicationCore.Enum;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Cliente
    {
        //Juan Perez, Maria Luisa
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public Estado Estado { get; set; }
        public List<Orden> Ordenes { get; set; }
    }   
}

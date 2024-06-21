using System;
namespace UI.Models
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public Guid IdCiudad { get; set; }
    }
}

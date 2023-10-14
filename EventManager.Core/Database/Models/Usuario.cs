// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Core.Database.Models
{
    [Table("usuario")]
    public class Usuario
    {
        public enum TipoUsuario
        {
            JEFE,
            ENCARGADA,
            OTROS
        }

        [Key][Column("id")] public int Id { get; set; }
        [Required][Column("nombre")] public string Nombre { get; set; }
        [Required][Column("password")] public string Password { get; set; }
        [Required][Column("tipo")] public TipoUsuario Tipo { get; set; }
        public List<Evento> Eventos { get; set; } = new List<Evento>();
    }
}
// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Core.Database.Models
{
    [Table("agregable")]
    public class Agregable
    {
        public enum TipoAgregable
        {
            MATERIAL,
            EQUIPO,
            MOBILIARIO
        }

        [Key][Column("id")] public int Id { get; set; }
        [Required][Column("nombre")] public string Nombre { get; set; }
        [Required][Column("tipo")] public TipoAgregable Tipo { get; set; }
        [Required][Column("cantidad")] public int Cantidad { get; set; }
        public List<EventoAgregable> EventoAgregables { get; set; } = new List<EventoAgregable>();
    }
}
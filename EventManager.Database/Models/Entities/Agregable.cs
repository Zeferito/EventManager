// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventManager.Database.Models.Enums;

namespace EventManager.Database.Models.Entities
{
    [Table("agregable")]
    public partial class Agregable
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tipo")]
        public TipoAgregable Tipo { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [Column("total")]
        public int Total { get; set; }

        public List<EventoAgregable> EventoAgregables { get; private set; } = new();
    }
}
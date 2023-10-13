// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Database.Models
{
    [Table("agregable")]
    public class Agregable
    {
        [Key][Column("id")] public int Id { get; set; }
        [Required][Column("nombre")] public string Nombre { get; set; }
        [Required][Column("tipo_agregable")] public string TipoAgregable { get; set; }
        [Required][Column("cantidad")] public int Cantidad { get; set; }
        public List<EventoAgregable> EventoAgregables { get; set; }
    }
}
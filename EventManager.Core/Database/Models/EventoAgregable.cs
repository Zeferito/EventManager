// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Core.Database.Models
{
    [Table("evento_agregable")]
    public class EventoAgregable
    {
        [Required][Column("agregable_id")] public int AgregableId { get; set; }
        public Agregable Agregable { get; set; }
        [Required][Column("evento_id")] public int EventoId { get; set; }
        public Evento Evento { get; set; }
    }
}
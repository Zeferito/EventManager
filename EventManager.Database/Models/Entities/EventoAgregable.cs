// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventManager.Database.Models.Entities
{
    [Table("evento_agregable")]
    public partial class EventoAgregable
    {
        [Required]
        [Column("cantidad_reservada")]
        public int CantidadReservada { get; set; }

        [Required]
        [Column("agregable_id")]
        public int AgregableId { get; set; }

        [JsonIgnore]
        public Agregable? Agregable { get; set; }

        [Required]
        [Column("evento_id")]
        public int EventoId { get; set; }

        [JsonIgnore]
        public Evento? Evento { get; set; }
    }
}

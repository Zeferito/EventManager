// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lombok.NET;

namespace EventManager.Core.Models
{
    [Table("sala")]
    public partial class Sala
    {
        public enum TipoSala
        {
            Salon,
            Auditorio
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tipo")]
        public TipoSala Tipo { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("evento_id")]
        public int? EventoId { get; set; }

        public Evento? Evento { get; set; }
    }
}
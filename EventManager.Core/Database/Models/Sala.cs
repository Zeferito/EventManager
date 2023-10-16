// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Core.Database.Models
{
    [Table("sala")]
    public class Sala
    {
        public enum TipoSala
        {
            SALON,
            AUDITORIO,
        }
        
        [Key][Column("id")] public int Id { get; set; }
        [Required][Column("nombre")] public string Nombre { get; set; }
        [Required][Column("tipo")] public TipoSala Tipo { get; set; }
        [Column("evento_id")] public int? EventoId { get; set; }
        public Evento? Evento { get; set; }
    }
}
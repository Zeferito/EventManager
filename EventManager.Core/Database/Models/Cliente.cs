// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EventManager.Core.Database.Models
{
    [Table("cliente")]
    public class Cliente
    {
        [Key][Column("id")] public int Id { get; set; }
        [Column("evento_id")] public int? EventoId { get; set; }
        public Evento? Evento { get; set; }
        [Required][Column("nombre")] public string Nombre { get; set; }
        [Required][Column("telefono")] public string Telefono { get; set; }

        public override string ToString()
        {
            return $"Cliente: {Nombre}";
        }
    }
}
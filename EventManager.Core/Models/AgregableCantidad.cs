// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Core.Models;
using Lombok.NET;

namespace EventManager.Core.Models
{
    public partial class AgregableCantidad
    {
        public Agregable Agregable { get; set; }

        public int Cantidad { get; set; }
    }
}
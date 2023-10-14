// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.Services
{
    public class UsuarioService
    {
        private readonly DatabaseContext _context;

        public UsuarioService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario Usuario)
        {
            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();
            return Usuario;
        }

        public async Task<Usuario> UpdateUsuarioAsync(int id, Usuario Usuario)
        {
            _context.Entry(Usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Usuario;
        }

        public async Task<Usuario> DeleteUsuarioAsync(int id)
        {
            var Usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(Usuario);
            await _context.SaveChangesAsync();
            return Usuario;
        }

        public async Task<bool> UsuarioExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.Id == id);
        }
    }
}
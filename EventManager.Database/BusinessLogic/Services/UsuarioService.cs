// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;

namespace EventManager.Database.BusinessLogic.Services
{
    public partial class UsuarioService
    {
        private readonly DatabaseContext _context;

        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(DatabaseContext context)
        {
            _context = context;
            _usuarioRepository = new UsuarioRepository(context);
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario?> CreateAsync(Usuario usuario)
        {
            return await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task<Usuario?> UpdateAsync(Usuario usuario)
        {
            return await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _usuarioRepository.DeleteAsync(id);
        }

        public async Task<List<Usuario>> GetUsuariosWithEventosAsync()
        {
            return await _usuarioRepository.GetUsuariosWithEventosAsync();
        }
    }
}
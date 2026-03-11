using BakuganApi.Data;
using BakuganApi.models;
using BakuganApi.models.DTO.usuariosDTO;
using BakuganApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace BakuganApi.services
{

    public interface IUsuarioService
    {
        Task<UserInfoDTO> RegistrarUsuarioServicio(registerDTO registerData);
        Task<string> LoginUsuarioServicio(loginDto loginDto);
    }
    public class UsuariosService : IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly Utilidades _utilidades;
        
        public UsuariosService(AppDbContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }
        public async Task<UserInfoDTO> RegistrarUsuarioServicio(registerDTO registerData)
        {
                var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == registerData.Email);
                if (emailExiste)
                {
                    throw new InvalidOperationException("No se puede registrar el usuario: el email ya está en uso."); ;
                }

                if (string.IsNullOrEmpty(registerData.Nombre))
                {
                    throw new ArgumentException("El nombre no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(registerData.Email))
                {
                    throw new ArgumentException("El email no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(registerData.PasswordHash) || registerData.PasswordHash.Length < 6)
                {
                    throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");
                }
                var nuevoUsuario = new Usuario
                {
                    Nombre = registerData.Nombre,
                    Email = registerData.Email,
                    PasswordHash = _utilidades.EncryptContrasena(registerData.PasswordHash),
                    TipoUsuario = "User"
                };

                await _context.Usuarios.AddAsync(nuevoUsuario);
                await _context.SaveChangesAsync();

                return new UserInfoDTO
                {
                    Nombre = registerData.Nombre,
                    Email = registerData.Email,
                    TipoUsuario = nuevoUsuario.TipoUsuario
                };
            }

        

        public async Task<string> LoginUsuarioServicio(loginDto loginDto)
        {


                if(string.IsNullOrEmpty(loginDto.Email))
                {
                    throw new ArgumentException("El email no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(loginDto.Password))
                {
                    throw new ArgumentException("La contraseña no puede estar vacía.");
                }

                var loginUsuario = await _context.Usuarios
                    .Where(u => u.Email == loginDto.Email && u.PasswordHash == _utilidades.EncryptContrasena(loginDto.Password)).FirstOrDefaultAsync();
                if(loginUsuario == null)
                {
                    throw new InvalidOperationException("Credenciales inválidas.");
                }
                string tokenUser = _utilidades.GenerarToken(loginUsuario);

                return tokenUser;
           
            
        }
    }
}

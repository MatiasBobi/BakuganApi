using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BakuganApi.Data;
using BakuganApi.models;


namespace BakuganApi.Utils
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;
        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EncryptContrasena(string contrasena)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computar hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contrasena));


                // Convertir el array de bytes a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) { 
                builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string GenerarToken(Usuario usuario)
        {
            // Crear info del usuario para el jwt

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario),
                new Claim(ClaimTypes.Name, usuario.Nombre),
            };

            // Crear Llave
            // 

            var jwtKey = _configuration["JWT:key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                return "ERROR: No se encontro la key de JWT.";
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

    }
}

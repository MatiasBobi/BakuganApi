using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BakuganApi.Data;
using BakuganApi.models;
using BakuganApi.models.DTO;
using BakuganApi.Utils;
using BakuganApi.models.DTO.usuariosDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using BakuganApi.services;


namespace BakuganApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthUserController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthUserController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("register")]

        public async Task<ActionResult<UserInfoDTO>> RegistrarUsuario(registerDTO registerData)
        {
            try
            {
                var nuevoUsuario = await _usuarioService.RegistrarUsuarioServicio(registerData);
                return Ok(nuevoUsuario);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest("Error de argumento: " + argEx.Message);
            }
            catch (InvalidOperationException invOpEx)
            {
                return BadRequest("Operación inválida: " + invOpEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest("Error de base de datos al registrar el usuario: " + dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el usuario: " + ex.Message);
            }
            
        }

        [HttpPost("login")]

        public async Task<ActionResult<string>> LoginUsuario(loginDto loginDto)
        {
            try
            {
                var usuarioInfo = await _usuarioService.LoginUsuarioServicio(loginDto);
                return Ok(usuarioInfo);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest("Error de argumento: " + argEx.Message);
            }
            catch (InvalidOperationException invOpEx)
            {
                return BadRequest("Operación inválida: " + invOpEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al iniciar sesión: " + ex.Message);
            }
        }
    }
}

namespace BakuganAPI.Controllers;

using BakuganApi.models.DTO.bakugansDTO;
using BakuganAPI.Models;
using BakuganAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("/[controller]")]
public class BakuganController : ControllerBase
{

    private readonly IBakuganService _bakuganService;

    // Inyección de dependencias del servicio de Bakugan
    public BakuganController(IBakuganService bakuganService) {

        _bakuganService = bakuganService;
    }

    [HttpGet("AllBakugans")]

    // ***** Obtener todos los Bakugans *****
    public async Task<ActionResult<List<BakuganDetailDTO>>> ObtenerBakugans()
    {
        try
        {
            var bakugan = await _bakuganService.TraerBakugansServicio();

            if (!bakugan.Any())
            {
                return NotFound("No se encontraron Bakugans.");
            }

            return Ok(bakugan);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error al obtener los bakugans (Operacion invalida): {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Error de base de datos al obtener los bakugans: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al obtener los bakugans por parte del servidor: {ex.Message}");

        }

    }
    // ***** Crear un nuevo Bakugan *****
    [Authorize(Roles = "Admin")]
    [HttpPost("add")]
    public async Task<ActionResult<List<BakuganModel>>> CrearBakugan([FromBody] BakuganModelCreateDTO bakugan)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos de Bakugan no válidos.");
            }

            await _bakuganService.CrearBakuganServicio(bakugan);

            return Ok("Bakugan Creado.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($" Error al crear: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error al crear el bakugan: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Error de base de datos al crear el bakugan: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al crear el bakugan por parte del servidor: {ex.Message}");
        }

    }

    // ***** Obtener Bakugan por ID *****
    [HttpGet("{bakuganId}")]
    public async Task<ActionResult<BakuganModel>> ObtenerBakuganPorId(int bakuganId)
    {
        try
        {
            var bakugan = await _bakuganService.TraerBakuganPorIdServicio(bakuganId);

            return Ok(bakugan);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error al obtener el bakugan: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Error de base de datos al obtener el bakugan: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al obtener el bakugan por parte del servidor: {ex.Message}");
        }
    }
    // ***** Actualizar el nombre y tipo de un Bakugan *****
    [Authorize(Roles = "Admin")]
    [HttpPatch("update_info/{bakuganId}")]

    public async Task<ActionResult<BakuganUpdateDTO>> ActualizarBakugan(int bakuganId, BakuganUpdateDTO bakuganUpdate)
    {
        try
        {
            var bakugan = await _bakuganService.ModificarBakuganServicio(bakuganId, bakuganUpdate);
            return Ok(new { mensaje = "Bakugan actualizado correctamente.", data = bakugan });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error al actualizar el bakugan: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Error de base de datos al actualizar el bakugan: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al actualizar el bakugan por parte del servidor: {ex.Message}");
        }

    }

    // ***** Eliminar un Bakugan *****
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{bakuganId}")]

    public async Task<ActionResult> ElminarBakugan(int bakuganId)
    {
        try
        {
            bool resultado = await _bakuganService.BorrarBakuganServicio(bakuganId);

            if (resultado)
            {
                return Ok("Bakugan eliminado correctamente.");
            }
            throw new Exception("ERROR AL BORRAR.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error al eliminar el bakugan: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Error de base de datos al eliminar el bakugan: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al eliminar el bakugan por parte del servidor: {ex.Message}");
        }
    }

    // ***** SKILLS *****

    // ***** Agregar una nueva habilidad a un Bakugan *****
    [Authorize(Roles = "Admin")]
    [HttpPost("skills/add/{bakuganId}")]
    public async Task<ActionResult<BakuganSkillDetailDTO>> AgregarSkill( int bakuganId, BakuganSkillAddDTO skillCreate)
    {
        try
        {
            var skill = await _bakuganService.AgregarSkillServicio(bakuganId, skillCreate);
            return Ok(new { mensaje = "Skill agregada correctamente.", data = skill });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error al agregar la habilidad: {ex.Message}");
        }

        catch (DbUpdateException ex)
        {
            return BadRequest($"Error de base de datos al agregar la habilidad: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al agregar la habilidad por parte del servidor: {ex.Message}");
        }
    }

    // ***** Actualizar una habilidad de un Bakugan *****
    [Authorize(Roles = "Admin")]
    [HttpPatch("skills/update/{bakuganId}/{skillId}")]
    public async Task<ActionResult<BakuganSkillDetailDTO>> ActualizarSkill(int bakuganId, int skillId, BakuganSkillUpdateDTO skillUpdateDTO)
    {
        try
        {
            var skill = await _bakuganService.ActualizarSkillServicio(bakuganId, skillId, skillUpdateDTO);
            return Ok(new {mensaje = "Habilidad actualizada correctamente." , data = skill});
        }

        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error al actualizar la habilidad: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Error de base de datos al actualizar la habilidad: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al actualizar la habilidad por parte del servidor: {ex.Message}");
        }
    }

    // ***** Obtener habilidades de un Bakugan *****

    [HttpGet("skills/{bakuganId}")]

    public async Task<ActionResult<BakuganSkillDetailDTO>> ObtenerSkillsBakugan(int bakuganId)
    {
        try
        {
            var bakuganSkills = await _bakuganService.TraerSkillsBakugan(bakuganId);
            if (bakuganSkills == null)
            {
                return NotFound("No se encontraron habilidades para este Bakugan.");
            }

            return Ok(bakuganSkills);
        }

        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);

        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error al obtener las habilidades: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Error de base de datos al obtener las habilidades: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al obtener las habilidades por parte del servidor: {ex.Message}");
        }
    }
}


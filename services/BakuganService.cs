using BakuganApi.Data;
using BakuganApi.Enums;
using BakuganApi.models.DTO.bakugansDTO;
using BakuganAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BakuganAPI.Services;

public interface IBakuganService
{
    Task<BakuganDetailDTO> CrearBakuganServicio(BakuganModelCreateDTO bakugan);
    Task<BakuganDetailDTO?> TraerBakuganPorIdServicio(int id);
    Task<List<BakuganDetailDTO>> TraerBakugansServicio();
    Task<BakuganDetailDTO> ModificarBakuganServicio(int id, BakuganUpdateDTO bakuganUpdate);
    Task<bool> BorrarBakuganServicio(int bakuganId);

    // SKILLS
    Task<BakuganSkillDetailDTO> AgregarSkillServicio(int id, BakuganSkillAddDTO skillDto);
    Task<BakuganSkillDetailDTO> ActualizarSkillServicio(int bakuganId, int skillid, BakuganSkillUpdateDTO skillUpdateDTO);
    Task<List<BakuganSkillDetailDTO>> TraerSkillsBakugan(int bakuganId);
}

public class BakuganService : IBakuganService
{
    private readonly AppDbContext _context;

    public BakuganService(AppDbContext context)
    {
        _context = context;
    }

    // ************* CREAR BAKUGAN **************
    public async Task<BakuganDetailDTO> CrearBakuganServicio(BakuganModelCreateDTO dto)
    {

            // Validaciones
            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new ArgumentException("El nombre del Bakugan es requerido");
            }
            if (!Enum.IsDefined(dto.Tipo))
            {
                throw new ArgumentException("El tipo de Bakugan no es válido");
            }

            if (dto.Precio < 0)
            {
                throw new ArgumentException("El precio no puede ser negativo");
            }

            // Verificar si ya existe
            var existe = await _context.Bakugans
                .AnyAsync(b => b.Nombre.ToLower() == dto.Nombre.ToLower());

            if (existe)
            {
                throw new InvalidOperationException($"Ya existe un Bakugan con el nombre '{dto.Nombre}'");
            }

            var bakugan = new BakuganModel
            {
                Nombre = dto.Nombre.Trim(),
                Tipo = dto.Tipo,
                Category = dto.Category,
                Precio = dto.Precio
            };

            _context.Bakugans.Add(bakugan);
            int filasAfectadas = await _context.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                throw new DbUpdateException("No se pudo guardar el Bakugan en la base de datos");
            }

            return new BakuganDetailDTO
            {
                Id = bakugan.Id,
                Nombre = bakugan.Nombre,
                Tipo = bakugan.Tipo,
                Habilidades = new(),
            };
        
    }

    // ************* TRAER BAKUGAN POR ID **************
    public async Task<BakuganDetailDTO?> TraerBakuganPorIdServicio(int id)

            
    {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor a 0");
            }

            var bakugan = await _context.Bakugans
                .Include(h => h.Habilidades)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bakugan == null)
            {
                throw new InvalidOperationException($"No se encontró el Bakugan con ID {id}");
            }

            return new BakuganDetailDTO
            {
                Id = bakugan.Id,
                Nombre = bakugan.Nombre,
                Tipo = bakugan.Tipo,
            };

    }

    // ************* TRAER TODOS LOS BAKUGANES **************
    public async Task<List<BakuganDetailDTO>> TraerBakugansServicio()
    {

            return await _context.Bakugans
                .Include(h => h.Habilidades)
                .Select(b => new BakuganDetailDTO
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Tipo = b.Tipo,
                    Habilidades = b.Habilidades.Select(h => new BakuganSkillDetailDTO
                    {
                        Id = h.Id,
                        Nombre = h.Nombre,
                        Dano = h.Dano,
                        BakuganId = h.BakuganId,
                        SkillDano = h.SkillDano
                    }).ToList()
                })
                .ToListAsync();
        

    }

    // ************ MODIFICAR BAKUGAN **************
    public async Task<BakuganDetailDTO> ModificarBakuganServicio(int id, BakuganUpdateDTO bakuganModel)
    {

            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor a 0");
            }
            

            var bakugan = await _context.Bakugans.FirstOrDefaultAsync(b => b.Id == id);

            if (bakugan == null)
            {
                throw new InvalidOperationException($"No se encontró el Bakugan con ID {id}");
            }

            // Validar nombre duplicado

            if (bakuganModel.Nombre != null) {

                if (string.IsNullOrEmpty(bakuganModel.Nombre)) {
                    throw new ArgumentException("El nombre no puede estar vacío.");
                }
                
                
                if (!bakugan.Nombre.Equals(bakuganModel.Nombre, StringComparison.OrdinalIgnoreCase))
                {
                    var nombreExiste = await _context.Bakugans
                        .AnyAsync(b => b.Id != id && b.Nombre.ToLower() == bakuganModel.Nombre.ToLower());

                    if (nombreExiste)
                    {
                        throw new InvalidOperationException($"Ya existe otro Bakugan con el nombre '{bakuganModel.Nombre}'");
                    }

                    bakugan.Nombre = bakuganModel.Nombre.Trim();
                }
            }
            if (bakuganModel.Category.HasValue)
            {
                if(!Enum.IsDefined(typeof(EBakuganCategoria), bakuganModel.Category.Value))
                {
                    throw new ArgumentException("El tipo de bakugan no es valido.");
                }
                bakugan.Category = bakuganModel.Category.Value; 
            }


            if (bakuganModel.Tipo.HasValue)
            {
                if(!Enum.IsDefined(typeof(EClasificacion), bakuganModel.Tipo.Value))
                {
                    throw new ArgumentException("El tipo de clasificacion no es vailido"); ;
                }
                bakugan.Tipo = bakuganModel.Tipo.Value;
            }

            if (bakuganModel.Precio.HasValue)
            {
                if (bakuganModel.Precio.Value < 0)
                {
                    throw new ArgumentException("El precio del bakugan no puede ser inferior a 0.");
                }

                bakugan.Precio = bakuganModel.Precio.Value;
            }

        int filasAfectadas = await _context.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                throw new DbUpdateException("No se pudo actualizar el Bakugan");
            }
            var bakuganReturn = new BakuganDetailDTO
            {
                Nombre = bakugan.Nombre,
                Tipo = bakugan.Tipo,
                Precio = bakugan.Precio,

            };
            return bakuganReturn;
        
    }

    // *********** BORRAR BAKUGAN *************
    public async Task<bool> BorrarBakuganServicio(int bakuganId)
    {

            if (bakuganId <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor a 0");
            }

            var bakugan = await _context.Bakugans.FindAsync(bakuganId);

            if (bakugan == null)
            {
                throw new InvalidOperationException($"No se encontró el Bakugan con ID {bakuganId}");
            }

            _context.Bakugans.Remove(bakugan);
            int filasAfectadas = await _context.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                throw new DbUpdateException("No se pudo eliminar el Bakugan");
            }

            return true;
        
    }

    // *************** SKILLS ******************

    // ************ AGREGAR SKILL **************
    public async Task<BakuganSkillDetailDTO> AgregarSkillServicio(int id, BakuganSkillAddDTO skillDto)
    {

            if (id <= 0)
            {
                throw new ArgumentException("El ID del Bakugan debe ser mayor a 0");
            }

            if (string.IsNullOrWhiteSpace(skillDto.Nombre))
            {
                throw new ArgumentException("El nombre de la habilidad es requerido");
            }

            var bakugan = await _context.Bakugans
                .Include(b => b.Habilidades)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bakugan == null)
            {
                throw new InvalidOperationException($"No se encontró el Bakugan con ID {id}");
            }

            // Verificar si ya tiene una skill con ese nombre
            var skillExiste = bakugan.Habilidades
                .Any(h => h.Nombre.ToLower() == skillDto.Nombre.ToLower());

            if (skillExiste)
            {
                throw new InvalidOperationException($"El Bakugan ya tiene una habilidad llamada '{skillDto.Nombre}'");
            }

            var nuevaSkill = new BakuganSkillModel
            {
                Nombre = skillDto.Nombre.Trim(),
                Dano = skillDto.Dano,
                BakuganId = id, 
                SkillDano = skillDto.SkillDano
            };

            bakugan.Habilidades.Add(nuevaSkill);
            int filasAfectadas = await _context.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                throw new DbUpdateException("No se pudo agregar la habilidad");
            }

            return new BakuganSkillDetailDTO
            {
                Id = nuevaSkill.Id,
                Nombre = nuevaSkill.Nombre,
                BakuganId = nuevaSkill.BakuganId,
                Dano = nuevaSkill.Dano,
                SkillDano = nuevaSkill.SkillDano
            };

    }

    // ************ ACTUALIZAR SKILL **************
    public async Task<BakuganSkillDetailDTO> ActualizarSkillServicio(int bakuganId, int skillId, BakuganSkillUpdateDTO skillUpdateDTO)
    {

            if (bakuganId <= 0 || skillId <= 0)
            {
                throw new ArgumentException("Los los ids deben ser mayores a 0");
            }

            

        var bakuganSkill = await _context.BakuganSkills
                .FirstOrDefaultAsync(b => b.Id == skillId && b.BakuganId == bakuganId);

            if (bakuganSkill == null)
            {
                throw new InvalidOperationException($"No se encontró la habilidad con ID {skillId} para el Bakugan {bakuganId}");
            }
            // Actualizar solo si se proporcionan valores
            if(skillUpdateDTO.Nombre != null)
            {
                if (!string.IsNullOrWhiteSpace(skillUpdateDTO.Nombre))
                {
                
                    var nombreExiste = await _context.BakuganSkills
                    .AnyAsync(s => s.BakuganId == bakuganId &&
                                   s.Id != skillId &&
                                   s.Nombre.ToLower() == skillUpdateDTO.Nombre.ToLower());

                    if (nombreExiste)
                    {
                        throw new InvalidOperationException($"Ya existe otra habilidad con el nombre '{skillUpdateDTO.Nombre}'");
                    }

                    bakuganSkill.Nombre = skillUpdateDTO.Nombre.Trim();
                } else {
                    throw new ArgumentException("El nombre de la habilidad no puede estar vacío.");
                }
            }

            if (skillUpdateDTO.SkillDano.HasValue)
            {
                if (Enum.IsDefined(typeof(EIntensidadDano), skillUpdateDTO.SkillDano.Value))
                {
                    bakuganSkill.SkillDano = skillUpdateDTO.SkillDano.Value;
                }
                else
                {
                throw new ArgumentException("La intensidad de daño no es válida");
                }
            }

            bakuganSkill.Dano = skillUpdateDTO.Dano ?? throw new ArgumentException("El daño no puede ser nulo.");

            int filasAfectadas = await _context.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                throw new DbUpdateException("No se pudo actualizar la habilidad");
            }

            return new BakuganSkillDetailDTO
            {
                Id = bakuganSkill.Id,
                Nombre = bakuganSkill.Nombre,
                BakuganId = bakuganSkill.BakuganId,
                Dano = bakuganSkill.Dano,
                SkillDano = bakuganSkill.SkillDano
            };
        
    }

    // ************ TRAER TODAS LAS SKILLS DE UN BAKUGAN ***************
    public async Task<List<BakuganSkillDetailDTO>> TraerSkillsBakugan(int bakuganId)
    {

            if (bakuganId <= 0)
            {
                throw new ArgumentException("El ID del Bakugan debe ser mayor a 0");
            }

            var bakugan = await _context.Bakugans
                .Include(h => h.Habilidades)
                .FirstOrDefaultAsync(b => b.Id == bakuganId);

            if (bakugan == null)
            {
                throw new InvalidOperationException($"No se encontró el Bakugan con ID {bakuganId}");
            }

            var habilidadesDTO = bakugan.Habilidades.Select(h => new BakuganSkillDetailDTO
            {
                Id = h.Id,
                Nombre = h.Nombre,
                Dano = h.Dano,
                BakuganId = h.BakuganId,
                SkillDano = h.SkillDano
            }).ToList();

            return habilidadesDTO;
        
    }
}
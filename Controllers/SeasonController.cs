using Microsoft.AspNetCore.Mvc;

namespace EstacionesMeteorologicas.Controllers;

[ApiController]
[Route("[controller]")]
public class SeasonController : ControllerBase
{
    private readonly ILogger<SeasonController> _logger;
    public static readonly List<Season> seasons = new()
    {
        new Season
        {
            Id = 1,
            Name = "Invierno",
            Place = "Cordoba",
            Status = true    
        }
    }; 
    public SeasonController(ILogger<SeasonController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Season newSeason)
    {
        try
        {
            if (newSeason.Id <= 0)
            {
                return BadRequest("Ingresa un id valido.");
            }

            bool idExists = seasons.Any(S => S.Id == newSeason.Id);

            if (idExists)
            {
                return Conflict("El ID ya existe.");
            }
            
            bool nameExists = seasons.Any(S => S.Name == newSeason.Name);

            if (String.IsNullOrWhiteSpace(newSeason.Name))
            {
                return BadRequest("Registre el nombre de la estacion.");
            }

            if (nameExists)
            {
                return Conflict("La estacion que intentas ingresar ya existe.");
            }

            return Ok("Estacion registrada correctamente.");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }
}

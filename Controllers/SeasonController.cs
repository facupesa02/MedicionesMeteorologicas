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

            if (string.IsNullOrWhiteSpace(newSeason.Name))
            {
                return BadRequest("Registre el nombre de la estacion.");
            }

            seasons.Add(newSeason);
            return Ok("Estacion registrada correctamente.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }

    [HttpGet("{id}/measurings")]
    public IActionResult GetMeasuringsBySeason(int id)
    {
        try
        {
            Season seasonExists = seasons.FirstOrDefault(S => S.Id == id);

            if (seasonExists is null)
            {
                return NotFound("La estacion con el id ingresado no existe.");
            }

            var measurings = Season.SeasonMeasurings.Where(m => m.SeasonId == id)
                                                    .ToList();

            return Ok(measurings);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }

    [HttpGet("Place")]
    public IActionResult GetByPlace(string place)
    {
        try
        {
            if (seasons.Count == 0)
            {
                return NotFound("No se encuentran estaciones registradas.");
            }

            var seasonPlace = seasons.Where(S => S.Place == place)
                                    .ToList();

            if (seasonPlace is null)
            {
                return NotFound("No se encuentran estaciones registradas en esa localidad.");
            }
            
            return Ok(seasonPlace);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }
}

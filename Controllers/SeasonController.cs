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
            Status = true,
            SeasonMeasurings =
            {
                new Measuring
                {
                    Id = 1,
                    SeasonId = 1,
                    Temperature = 34,
                    Dampness = 30,
                    WindSpeed = 20,
                    DateTime = new DateTime(2026, 08, 24, 12, 00, 00)
                }
            }
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
            var season = seasons.FirstOrDefault(S => S.Id == id);

            if (season is null)
            {
                return NotFound("La estacion con el id ingresado no existe.");
            }

            return Ok(season.SeasonMeasurings);
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

            if (seasonPlace.Count == 0)
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

    [HttpGet("Mediciones/Estacion")]
    public IActionResult GetMeasuringsBySeason()
    {
        var measurings = seasons.Select(S => S.SeasonMeasurings).ToList();

        if (measurings.Count == 0)
        {
            return NotFound("No hay mediciones registradas aun.");
        }

        return Ok(measurings);
    }
}

using Microsoft.AspNetCore.Mvc;

namespace EstacionesMeteorologicas.Controllers;

[ApiController]
[Route("[controller]")]
public class MeasuringController : ControllerBase
{
    private readonly ILogger<MeasuringController> _logger;

    private static readonly List<Measuring> measurings = new()
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
    };
    public MeasuringController(ILogger<MeasuringController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Measuring newMeasuring)
    {
        try
        {
            if (newMeasuring.Id <= 0)
            {
                return BadRequest("Ingresa un id valido.");
            }

            bool idExists = measurings.Any(S => S.Id == newMeasuring.Id);

            if (idExists)
            {
                return Conflict("El ID ya existe.");
            }

            if (newMeasuring.SeasonId <= 0)
            {
                return BadRequest("Ingresa un id valido.");
            }

            bool seasonIdExists = SeasonController.seasons.Any(S => S.Id == newMeasuring.SeasonId);

            if (seasonIdExists == false)
            {
                return NotFound("La estacion con el id ingresado no existe.");
            }
            
            if (seasonIdExists)
            {
                Season.SeasonMeasurings.Add(newMeasuring);
            }

            
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }
}
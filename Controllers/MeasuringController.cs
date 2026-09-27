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

            var statusValidation = SeasonController.seasons.FirstOrDefault(S => S.Id == newMeasuring.SeasonId);

            if (statusValidation.Status == false)
            {
                return Conflict("La estacion se encuentra inactiva.");
            }

            if (newMeasuring.DateTime > DateTime.Now)
            {
                return BadRequest("La fecha del registro no puede ser futura.");
            }

            if (newMeasuring.WindSpeed < 0)
            {
                return BadRequest("La velocidad del viento no puede ser negativa.");
            }

            if (newMeasuring.Dampness < 0 || newMeasuring.Dampness > 100)
            {
                return BadRequest("La humedad debe estar entre 0 y 100.");
            }

            measurings.Add(newMeasuring);
            Season.SeasonMeasurings.Add(newMeasuring);

            return Ok("Medicion tomada exitosamente.");

        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            if (measurings.Count == 0)
            {
                return NotFound("No hay mediciones registradas aun.");
            }

            return Ok(measurings);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }

    [HttpGet("Temperature")]
    public IActionResult GetByTemperature(double temperature)
    {
        try
        {
            if(measurings.Count == 0)
            {
                return NotFound("No hay mediciones registradas aun.");
            }

            var temperatures = measurings.Where(M => M.Temperature >= temperature)
                                        .ToList();
            
            if (temperatures is null)
            {
                return NotFound($"No se registraron temperaturas mayores a {temperature} grados."); 
            }

            return Ok(temperatures);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }
}
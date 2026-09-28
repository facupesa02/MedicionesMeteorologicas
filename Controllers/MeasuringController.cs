using Microsoft.AspNetCore.Mvc;

namespace EstacionesMeteorologicas.Controllers;

[ApiController]
[Route("[controller]")]
public class MeasuringController : ControllerBase
{
    private readonly ILogger<MeasuringController> _logger;

    private static IEnumerable<Measuring> Measurings =>
        SeasonController.seasons.SelectMany(season => season.SeasonMeasurings);
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

            bool idExists = Measurings.Any(measuring => measuring.Id == newMeasuring.Id);

            if (idExists)
            {
                return Conflict("El ID ya existe.");
            }

            if (newMeasuring.SeasonId <= 0)
            {
                return BadRequest("Ingresa un id valido.");
            }

            var season = SeasonController.seasons.FirstOrDefault(S => S.Id == newMeasuring.SeasonId);
            if (season is null)
            {
                return NotFound("La estacion con el id ingresado no existe.");
            }

            if (season.Status == false)
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

            season.SeasonMeasurings.Add(newMeasuring);

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
            var measurings = Measurings.ToList();
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
            var measurings = Measurings.ToList();
            if(measurings.Count == 0)
            {
                return NotFound("No hay mediciones registradas aun.");
            }

            var temperatures = measurings.Where(M => M.Temperature >= temperature)
                                        .ToList();
            
            if (temperatures.Count == 0)
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

    [HttpGet("Order")]
    public IActionResult GetByOrder()
    {
        try
        {
            var Order = Measurings.OrderByDescending(M => M.Temperature).ToList();

            if(Order.Count == 0)
            {
                return NotFound("No hay mediciones registradas aun.");
            }

            return Ok(Order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }

    [HttpGet("Promedio")]
    public IActionResult GetAvg()
    {
        try
        {
            var average = Measurings.Average(A => A.Temperature);

            if(average == 0)
            {
                return NotFound("No hay tempearturas registradas aun.");
            }

            return Ok(average);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }

    [HttpGet("MaximaTemp")]
    public IActionResult GetMax()
    {
        try
        {
            var maxTemp = Measurings.Max(M => M.Temperature);

            if (maxTemp == 0)
            {
                return NotFound("No hay tempearturas registradas aun.");
            }

            return Ok(maxTemp);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }

    [HttpGet("MinimaTemp")]
    public IActionResult GetMin()
    {
        try
        {
            var minTemp = Measurings.Min(M => M.Temperature);

            if (minTemp == 0)
            {
                return NotFound("No hay tempearturas registradas aun.");
            }

            return Ok(minTemp);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ha ocurrido un error en el servidor {ex.Message}");
        }
    }
}
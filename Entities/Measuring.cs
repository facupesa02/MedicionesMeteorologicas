namespace EstacionesMeteorologicas;
public class Measuring
{
    private int id;
    private int seasonId;
    private double temperature;
    private double dampness;
    private double windSpeed;
    private DateTime dateTime;

    public int Id { get => id; set => id = value; }
    public int SeasonId { get => seasonId; set => seasonId = value; }
    public double Temperature { get => temperature; set => temperature = value; }
    public double Dampness { get => dampness; set => dampness = value; }
    public double WindSpeed { get => windSpeed; set => windSpeed = value; }
    public DateTime DateTime { get => dateTime; set => dateTime = value; }
}
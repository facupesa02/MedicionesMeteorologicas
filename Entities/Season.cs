namespace EstacionesMeteorologicas;
public class Season
{
    private int id;
    private string name;
    private string place;
    private bool status;
    public static readonly List<Measuring> SeasonMeasurings = new();

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string Place { get => place; set => place = value; }
    public bool Status { get => status; set => status = value; }
}
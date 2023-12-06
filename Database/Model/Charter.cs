namespace CaribbeanSailboat.Database.Model;

public class Charter
{
    public int CharterId { get; set; }
    public int CustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

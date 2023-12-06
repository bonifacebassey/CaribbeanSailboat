namespace CaribbeanSailboat.Database.Contract;

public interface ICharter : ICrudObject<ICharter>
{
    int CharterId { get; set; }
    int CustomerId { get; set; }
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
}

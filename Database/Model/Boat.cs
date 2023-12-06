namespace CaribbeanSailboat.Database.Model;

public class Boat
{
    public int BoatId { get; set; }
    public int OwnerId { get; set; }
    public string? Name { get; set; }
    public string? Size { get; set; }
    public string? RentalCost { get; set; }
}

namespace CaribbeanSailboat.Database.Contract;

public interface IBoat : ICrudObject<IBoat>
{
    int BoatId { get; set; }
    int OwnerId { get; set; }
    string? Name { get; set; }
    string? Size { get; set; }
    string? RentalCost { get; set; }

    List<IBoat> GetAllBoats();
}

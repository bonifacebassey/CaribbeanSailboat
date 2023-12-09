using Dapper;
using Oracle.ManagedDataAccess.Client;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using System.Data;

namespace CaribbeanSailboat.Database.ModelImpl;

public class BoatImpl : IBoat
{
    public BoatImpl(Boat boat)
    {
        Boat = boat;
    }

    public Boat Boat { get; set; }
    public int BoatId { get => Boat.BoatId; set => Boat.BoatId = value; }
    public int OwnerId { get => Boat.OwnerId; set => Boat.OwnerId = value; }
    public string? Name { get => Boat.Name; set => Boat.Name = value; }
    public string? Size { get => Boat.Size; set => Boat.Size = value; }
    public string? RentalCost { get => Boat.RentalCost; set => Boat.RentalCost = value; }

    public IBoat CreateItem()
    {
        return new BoatImpl(new Boat());
    }

    public void AddItem(IBoat boat)
    {
        throw new NotImplementedException();
    }

    public void DeleteItem(IBoat boat)
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(IBoat boat)
    {
        throw new NotImplementedException();
    }

    public List<IBoat> GetAllBoats()
    {
        List<IBoat> boats = new List<IBoat>();

        try
        {
            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                using (OracleCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM BOAT";

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var boat = new Boat
                            {
                                BoatId = reader.GetInt32(reader.GetOrdinal("BOAT_ID")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OWNER_ID")),
                                Name = reader["BOAT_NAME"] is DBNull ? null : reader.GetString(reader.GetOrdinal("BOAT_NAME")),
                                Size = reader["BOAT_SIZE"] is DBNull ? null : reader.GetString(reader.GetOrdinal("BOAT_SIZE")),
                                RentalCost = reader["BOAT_RENTAL_COST"] is DBNull ? null : reader.GetString(reader.GetOrdinal("BOAT_RENTAL_COST"))
                            };

                            boats.Add(new BoatImpl(boat));
                        }
                    }
                }
            }
        }
        catch { }
        return boats;
    }
}

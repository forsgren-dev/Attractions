using Seido.Utilities.SeedGenerator;

namespace Models;

public class City : ICity
{

    public virtual Guid CityId { get; set; }
    public string CityName { get; set; }

    public virtual List<IAddress> Addresses { get; set; }
    

}



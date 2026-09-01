using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

public class AttractionDbM : Attraction
{

    [Key]
    public override Guid AttractionId { get; set; }

    #region constructor
    public AttractionDbM() { }

    #endregion

}







using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DbModels;

public class CommentDbM : Comment
{
    
 [Key]
    public override Guid CommentId { get; set; }

}
   

    




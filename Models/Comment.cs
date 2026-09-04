using Seido.Utilities.SeedGenerator;

namespace Models;

public class Comment : IComment
{

    public virtual Guid CommentId { get; set; }
    public string CommentText { get; set; }

    public virtual IAttraction Attraction { get; set; }

    public virtual IUser User { get; set; }

}



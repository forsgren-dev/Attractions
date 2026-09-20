using Seido.Utilities.SeedGenerator;

namespace Models;

public class Comment : IComment, ISeed<Comment>
{

    public virtual Guid CommentId { get; set; }
    public string CommentText { get; set; }

    public virtual IAttraction Attraction { get; set; }

    public virtual IUser User { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Seeded { get; set; } = false;

    public virtual Comment Seed(SeedGenerator seeder)
    {
        CommentId = Guid.NewGuid();
        CommentText = seeder.LatinSentence;
        CreatedAt = DateTime.UtcNow;
        Seeded = true;
        return this;
    }
}



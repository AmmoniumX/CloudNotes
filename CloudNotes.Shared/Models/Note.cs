namespace CloudNotes.Shared.Models;

public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public NoteMetadata Metadata { get; set; } = new NoteMetadata();
    public string Content { get; set; } = string.Empty;

    // Parameterless constructor for JSON deserialization
    public Note(){}

    public Note(string title, string content)
    {
        Metadata = new NoteMetadata
        {
            Id = Id,
            Title = title,
            CreatedAt = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            Summary = content.Length > 100 ? content.Substring(0, 100) + "..." : content
        };
        Content = content;
    }
}
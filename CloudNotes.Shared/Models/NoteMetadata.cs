namespace CloudNotes.Shared.Models;

public class NoteMetadata
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;

    public string Summary { get; set; } = string.Empty;
}

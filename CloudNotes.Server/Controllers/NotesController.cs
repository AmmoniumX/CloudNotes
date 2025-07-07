using Microsoft.AspNetCore.Mvc;
using CloudNotes.Shared.Models;
using System.Text.Json;

namespace CloudNotes.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private static readonly string NotesFolder = Path.Combine(AppContext.BaseDirectory, "notes");

    // Ensure notes directory exists
    public NotesController()
    {
        if (!Directory.Exists(NotesFolder))
        {
            Directory.CreateDirectory(NotesFolder);
        }
    }

    

    // Load all notes from JSON files
    private List<NoteMetadata> LoadNotesMetadata()
    {
        var notes = new List<NoteMetadata>();

        foreach (var file in Directory.GetFiles(NotesFolder, "*.meta.json"))
        {
            try
            {
                var json = System.IO.File.ReadAllText(file);
                var note = JsonSerializer.Deserialize<NoteMetadata>(json);
                if (note != null) { notes.Add(note); }
            }
            catch
            {
                Console.WriteLine($"Error reading note file: {file}");
                continue; // Skip files that can't be read
            }
        }

        return notes.OrderByDescending(n => n.CreatedAt).ToList();
    }

    // Save a single note to JSON file
    private void SaveNoteToFile(Note note)
    {
        var metadataPath = Path.Combine(NotesFolder, $"{note.Id}.meta.json");
        var json = JsonSerializer.Serialize(note.Metadata, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(metadataPath, json);

        var contentPath = Path.Combine(NotesFolder, $"{note.Id}.content.json");
        System.IO.File.WriteAllText(contentPath, note.Content);
    }

    private void SaveNoteToFile(NoteMetadata note, string content)
    {
        var metadataPath = Path.Combine(NotesFolder, $"{note.Id}.meta.json");
        var json = JsonSerializer.Serialize(note, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(metadataPath, json);

        if (!string.IsNullOrEmpty(content))
        {
            var contentPath = Path.Combine(NotesFolder, $"{note.Id}.content.json");
            System.IO.File.WriteAllText(contentPath, content);
        }
    }

    // Delete note file
    private void DeleteNote(Guid id)
    {
        var metadataPath = Path.Combine(NotesFolder, $"{id}.meta.json");
        if (System.IO.File.Exists(metadataPath))
        {
            System.IO.File.Delete(metadataPath);
        }

        var contentPath = Path.Combine(NotesFolder, $"{id}.content.json");
        if (System.IO.File.Exists(contentPath))
        {
            System.IO.File.Delete(contentPath);
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        List<NoteMetadata> metadata = LoadNotesMetadata();

        return Ok(metadata);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var metadataPath = Path.Combine(NotesFolder, $"{id}.meta.json");
        if (!System.IO.File.Exists(metadataPath)) return NotFound();
        var json = System.IO.File.ReadAllText(metadataPath);
        var metadata = JsonSerializer.Deserialize<NoteMetadata>(json);


        return Ok(metadata);
    }

    [HttpGet("{id}/content")]
    public IActionResult GetContent(Guid id)
    {
        var contentPath = Path.Combine(NotesFolder, $"{id}.content.json");
        if (!System.IO.File.Exists(contentPath)) return NotFound();
        var content = System.IO.File.ReadAllText(contentPath);

        return Ok(content);
    }

    [HttpPost]
    public IActionResult Post(Note note)
    {
        Console.WriteLine($"Creating new note: {note.Metadata.Title}");
        SaveNoteToFile(note);
        return Ok(note);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Note updatedNote)
    {
        var notesMeta = LoadNotesMetadata();
        var metadata = notesMeta.FirstOrDefault(n => n.Id == id);
        if (metadata == null) return NotFound();

        metadata.Title = updatedNote.Metadata.Title;
        metadata.Summary = updatedNote.Metadata.Summary;
        metadata.LastModified = updatedNote.Metadata.LastModified;

        SaveNoteToFile(metadata, updatedNote.Content);
        return Ok(metadata);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var notes = LoadNotesMetadata();
        var existing = notes.FirstOrDefault(n => n.Id == id);
        if (existing == null) return NotFound();

        DeleteNote(id);
        return NoContent();
    }
}

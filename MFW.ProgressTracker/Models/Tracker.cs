namespace MFW.ProgressTracker.Models;

public class Tracker
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Name { get; set; }
}

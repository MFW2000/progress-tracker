using System.ComponentModel.DataAnnotations;

namespace MFW.ProgressTracker.Models;

public class Tracker
{
    public required Guid Id { get; init; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }
}

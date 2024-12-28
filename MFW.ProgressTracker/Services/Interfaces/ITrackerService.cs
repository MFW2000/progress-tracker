using MFW.ProgressTracker.Models;

namespace MFW.ProgressTracker.Services.Interfaces;

/// <summary>
/// Defines a contract for a data service that manages tracker items.
/// </summary>
public interface ITrackerService
{
    /// <summary>
    /// Get all tracker items from the local browser storage.
    /// If no trackers were found, the local browser storage will be initialized with an empty array instead.
    /// Potential errors during deserialization are handled by returning an empty list,
    /// as this error cannot be recovered.
    /// </summary>
    /// <returns>List of trackers items, or an empty list if no trackers were found.</returns>
    Task<List<Tracker>> GetTrackers();

    /// <summary>
    /// Validate and create a new tracker item to save in the local browser storage.
    /// </summary>
    /// <param name="newTracker">The tracker to be created.</param>
    Task CreateTracker(Tracker newTracker);

    /// <summary>
    /// Validate and update an existing tracker item for saving in local browser storage.
    /// </summary>
    /// <param name="updatedTracker">The tracker to be updated.</param>
    Task UpdateTracker(Tracker updatedTracker);

    /// <summary>
    /// Delete the tracker item with the given ID from local browser storage.
    /// </summary>
    /// <param name="trackerId">The ID of the tracker to be deleted.</param>
    Task DeleteTracker(Guid trackerId);

    Task<string> ExportToJson();

    Task ImportFromJson(string json);
}

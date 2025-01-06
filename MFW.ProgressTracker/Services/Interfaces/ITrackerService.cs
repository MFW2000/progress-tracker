using MFW.ProgressTracker.Exceptions;
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
    /// <exception cref="ProgressTrackerException">Thrown if the new tracker is not valid.</exception>
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
    /// <exception cref="ProgressTrackerException">Thrown when no tracker items could be found.</exception>
    Task DeleteTracker(Guid trackerId);

    /// <summary>
    /// Exports all tracker items to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of the trackers, or an empty string if there are no trackers.</returns>
    /// <exception cref="ProgressTrackerException">Thrown when an error occurs during the serialization process.</exception>
    Task<string> ExportToJson();

    /// <summary>
    /// Imports tracker items using the given JSON string.
    /// </summary>
    /// <param name="json">The JSON string containing tracker items.</param>
    /// <exception cref="ProgressTrackerException">Thrown when an error occurs during the deserialization process.</exception>
    Task ImportFromJson(string json);
}

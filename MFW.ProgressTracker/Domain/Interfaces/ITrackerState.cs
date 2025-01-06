using MFW.ProgressTracker.Exceptions;
using MFW.ProgressTracker.Models;

namespace MFW.ProgressTracker.Domain.Interfaces;

/// <summary>
/// Defines a contract for a state that puts tracker items in memory.
/// </summary>
public interface ITrackerState
{
    /// <summary>
    /// Represents an event that is triggered when the state changes.
    /// </summary>
    event Action? OnChange;

    /// <summary>
    /// Set the tracker items from the state as a readonly list to prevent direct access to state data.
    /// </summary>
    IReadOnlyList<Tracker> Trackers { get; }

    /// <summary>
    /// Initialize all tracker items from the local browser storage.
    /// </summary>
    Task InitializeTrackers();

    /// <summary>
    /// Remove a tracker item from memory and local browser storage.
    /// </summary>
    /// <param name="trackerId">The ID of tracker to remove.</param>
    /// <exception cref="ProgressTrackerException">Thrown if the tracker could not be removed.</exception>
    Task RemoveTracker(Guid trackerId);
}

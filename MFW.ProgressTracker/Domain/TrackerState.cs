using MFW.ProgressTracker.Domain.Interfaces;
using MFW.ProgressTracker.Exceptions;
using MFW.ProgressTracker.Models;
using MFW.ProgressTracker.Services.Interfaces;

namespace MFW.ProgressTracker.Domain;

/// <summary>
/// Provides a concrete implementation of <see cref="ITrackerState"/> for managing tracker items in memory.
/// </summary>
/// <param name="trackerService">Dependency injection that allows access to the local browser storage.</param>
public class TrackerState(ITrackerService trackerService) : ITrackerState
{
    private readonly List<Tracker> _trackers = [];

    /// <inheritdoc/>
    public event Action? OnChange;

    /// <summary>
    /// Notifies clients that the state has changed by invoking the <see cref="OnChange"/> event.
    /// </summary>
    private void NotifyStateChanged() => OnChange?.Invoke();

    /// <inheritdoc/>
    public IReadOnlyList<Tracker> Trackers => _trackers.AsReadOnly();

    /// <inheritdoc/>
    public async Task InitializeTrackers()
    {
        var result = await trackerService.GetTrackers();

        _trackers.Clear();
        _trackers.AddRange(result);

        NotifyStateChanged();
    }

    /// <inheritdoc/>
    public async Task RemoveTracker(Guid trackerId)
    {
        try
        {
            await trackerService.DeleteTracker(trackerId);
        }
        catch (Exception exception)
        {
            throw new ProgressTrackerException(exception.Message);
        }

        _trackers.RemoveAll(t => t.Id == trackerId);

        NotifyStateChanged();
    }
}

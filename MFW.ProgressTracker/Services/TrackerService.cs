using System.Text.Json;
using MFW.ProgressTracker.Enumerations;
using MFW.ProgressTracker.Exceptions;
using MFW.ProgressTracker.Models;
using MFW.ProgressTracker.Services.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MFW.ProgressTracker.Services;

/// <summary>
/// Provides a concrete implementation of <see cref="ITrackerService"/> for managing tracker items.
/// </summary>
/// <param name="localStorage">Dependency injection for local browser storage.</param>
/// <param name="logger">Dependency injection that allows logging.</param>
/// <param name="notificationService">Dependency injection that allows sending notifications to the client.</param>
public class TrackerService(
    ProtectedLocalStorage localStorage,
    ILogger<TrackerService> logger,
    INotificationService notificationService)
    : ITrackerService
{
    private const string StorageKey = "trackers";

    /// <inheritdoc/>
    public async Task<List<Tracker>> GetTrackers()
    {
        var trackersStorageResult = await localStorage.GetAsync<string>(StorageKey);

        if (!trackersStorageResult.Success || string.IsNullOrEmpty(trackersStorageResult.Value))
        {
            await SetTrackers([]);

            return [];
        }

        var trackers = new List<Tracker>();

        try
        {
            trackers = JsonSerializer.Deserialize<List<Tracker>>(trackersStorageResult.Value);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Constants.ReadLocalStorageException);
            notificationService.ShowNotification(SemanticVariant.Danger, Constants.ReadLocalStorageException);
        }

        return trackers ?? [];
    }

    /// <inheritdoc/>
    public async Task CreateTracker(Tracker newTracker)
    {
        try
        {
            ValidateTrackerData(newTracker);
        }
        catch (ArgumentException exception)
        {
            throw new ProgressTrackerException(exception.Message);
        }

        var trackers = await GetTrackers();

        if (trackers.Count == 0)
        {
            trackers.Add(newTracker);

            await SetTrackers(trackers);

            return;
        }

        var isExistingTracker = trackers.Any(tracker => tracker.Id == newTracker.Id);

        if (isExistingTracker)
        {
            throw new ProgressTrackerException(Constants.TrackerAlreadyExistsException);
        }

        trackers.Add(newTracker);

        await SetTrackers(trackers);
    }

    /// <inheritdoc/>
    public async Task UpdateTracker(Tracker updatedTracker)
    {
        // TODO: Refactor error handling.

        try
        {
            ValidateTrackerData(updatedTracker);
        }
        catch (Exception exception)
        {
            notificationService.ShowNotification(SemanticVariant.Warning, exception.Message);

            return;
        }

        var trackers = await GetTrackers();

        if (trackers.Count == 0)
        {
            notificationService.ShowNotification(SemanticVariant.Warning, Constants.NoTrackersForUpdateException);

            return;
        }

        var existingTrackerIndex = trackers.FindIndex(tracker => tracker.Id == updatedTracker.Id);

        if (existingTrackerIndex == -1)
        {
            notificationService.ShowNotification(SemanticVariant.Warning, Constants.TrackerNotFoundException);

            return;
        }

        trackers[existingTrackerIndex] = updatedTracker;

        await SetTrackers(trackers);
    }

    /// <inheritdoc/>
    public async Task DeleteTracker(Guid trackerId)
    {
        var trackers = await GetTrackers();

        if (trackers.Count == 0)
        {
            throw new ProgressTrackerException(Constants.NoTrackersToDeleteException);
        }

        trackers.RemoveAll(tracker => tracker.Id == trackerId);

        await SetTrackers(trackers);
    }

    /// <inheritdoc/>
    public async Task<string> ExportToJson()
    {
        // TODO: Consider leaving out the IDs of the tracker items.

        var trackers = await GetTrackers();

        if (trackers.Count == 0)
        {
            return string.Empty;
        }

        try
        {
            return JsonSerializer.Serialize(trackers);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Constants.ExportTrackersException);

            throw new ProgressTrackerException(Constants.ExportTrackersException);
        }
    }

    /// <inheritdoc/>
    public async Task ImportFromJson(string json)
    {
        // TODO: Verify the imported tracker data before saving.
        // TODO: This method currently replaces everything upon import, this should be changed.

        List<Tracker> importedTrackers;

        try
        {
            importedTrackers = JsonSerializer.Deserialize<List<Tracker>>(json) ?? [];
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Constants.ImportTrackersException);

            throw new ProgressTrackerException(Constants.ImportTrackersException);
        }

        if (importedTrackers.Count == 0)
        {
            notificationService.ShowNotification(SemanticVariant.Warning, Constants.NoTrackersForImportException);
        }

        await SetTrackers(importedTrackers);
    }

    /// <summary>
    /// Stores the given list of tracker items into the local browser storage.
    /// </summary>
    /// <param name="trackers">List of tracker items to save.</param>
    private async Task SetTrackers(List<Tracker> trackers)
    {
        await localStorage.SetAsync(StorageKey, JsonSerializer.Serialize(trackers));
    }

    /// <summary>
    /// Validates the data of a <see cref="Tracker"/> object to ensure it meets required criteria.
    /// </summary>
    /// <param name="tracker">The tracker item to validate.</param>
    /// <exception cref="ArgumentException">Thrown if a property does not meet the required criteria.</exception>
    private static void ValidateTrackerData(Tracker tracker)
    {
        if (string.IsNullOrWhiteSpace(tracker.Name))
        {
            throw new ArgumentException(Constants.TrackerHasNoNameException);
        }
    }
}

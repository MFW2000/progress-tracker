using System.Text.Json;
using MFW.ProgressTracker.Enumerations;
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
        catch (Exception exception)
        {
            notificationService.ShowNotification(SemanticVariant.Warning, exception.Message);

            return;
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
            notificationService.ShowNotification(SemanticVariant.Warning, Constants.TrackerAlreadyExistsException);

            return;
        }

        trackers.Add(newTracker);

        await SetTrackers(trackers);
    }

    /// <inheritdoc/>
    public async Task UpdateTracker(Tracker updatedTracker)
    {
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
            notificationService.ShowNotification(SemanticVariant.Warning, Constants.NoTrackersToDeleteException);

            return;
        }

        trackers.RemoveAll(tracker => tracker.Id == trackerId);

        await SetTrackers(trackers);
    }

    // TODO: Improve this method so it returns empty notification and json error notification.
    public async Task<string> ExportToJson()
    {
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
            logger.LogError(exception, "");
            notificationService.ShowNotification(SemanticVariant.Danger, "");
        }

        return string.Empty;
    }

    public async Task ImportFromJson(string json)
    {
        var trackers = JsonSerializer.Deserialize<List<Tracker>>(json) ?? [];

        await SetTrackers(trackers);
    }

    private async Task SetTrackers(List<Tracker> trackers)
    {
        await localStorage.SetAsync(StorageKey, JsonSerializer.Serialize(trackers));
    }

    private static void ValidateTrackerData(Tracker tracker)
    {
        if (string.IsNullOrWhiteSpace(tracker.Name))
        {
            throw new ArgumentException(Constants.TrackerHasNoNameException);
        }
    }
}

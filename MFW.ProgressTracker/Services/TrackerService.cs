using System.Text.Json;
using MFW.ProgressTracker.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MFW.ProgressTracker.Services;

public class TrackerService(ProtectedLocalStorage localStorage, ILogger<TrackerService> logger) : ITrackerService
{
    private const string StorageKey = "trackers";

    public async Task<List<Tracker>> GetTrackers()
    {
        var trackersStorageResult = await localStorage.GetAsync<string>(StorageKey);

        if (trackersStorageResult is not { Success: true, Value: not null })
        {
            return [];
        }

        var trackers = new List<Tracker>();

        try
        {
            trackers = JsonSerializer.Deserialize<List<Tracker>>(trackersStorageResult.Value);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred while reading trackers from localstorage.");
        }

        return trackers ?? [];
    }

    public async Task AddTracker(Tracker newTracker)
    {
        if (string.IsNullOrWhiteSpace(newTracker.Name))
        {
            throw new ArgumentException("The tracker item must have a name.");
        }

        var trackers = await GetTrackers();

        var isExistingTracker = trackers.Any(tracker => tracker.Id == newTracker.Id);

        if (isExistingTracker)
        {
            throw new ArgumentException("The tracker already exists.");
        }

        trackers.Add(newTracker);

        await SetTrackers(trackers);
    }

    public async Task UpdateTracker(Tracker updatedTracker)
    {
        if (string.IsNullOrWhiteSpace(updatedTracker.Name))
        {
            throw new ArgumentException("The tracker item must have a name.");
        }

        var trackers = await GetTrackers();

        var existingTrackerIndex = trackers.FindIndex(tracker => tracker.Id == updatedTracker.Id);

        if (existingTrackerIndex == -1)
        {
            throw new ArgumentException("The tracker does not exist.");
        }

        trackers[existingTrackerIndex] = updatedTracker;

        await SetTrackers(trackers);
    }

    public async Task DeleteTracker(Guid trackerId)
    {
        var trackers = await GetTrackers();

        trackers.RemoveAll(tracker => tracker.Id == trackerId);

        await SetTrackers(trackers);
    }

    public async Task<string> ExportToJson()
    {
        var trackers = await GetTrackers();

        return JsonSerializer.Serialize(trackers);
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
}

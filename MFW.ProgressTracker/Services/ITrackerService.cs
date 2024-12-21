using MFW.ProgressTracker.Models;

namespace MFW.ProgressTracker.Services;

public interface ITrackerService
{
    Task<List<Tracker>> GetTrackers();

    Task AddTracker(Tracker newTracker);

    Task UpdateTracker(Tracker updatedTracker);

    Task DeleteTracker(Guid trackerId);

    Task<string> ExportToJson();

    Task ImportFromJson(string json);
}

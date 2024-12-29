namespace MFW.ProgressTracker;

/// <summary>
/// Provides common text used throughout the application.
/// </summary>
public static class Constants
{
    // General
    public const string ApplicationName = "Progress Tracker";
    public const string TagLine = "Track your progress across various activities";

    // Common
    public const string Empty = "Nothing here, yet.";

    // Home
    public const string HomeTitle = "Home";

    // Trackers
    public const string TrackersTitle = "Trackers";

    // About
    public const string AboutTitle = "About";

    // Error
    public const string ErrorTitle = "Error";
    public const string ErrorSubtitle = "An error occurred while processing your request";
    public const string ErrorRequestIdHeading = "Request ID";

    // Accessibility
    public const string AriaNavigationBar = "Toggle navigation";
    public const string AriaClose = "Close";
    public const string LoadingSpinner = "Loading...";

    // Exceptions
    public const string ReadLocalStorageException =
        "Tracker item data in local storage was corrupted and could not be recovered.";
    public const string TrackerHasNoNameException = "The tracker item must have a name.";
    public const string TrackerAlreadyExistsException = "The tracker item already exists.";
    public const string TrackerNotFoundException = "The tracker item could not be found.";
    public const string NoTrackersForUpdateException =
        "The tracker could not be updated because no tracker items were found.";
    public const string NoTrackersToDeleteException =
        "The tracker could not be deleted because no tracker items were found.";
    public const string ExportTrackersException =
        "Something went wrong while exporting the tracker items to a JSON file.";
    public const string ImportTrackersException = "The given JSON file contains errors and cannot be imported.";
    public const string NoTrackersForImportException = "The given JSON file contains no tracker items.";
}

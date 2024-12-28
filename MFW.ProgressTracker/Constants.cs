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
        "Tracking data in local storage was corrupted and could not be recovered.";
    public const string TrackerHasNoNameException = "The tracker item must have a name.";
    public const string TrackerAlreadyExistsException = "The tracker already exists.";
    public const string TrackerNotFoundException = "The tracker could not be found.";
    public const string NoTrackersForUpdateException = "The tracker could not be updated because no trackers were found.";
    public const string NoTrackersToDeleteException = "The tracker could not be deleted because no trackers were found.";
}

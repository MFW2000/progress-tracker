using MFW.ProgressTracker.Enumerations;

namespace MFW.ProgressTracker.Services.Interfaces;

/// <summary>
/// Defines a contract for a notification service.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// An event that is triggered when a notification is shown.
    /// This event provides the notification's message and its semantic variant.
    /// </summary>
    event Action<SemanticVariant, string> OnShowNotification;

    /// <summary>
    /// Displays a notification with the specified message and semantic variant.
    /// </summary>
    /// <param name="variant">The variant of the notification.</param>
    /// <param name="message">The message of the notification.</param>
    void ShowNotification(SemanticVariant variant, string message);
}

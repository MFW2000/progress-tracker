using MFW.ProgressTracker.Enumerations;
using MFW.ProgressTracker.Services.Interfaces;

namespace MFW.ProgressTracker.Services;

/// <summary>
/// Provides a concrete implementation of <see cref="INotificationService"/> for managing notifications.
/// </summary>
public class NotificationService : INotificationService
{
    /// <inheritdoc/>
    public event Action<SemanticVariant, string>? OnShowNotification;

    /// <inheritdoc/>
    public void ShowNotification(SemanticVariant semanticVariant, string message)
    {
        OnShowNotification?.Invoke(semanticVariant, message);
    }
}

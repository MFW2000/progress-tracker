namespace MFW.ProgressTracker.Exceptions;

/// <summary>
/// The exception that is thrown when a custom application specific exception occurs.
/// </summary>
public class ProgressTrackerException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressTrackerException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ProgressTrackerException(string message) : base(message) {}
}

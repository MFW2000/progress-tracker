namespace MFW.ProgressTracker.Exceptions;

/// <summary>
/// Initializes a new instance of the <see cref="ProgressTrackerException"/> class with a specified error message.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
public class ProgressTrackerException(string message) : Exception(message);

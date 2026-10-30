namespace StageFlow.Application.Schedule.Exceptions;

public class PerformanceValidationException(string message) : InvalidOperationException(message);
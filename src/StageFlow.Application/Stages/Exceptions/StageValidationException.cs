namespace StageFlow.Application.Stages.Exceptions;

public class StageValidationException(string message) : InvalidOperationException(message);
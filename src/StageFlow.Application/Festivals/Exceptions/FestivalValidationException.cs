namespace StageFlow.Application.Festivals.Exceptions;

public class FestivalValidationException(string message) 
                : InvalidOperationException(message);
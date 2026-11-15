namespace StageFlow.Application.Festivals.Exceptions;

public class FestivalStatusTransitionException(string message) 
                : InvalidOperationException(message);
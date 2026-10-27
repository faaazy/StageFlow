namespace StageFlow.Application.Festivals.Exceptions;

public class FestivalAccessDeniedException() 
    : InvalidOperationException("You are not the organizer of this festival");
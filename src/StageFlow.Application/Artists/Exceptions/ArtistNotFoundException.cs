namespace StageFlow.Application.Artists.Exceptions;

public class ArtistNotFoundException() 
    : InvalidOperationException("Artist not found");
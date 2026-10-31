namespace StageFlow.Application.Artists.Exceptions;

public class ArtistValidationException(string message) : InvalidOperationException(message);
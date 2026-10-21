namespace StageFlow.Application.Auth;

public record LoginResult(
    bool Succeeded, 
    string? Token, 
    IReadOnlyList<string> Errors
);
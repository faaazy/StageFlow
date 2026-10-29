namespace StageFlow.Application.Stages.Exceptions;

public class StageNotFoundException() : InvalidOperationException("Stage not found");
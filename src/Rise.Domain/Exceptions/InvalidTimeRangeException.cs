namespace Rise.Domain.Exceptions;

public class InvalidTimeRangeException(string message) : Exception(
    message ?? "The provided time range is invalid."
    );
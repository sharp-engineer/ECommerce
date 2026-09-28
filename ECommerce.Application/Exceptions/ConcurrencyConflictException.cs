namespace ECommerce.Application.Exceptions;

public sealed class ConcurrencyConflictException(string message, Exception innerException)
    : Exception(message, innerException);
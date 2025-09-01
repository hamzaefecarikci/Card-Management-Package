namespace CardManagement.Shared.Exceptions;

public class CardManagementException : Exception
{
    public CardManagementException(string message) : base(message) { }
    public CardManagementException(string message, Exception innerException) : base(message, innerException) { }
}

public class ValidationException : CardManagementException
{
    public ValidationException(string message) : base(message) { }
}

public class NotFoundException : CardManagementException
{
    public NotFoundException(string message) : base(message) { }
}

public class UnauthorizedException : CardManagementException
{
    public UnauthorizedException(string message) : base(message) { }
}

public class InsufficientBalanceException : CardManagementException
{
    public InsufficientBalanceException(string message) : base(message) { }
} 
using System;

namespace LinkShortener.Domain.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException()
        : base("Unauthorized access")
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }

    public UnauthorizedException(string message, Exception inner)
        : base(message, inner)
    {
    }
} 
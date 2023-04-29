using System;

namespace Itixo.Timesheets.Shared.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { } 
}
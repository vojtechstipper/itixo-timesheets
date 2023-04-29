using System;

namespace Itixo.Timesheets.Shared.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
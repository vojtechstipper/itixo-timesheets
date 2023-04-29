using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Itixo.Timesheets.Shared.Exceptions;

public class AppValidationException : Exception
{
    public AppValidationException() : base("One or more validation failures have occurred.")
    {
        Failures = new Dictionary<string, string[]>();
    }

    public AppValidationException(List<ValidationFailure> failures)
        : this()
    {
        IEnumerable<string> propertyNames = failures
            .Select(e => e.PropertyName)
            .Distinct<string>();

        foreach (string propertyName in propertyNames)
        {
            string[] propertyFailures = failures
                .Where(e => e.PropertyName == propertyName)
                .Select(e => e.ErrorMessage)
                .ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }

    public IDictionary<string, string[]> Failures { get; }
}

using System.Runtime.CompilerServices;

namespace Ardalis.GuardClauses;

public static class RangeGuard
{
    /// <summary>
    /// Throws an exception if the input is outside the inclusive range.
    /// </summary>
    /// <param name="input">The value to check.</param>
    /// <param name="min">Minimum allowed value (inclusive).</param>
    /// <param name="max">Maximum allowed value (inclusive).</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    public static int BetweenMinAndMax(this IGuardClause guardClause,
        int input,
        int min,
        int max,
        [CallerArgumentExpression("input")] string? parameterName = null)
    {
        if (input < min || input > max)
            throw new ArgumentOutOfRangeException(parameterName,
                $"Value should be between {min} and {max} (inclusive)");
        return input;
    }
}

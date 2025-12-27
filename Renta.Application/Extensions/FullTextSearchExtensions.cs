using System;

namespace Renta.Application.Extensions;

public static class FullTextSearchExtensions
{
    /// <summary>
    /// Cleans a search string to make it safe for use with to_tsquery in PostgreSQL.
    /// Removes characters that have special meaning.
    /// </summary>
    /// <param name="query">The user's search text.</param>
    /// <returns>A sanitized string.</returns>
    public static string SanitizeForTsQuery(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return string.Empty;
        }
        var sanitizedQuery = query.Replace("&", " ")
            .Replace("|", " ")
            .Replace("!", " ")
            .Replace("(", " ")
            .Replace(")", " ")
            .Replace("'", " ")
            .Replace(":", " ")
            .Replace("*", " ");
        return sanitizedQuery;
    }
}

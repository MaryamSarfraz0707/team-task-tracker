using MySql.Data.MySqlClient;

namespace TeamTaskTracker.Api.Data;

public static class MySqlDataReaderExtensions
{
    public static long GetInt64(this MySqlDataReader reader, string column)
    {
        return reader.GetInt64(reader.GetOrdinal(column));
    }

    public static long? GetNullableInt64(this MySqlDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);
        return reader.IsDBNull(ordinal) ? null : reader.GetInt64(ordinal);
    }

    public static string GetString(this MySqlDataReader reader, string column)
    {
        return reader.GetString(reader.GetOrdinal(column));
    }

    public static string? GetNullableString(this MySqlDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    public static bool GetBoolean(this MySqlDataReader reader, string column)
    {
        return reader.GetBoolean(reader.GetOrdinal(column));
    }

    public static DateTime GetDateTime(this MySqlDataReader reader, string column)
    {
        return reader.GetDateTime(reader.GetOrdinal(column));
    }

    public static DateOnly? GetNullableDateOnly(this MySqlDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);
        return reader.IsDBNull(ordinal) ? null : DateOnly.FromDateTime(reader.GetDateTime(ordinal));
    }
}

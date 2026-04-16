using System;

public static class EnumExtensions
{
    public static string ToDisplayString(this Enum value)
    {
        return value.ToString().Replace("_", " ");
    }
}

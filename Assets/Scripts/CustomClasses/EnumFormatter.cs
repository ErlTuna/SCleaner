using System;

public static class EnumFormatter
{
    public static string ToDisplay(Enum value)
    {
        return value.ToString().Replace("_", " ");
    }
}

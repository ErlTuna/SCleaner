public struct ConditionResult
{
    public bool IsSucessful;
    public string Reason;

    public static ConditionResult Success() => new() { IsSucessful = true };
    public static ConditionResult Failure(string reason) => new() { IsSucessful = false, Reason = reason };
}

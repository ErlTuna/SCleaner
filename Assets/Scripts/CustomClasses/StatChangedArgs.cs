public struct StatChangedArgs
{   
    public int Previous;
    public int Current;
    public readonly int Delta => Current - Previous;

    public StatChangedArgs(int previous, int current)
    {
        Previous = previous;
        Current = current;
    }
}

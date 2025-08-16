using System;

public interface IPredicate
{
    public string Description {get; set;}
    bool Evaluate();
}

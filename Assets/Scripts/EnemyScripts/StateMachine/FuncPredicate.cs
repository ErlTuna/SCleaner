using System;

public class FuncPredicate : IPredicate
{
    public string Description {get; set;}

    readonly Func<bool> func;

    public FuncPredicate(Func<bool> func, string description = "default"){
        this.func = func;
        Description = description;
    }

    public bool Evaluate() => func.Invoke();

}
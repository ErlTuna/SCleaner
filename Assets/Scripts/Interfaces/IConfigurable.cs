// A configurable is denoted by this marker interface
public interface IConfigurable { }

// A configurable that can be configured by a specific configuration is denoted by this interface
// It is akin to "every square is a rectangle but not all rectangles are squares"
// Everything Configurable will implement IConfigurable but some may require specific configurations
public interface IConfigurable<TConfig> : IConfigurable
{
    public void ConfigureWith(TConfig config);

}


public interface INodeGraphGenerationStrategy
{
    public NodeGraph GenerateNodeGraph();
}

public interface INodeGraphGenerationManager
{
    public INodeGraphGenerationStrategy GetStrategy();
    public void SetStrategy(INodeGraphGenerationStrategy strategy);
}

[Injectable(typeof(INodeGraphGenerationManager))]
public class NodeGraphGenerationManager : INodeGraphGenerationManager
{
    private INodeGraphGenerationStrategy strategy;

    public NodeGraphGenerationManager()
    {
       
    }

    public void SetStrategy(INodeGraphGenerationStrategy generationStrategy)
    {
        strategy = generationStrategy;
    }

    public INodeGraphGenerationStrategy GetStrategy()
    {
        return strategy;
    }
}

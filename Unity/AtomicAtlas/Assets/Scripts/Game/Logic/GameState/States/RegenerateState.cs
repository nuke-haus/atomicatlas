
public class RegenerateState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.REGENERATE;
    public override GameStateType GetNextState() => GameStateType.GAMEPLAY;

    private ISettingsManager settingsManager;
    private IWorldGenerationManager worldGenerationManager;
    private bool isRegenComplete;

    public override bool CanExitState()
    {
        return isRegenComplete;
    }

    public override void OnEnter()
    {
        isRegenComplete = false;

        settingsManager = DependencyInjector.Resolve<ISettingsManager>();
        worldGenerationManager = DependencyInjector.Resolve<IWorldGenerationManager>();

        var world = worldGenerationManager.GenerateWorld(settingsManager.ActiveStrategy, settingsManager.ActiveStrategyDefinition, settingsManager.AllPlayerInfo);

        if (world != null)
        {
            NodeGraphManager.GlobalInstance.GenerateNodeGraphs(world);
        }
        else
        {
            isRegenComplete = true;
        }
    }

    public override void OnUpdate()
    {
        if (!NodeGraphManager.GlobalInstance.IsGeneratingNodeGraph)
        {
            isRegenComplete = true;
        }
    }
}

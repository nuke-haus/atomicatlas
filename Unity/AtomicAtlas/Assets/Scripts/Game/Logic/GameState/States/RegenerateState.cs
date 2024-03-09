
public class RegenerateState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.REGENERATE;
    public override GameStateType GetNextState() => GameStateType.GAMEPLAY;

    private ISettingsManager settingsManager;
    private bool isRegenComplete;

    public override bool CanExitState()
    {
        return isRegenComplete;
    }

    public override void OnEnter()
    {
        isRegenComplete = false;

        settingsManager = DependencyInjector.Resolve<ISettingsManager>();

        var strategy = settingsManager.ActiveStrategy;
        var definition = settingsManager.ActiveStrategyDefinition;

        settingsManager.ActiveStrategy.GenerateWorld(definition, settingsManager.AllPlayerInfo);

        isRegenComplete = true;
    }
}


public class RegenerateState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.REGENERATE;
    public override GameStateType GetNextState() => GameStateType.GAMEPLAY;

    private IDataManager dataManager;

    public override bool CanExitState()
    {
        return true;
    }

    public override void OnEnter()
    {
        dataManager = DependencyInjector.Resolve<IDataManager>();
    }
}

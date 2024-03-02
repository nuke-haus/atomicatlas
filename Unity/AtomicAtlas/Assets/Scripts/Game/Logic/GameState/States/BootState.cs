
public class BootState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.BOOT;

    public override GameStateType GetNextState()
    {
        return GameStateType.GAMEPLAY;
    }

    public override bool CanExitState()
    {
        return true;
    }

    public override void OnEnter()
    {
        
    }
}

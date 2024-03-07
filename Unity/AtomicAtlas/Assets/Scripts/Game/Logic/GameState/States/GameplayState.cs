using System;

// The default state for the mapgen, when it's not doing any heavy lifting
public class GameplayState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.GAMEPLAY;

    private GameStateType nextState = GameStateType.GAMEPLAY;
    private IDataManager dataManager;
    private IExportManager exportManager;

    public override GameStateType GetNextState()
    {
        return nextState;
    }

    public override void OnEnter()
    {
        nextState = GameStateType.GAMEPLAY;
        dataManager = DependencyInjector.Resolve<IDataManager>();
        exportManager = DependencyInjector.Resolve<IExportManager>();

        MainMenuManager.GlobalInstance.SetUIActive(true);
        MainMenuManager.GlobalInstance.OnClickQuitEvent += this.OnClickQuit;
        MainMenuManager.GlobalInstance.OnClickRegenerateEvent += this.OnClickRegenerate;
        MainMenuManager.GlobalInstance.OnClickExportEvent += this.OnClickExport;
    }

    public override void OnExit()
    {
        //MainMenuManager.GlobalInstance.SetMainMenuActive(false);
        MainMenuManager.GlobalInstance.OnClickQuitEvent -= this.OnClickQuit;
        MainMenuManager.GlobalInstance.OnClickRegenerateEvent -= this.OnClickRegenerate;
        MainMenuManager.GlobalInstance.OnClickExportEvent -= this.OnClickExport;
    }

    protected void OnClickQuit(object sender, EventArgs args)
    {
        nextState = GameStateType.QUIT;
    }

    protected void OnClickRegenerate(object sender, EventArgs args)
    {
        nextState = GameStateType.REGENERATE;
    }

    protected void OnClickExport(object sender, EventArgs args)
    {
        exportManager.ExportMap(); // TODO: logic for export
    }
}

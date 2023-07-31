using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.GAMEPLAY;

    private GameStateType nextState = GameStateType.GAMEPLAY;
    private IDataManager dataManager;

    public override GameStateType GetNextState()
    {
        return nextState;
    }

    public override void OnEnter()
    {
        dataManager = DependencyInjector.Resolve<IDataManager>();

        MainMenuManager.GlobalInstance.SetMainMenuActive(true);
        MainMenuManager.GlobalInstance.OnClickQuitEvent += this.OnClickQuit;
    }

    public override void OnExit()
    {
        MainMenuManager.GlobalInstance.SetMainMenuActive(false);
        MainMenuManager.GlobalInstance.OnClickQuitEvent -= this.OnClickQuit;
    }

    public void OnClickQuit(object sender, EventArgs args)
    {
        nextState = GameStateType.QUIT;
    }
}

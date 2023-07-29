using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MainMenuState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.MAIN_MENU;

    private GameStateType nextState = GameStateType.MAIN_MENU;

    public override GameStateType GetNextState()
    {
        return nextState;
    }

    public override void OnEnter()
    {
        Debug.Log("[MENU STATE] Main menu entered");

        MainMenuManager.GlobalInstance.SetMainMenuActive(true);
        MainMenuManager.GlobalInstance.OnClickStartEvent += this.OnClickStart;
        MainMenuManager.GlobalInstance.OnClickQuitEvent += this.OnClickQuit;
    }

    public override void OnExit()
    {
        MainMenuManager.GlobalInstance.SetMainMenuActive(false);
        MainMenuManager.GlobalInstance.OnClickStartEvent -= this.OnClickStart;
        MainMenuManager.GlobalInstance.OnClickQuitEvent -= this.OnClickQuit;
    }

    public void OnClickStart(object sender, EventArgs args)
    {
        nextState = GameStateType.GAMEPLAY;
    }

    public void OnClickQuit(object sender, EventArgs args)
    {
        nextState = GameStateType.QUIT;
    }
}

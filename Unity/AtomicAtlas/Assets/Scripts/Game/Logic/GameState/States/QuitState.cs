using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class QuitState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.QUIT;
    public override GameStateType GetNextState() => GameStateType.QUIT;

    public override void OnEnter()
    {
        Application.Quit();
    }
}

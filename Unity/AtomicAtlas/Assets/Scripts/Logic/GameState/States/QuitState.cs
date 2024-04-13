
using UnityEngine;
using Atlas.Core;

namespace Atlas.Logic
{
    public class QuitState : GameState<GameStateType>
    {
        public override GameStateType GameStateType => GameStateType.QUIT;
        public override GameStateType GetNextState() => GameStateType.QUIT;

        public override void OnEnter()
        {
            Application.Quit();
        }
    } 
}


using Atlas.Core;

namespace Atlas.Logic
{
    public interface IGameStateManager
    {
        void OnUpdate();
        void OnFixedUpdate();
    }

    [Injectable(typeof(IGameStateManager))]
    public class GameStateManager : GameStateMachine<GameStateType>, IGameStateManager
    {
        protected override GameStateType InitialState => GameStateType.BOOT;
    }

}
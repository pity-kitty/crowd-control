using UnityEngine;

namespace StateMachine
{
    public class GameStateContext : MonoBehaviour
    {
        private BaseGameState currentState;

        public MainMenuState MainMenuState = new();
        public RunningState RunningState = new();
        public FightingState FightingState = new();
    }
}
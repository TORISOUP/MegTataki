using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace MeguTataki.Scripts.Game.Managers
{
    public enum GameState
    {
        Ready,
        InGame,
        Result
    }

    public class GameStateManager : MonoBehaviour
    {
        private ReactiveProperty<GameState> _gameState = new ReactiveProperty<GameState>(GameState.Ready);
        public IReadOnlyReactiveProperty<GameState> State => _gameState;

        [Inject] private GameTimer _timer;
        
        private void Start()
        {
            _timer.Ready.Where(x => x == 0)
                .Take(1)
                .Subscribe(_ => _gameState.Value = GameState.InGame)
                .AddTo(this);

            _timer.Main.Where(x => x == 0)
                .Take(1)
                .Subscribe(_ => _gameState.Value = GameState.Result)
                .AddTo(this);
        }
    }
}
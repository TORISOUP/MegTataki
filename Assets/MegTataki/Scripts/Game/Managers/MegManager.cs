using System;
using System.Threading;
using UniRx;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;
using Zenject;

namespace MegTataki.Scripts.Game.Managers
{
    public class MegManager : MonoBehaviour
    {
        public IObservable<(int, int)> ShowMegMessage => _showMeg;

        //id,millsec
        private Subject<(int, int)> _showMeg = new Subject<(int, int)>();

        private Subject<Unit> _onClicked = new Subject<Unit>();
        public IObservable<Unit> OnClicked => _onClicked;

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GameTimer _gameTimer;
        private readonly CancellationTokenSource _inGameCancellationTokenSource = new CancellationTokenSource();
        private CancellationTokenSource _linkedCancellationTokenSource;
        private int _maxMegCount;

        public void Init(int maxMeg)
        {
            _maxMegCount = maxMeg;
        }

        private void Start()
        {
            _linkedCancellationTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(
                    _inGameCancellationTokenSource.Token,
                    this.GetCancellationTokenOnDestroy());

            _inGameCancellationTokenSource.AddTo(this);
            _linkedCancellationTokenSource.AddTo(this);

            _showMeg.AddTo(this);

            _gameStateManager.State.Subscribe(x =>
            {
                switch (x)
                {
                    case GameState.InGame:
                        MainLoopAsync(_linkedCancellationTokenSource.Token).Forget();
                        break;
                    case GameState.Result:
                        _inGameCancellationTokenSource.Cancel();
                        break;
                }
            }).AddTo(this);

            _onClicked.AddTo(this);
        }

        public void OnClickMeg()
        {
            _onClicked.OnNext(Unit.Default);
        }

        private async UniTaskVoid MainLoopAsync(CancellationToken token)
        {
            // 最初は少しずつでる
            while (!token.IsCancellationRequested && _gameTimer.Main.Value > 15)
            {
                var wait = UnityEngine.Random.Range(500, 1200);
                await UniTask.Delay(
                    wait,
                    false,
                    PlayerLoopTiming.Update,
                    token);

                _showMeg.OnNext(
                    (UnityEngine.Random.Range(0, _maxMegCount),
                        UnityEngine.Random.Range(750, 2000)));
            }

            // そのあとはたくさん
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(
                    UnityEngine.Random.Range(250, 800),
                    false,
                    PlayerLoopTiming.Update,
                    token);

                var max = UnityEngine.Random.Range(1, 4);
                for (int i = 0; i < max; i++)
                {
                    _showMeg.OnNext(
                        (UnityEngine.Random.Range(0, _maxMegCount),
                            UnityEngine.Random.Range(500, 1000)));
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
        }
    }
}
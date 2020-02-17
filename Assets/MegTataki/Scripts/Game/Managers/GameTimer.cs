using System.Threading;
using UniRx;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;

namespace MegTataki.Scripts.Game.Managers
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private IntReactiveProperty _ready = new IntReactiveProperty(3);

        [SerializeField] private IntReactiveProperty _main = new IntReactiveProperty(30);

        public IReadOnlyReactiveProperty<int> Ready => _ready;
        public IReadOnlyReactiveProperty<int> Main => _main;

        private void Start()
        {
            _ready.AddTo(this);
            _main.AddTo(this);
        }

        public async UniTaskVoid StartCountDownAsync()
        {
            var token = this.GetCancellationTokenOnDestroy();
            await ReadyAsync(token);
            await UniTask.Delay(1000, false, PlayerLoopTiming.Update, token);
            await MainAsync(token);
        }

        private async UniTask ReadyAsync(CancellationToken token)
        {
            while (_ready.Value > 0)
            {
                await UniTask.Delay(1000, cancellationToken: token);
                _ready.Value--;
            }
        }

        private async UniTask MainAsync(CancellationToken token)
        {
            _main.SetValueAndForceNotify(_main.Value);
            while (_main.Value > 0)
            {
                await UniTask.Delay(1000, cancellationToken: token);
                _main.Value--;
            }
        }
    }
}
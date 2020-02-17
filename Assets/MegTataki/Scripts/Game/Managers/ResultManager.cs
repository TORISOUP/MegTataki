using System.Threading;
using MegTataki.Scripts.Common;
using UniRx;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;
using Zenject;

namespace MegTataki.Scripts.Game.Managers
{
    public class ResultManager : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;

        [Inject] private ScoreManager _scoreManager;

        private StringReactiveProperty _resultMessage = new StringReactiveProperty();
        public IReadOnlyReactiveProperty<string> ResultMessage => _resultMessage;

        private void Start()
        {
            _gameStateManager.State
                .Where(x => x == GameState.Result)
                .Take(1)
                .Subscribe(_ => ResultAsync(this.GetCancellationTokenOnDestroy()).Forget())
                .AddTo(this);

            _resultMessage.AddTo(this);
        }

        private async UniTaskVoid ResultAsync(CancellationToken token)
        {
            _resultMessage.Value = "そこまで！";
            await UniTask.Delay(2000, false, PlayerLoopTiming.Update, token);

            _resultMessage.Value = $"スコア:{_scoreManager.Score}";

            await UniTask.Delay(3000, false, PlayerLoopTiming.Update, token);

            SceneLoader.LoadScene(GameScenes.Title.ToString(), _ => { });
        }
    }
}
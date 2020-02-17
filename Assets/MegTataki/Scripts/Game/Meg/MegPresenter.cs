using MegTataki.Scripts.Game.Managers;
using UniRx;
using UnityEngine;
using Zenject;

namespace MegTataki.Scripts.Game.Meg
{
    public class MegPresenter : MonoBehaviour
    {
        [Inject] private MegManager _megManager;
        [Inject] private GameStateManager _stateManager;

        private MegButton _megButton;

        private void Start()
        {
            _megButton = GetComponent<MegButton>();

            _stateManager.State.FirstOrDefault(x => x == GameState.Result)
                .Subscribe(_ => _megButton.Close()).AddTo(this);

            _megManager.ShowMegMessage
                .TakeUntil(_stateManager.State.FirstOrDefault(x => x == GameState.Result))
                .Where(x => x == _megButton.Id)
                .Subscribe(_ => _megButton.Open())
                .AddTo(this);

            _megButton.OnClickedAsObservable
                .TakeUntil(_stateManager.State.FirstOrDefault(x => x == GameState.Result))
                .Subscribe(_ => _megManager.OnClickMeg())
                .AddTo(this);
        }
    }
}
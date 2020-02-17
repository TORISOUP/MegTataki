using MegTataki.Scripts.Game.Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MegTataki.Scripts.Game.View
{
    public class TimePresenter : MonoBehaviour
    {
        [Inject] private GameTimer _timer;

        [SerializeField] private Text _readyText;
        [SerializeField] private Text _mainText;

        private void Start()
        {
            _mainText.text = "";

            _timer.Ready
                .Subscribe(x =>
                {
                    if (x == 0)
                    {
                        _readyText.text = "はじめ！";
                    }
                    else
                    {
                        _readyText.text = "よーーーい";
                    }
                }).AddTo(this);

            _timer.Main
                .SkipLatestValueOnSubscribe()
                .Take(1)
                .Subscribe(_ => _readyText.text = "")
                .AddTo(this);

            _timer.Main
                .SkipLatestValueOnSubscribe()
                .SubscribeToText(_mainText).AddTo(this);
        }
    }
}
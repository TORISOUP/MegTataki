using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace MeguTataki.Scripts.Game.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<int> Score => _score;
        private IntReactiveProperty _score = new IntReactiveProperty(0);

        [Inject] private MegManager _megManager;

        private void Start()
        {
            _score.AddTo(this);

            _megManager.OnClicked.Subscribe(_ => _score.Value++).AddTo(this);
        }
    }
}
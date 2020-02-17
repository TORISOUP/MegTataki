using System;
using MeguTataki.Scripts.Game.Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MeguTataki.Scripts.Game.View
{
    public class ScorePresenter : MonoBehaviour
    {
        [Inject] private ScoreManager _scoreManager;

        [SerializeField] private Text _text;

        private void Start()
        {
            _scoreManager.Score.SubscribeToText(_text).AddTo(this);
        }
    }
}
using MegTataki.Scripts.Game.Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MegTataki.Scripts.Game.View
{
    public class ResultPresenter : MonoBehaviour
    {
        [SerializeField] private Text _resultText;

        [Inject] private ResultManager _resultManager;


        private void Start()
        {
            _resultManager.ResultMessage.SubscribeToText(_resultText).AddTo(this);
        }
    }
}
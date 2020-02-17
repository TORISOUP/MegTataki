using MegTataki.Scripts.Common;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MegTataki.Scripts.Title
{
    public class TitlePresenter : MonoBehaviour
    {
        [Inject] private TitleManager _manager;

        [SerializeField] private GameLevel _level;

        private void Start()
        {
            var b = GetComponent<Button>();

            b.OnClickAsObservable()
                .Take(1)
                .Subscribe(_ => _manager.GoGame(_level))
                .AddTo(this);
        }
    }
}
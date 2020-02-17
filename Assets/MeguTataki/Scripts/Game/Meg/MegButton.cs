using System;
using System.Threading;
using UniRx;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace MeguTataki.Scripts.Game.Meg
{
    public class MegButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private Image _image;

        private Subject<Unit> _onClicked = new Subject<Unit>();
        public IObservable<Unit> OnClickedAsObservable => _onClicked;

        public int Id => _id;
        private int _id;

        private BoolReactiveProperty _isOpend = new BoolReactiveProperty(false);

        private void Start()
        {
            _isOpend.AddTo(this);

            _isOpend.Subscribe(x =>
            {
                _button.interactable = x;
                _image.gameObject.SetActive(x);
            });
        }

        public void Init(int id)
        {
            _id = id;
        }

        public void Open()
        {
            // すでに出ているなら何もしない
            if (_isOpend.Value) return;

            OpenAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void Close()
        {
            _isOpend.Value = false;
        }

        private async UniTaskVoid OpenAsync(CancellationToken token)
        {
            _isOpend.Value = true;

            var seconds = UnityEngine.Random.Range(1000, 4000);

            var isTimeout = await _button.OnClickAsync(token)
                .TimeoutWithoutException(TimeSpan.FromMilliseconds(seconds));


            if (!isTimeout) _onClicked.OnNext(Unit.Default);

            _isOpend.Value = false;
        }

        private void OnDestroy()
        {
            _onClicked.Dispose();
        }
    }
}
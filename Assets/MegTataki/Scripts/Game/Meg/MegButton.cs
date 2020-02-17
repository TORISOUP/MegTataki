using System;
using System.Threading;
using UniRx;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace MegTataki.Scripts.Game.Meg
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

        public void Open(int millsec)
        {
            // すでに出ているなら何もしない
            if (_isOpend.Value) return;

            OpenAsync(millsec,this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void Close()
        {
            _isOpend.Value = false;
        }

        private async UniTaskVoid OpenAsync(int millsec,CancellationToken token)
        {
            _isOpend.Value = true;


            var isTimeout = await _button.OnClickAsync(token)
                .TimeoutWithoutException(TimeSpan.FromMilliseconds(millsec));


            if (!isTimeout) _onClicked.OnNext(Unit.Default);

            _isOpend.Value = false;
        }

        private void OnDestroy()
        {
            _onClicked.Dispose();
        }
    }
}
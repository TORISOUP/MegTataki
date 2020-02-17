using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MeguTataki.Scripts.Common;
using MeguTataki.Scripts.Game.Meg;
using UnityEngine;
using Zenject;

namespace MeguTataki.Scripts.Game.Managers
{
    /// <summary>
    /// 初期化マン
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private Transform _stageArea;

        [SerializeField] private GameObject _varticalLinePrefab;

        [SerializeField] private GameObject _megButtonPrefab;

        [InjectOptional] private GameInit _gameInit;

        [Inject] private GameTimer _gameTimer;
        [Inject] private MegManager _megManager;

        private void Start()
        {
            var size = 2;
            switch (_gameInit.Level)
            {
                case GameLevel.Easy:
                    size = 2;
                    break;
                case GameLevel.Normal:
                    size = 3;
                    break;
                case GameLevel.Hard:
                    size = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            int count = 0;
            for (int i = 0; i < size; i++)
            {
                var v = Instantiate(_varticalLinePrefab, Vector3.zero, Quaternion.identity, _stageArea);

                for (int j = 0; j < size; j++)
                {
                    var m = Instantiate(_megButtonPrefab, Vector3.zero, Quaternion.identity, v.transform);
                    var c = m.GetComponent<MegButton>();
                    c.Init(count++);
                }
            }
            
            _megManager.Init(size * size);
            _gameTimer.StartCountDownAsync().Forget();
        }
    }
}
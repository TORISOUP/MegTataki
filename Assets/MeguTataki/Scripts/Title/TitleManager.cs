using MeguTataki.Scripts.Common;
using UnityEngine;

namespace MeguTataki.Scripts.Title
{
    public class TitleManager : MonoBehaviour
    {
        public void GoGame(GameLevel level)
        {
            SceneLoader.LoadScene(GameScenes.Game.ToString(),
                container =>
                {
                    container.Bind<GameInit>().FromInstance(new GameInit(level)).AsCached();
                });
        }
    }
}
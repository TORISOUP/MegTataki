namespace MeguTataki.Scripts.Common
{
    public struct GameInit
    {
        public GameLevel Level { get; }

        public GameInit(GameLevel level)
        {
            Level = level;
        }
    }

    public enum GameLevel
    {
        Easy,
        Normal,
        Hard
    }
}
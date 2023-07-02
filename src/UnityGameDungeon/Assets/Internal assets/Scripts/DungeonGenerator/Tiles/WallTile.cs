#region

using DungeonGenerator.Tiles.Interface;

#endregion

namespace DungeonGenerator.Tiles
{
    public class WallTile : ITile
    {
        public bool IsLight;
        public DirectionType LightDirectionType;

        public enum DirectionType
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3
        }
    }
}

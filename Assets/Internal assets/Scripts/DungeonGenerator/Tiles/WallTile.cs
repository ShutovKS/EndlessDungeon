using DungeonGenerator.Tiles.Interface;

namespace DungeonGenerator.Tiles
{
    public class WallTile : ITile
    {
        public DungeonTilesType Type { get; set; } = DungeonTilesType.WALL;
    
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


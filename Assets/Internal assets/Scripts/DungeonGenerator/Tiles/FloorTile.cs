using DungeonGenerator.Tiles.Interface;

namespace DungeonGenerator.Tiles
{
    public class FloorTile : ITile
    {
        public DungeonTilesType Type { get; set; } = DungeonTilesType.FLOOR;
        public bool IsOccupied;
    }
}
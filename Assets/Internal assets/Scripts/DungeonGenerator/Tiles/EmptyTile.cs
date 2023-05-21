using DungeonGenerator.Tiles.Interface;

namespace DungeonGenerator.Tiles
{
    public class EmptyTile : ITile
    {
        public DungeonTilesType Type { get; set; } = DungeonTilesType.EMPTY;
    }
}
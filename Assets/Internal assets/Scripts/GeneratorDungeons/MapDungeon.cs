using System.Collections.Generic;

namespace GeneratorDungeons
{
    public struct MapDungeon
    {
        public Tiles[,] TilesMap;
        /// <summary>
        /// X Y
        /// </summary>
        public List<(int, int)> EnemiesPosition;
        public List<Room> Rooms;
        public Room StartRoom;

        public readonly int Seed;
        public readonly int MinRoomSize;
        public readonly int MaxRoomSize;
        public readonly int MinRoomCount;
        public readonly int MaxRoomCount;
        public readonly int Width;
        public readonly int Height;

        // public MapDungeon()
        // {
        //     Seed = 0;
        //     MinRoomSize = 0;
        //     MaxRoomSize = 0;
        //     MinRoomCount = 0;
        //     MaxRoomCount = 0;
        //     Width = 0;
        //     Height = 0;
        //
        //     TilesMap = new Tiles[Height, Width];
        //     EnemiesPosition = new List<(int, int)>();
        //     Rooms = new List<Room>();
        //     StartRoom = new Room();
        // }

        public MapDungeon(
            int seed,
            int minRoomSize, int maxRoomSize,
            int minRoomCount, int maxRoomCount,
            int width, int height)
            : this()
        {
            Seed = seed;
            MinRoomSize = minRoomSize;
            MaxRoomSize = maxRoomSize;
            MinRoomCount = minRoomCount;
            MaxRoomCount = maxRoomCount;
            Width = width;
            Height = height;

            TilesMap = new Tiles[Height, Width];
        }

        public MapDungeon(MapDungeon mapDungeon) : this()
        {
            this = mapDungeon;
        }

        public MapDungeon(Tiles[,] tilesMap, MapDungeon mapDungeon) : this(mapDungeon)
        {
            TilesMap = tilesMap;
        }

        public MapDungeon(Tiles[,] tilesMap, List<Room> rooms, Room startRoom,
            int seed,
            int minRoomSize, int maxRoomSize,
            int minRoomCount, int maxRoomCount,
            int width, int height) :
            this(seed, minRoomSize, maxRoomSize, minRoomCount, maxRoomCount, width, height)
        {
            TilesMap = tilesMap;
            Rooms = rooms;
            StartRoom = startRoom;
        }

        public MapDungeon(Tiles[,] tilesMap, List<Room> rooms, Room startRoom, MapDungeon mapDungeon) : this(mapDungeon)
        {
            TilesMap = tilesMap;
            Rooms = rooms;
            StartRoom = startRoom;
        }

        public MapDungeon(List<(int, int)> enemiesPosition, MapDungeon mapDungeon) : this(mapDungeon)
        {
            EnemiesPosition = enemiesPosition;
        }

        public MapDungeon(
            Tiles[,] tilesMap, List<Room> rooms, Room startRoom, List<(int, int)> enemiesPosition,
            int seed,
            int minRoomSize, int maxRoomSize,
            int minRoomCount, int maxRoomCount,
            int width, int height)
        {
            TilesMap = tilesMap;
            Rooms = rooms;
            StartRoom = startRoom;
            EnemiesPosition = enemiesPosition;

            Seed = seed;
            MinRoomSize = minRoomSize;
            MaxRoomSize = maxRoomSize;
            MinRoomCount = minRoomCount;
            MaxRoomCount = maxRoomCount;
            Width = width;
            Height = height;
        }
    }
}
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace GeneratorDungeons
{
    public class TileDungeon
    {
        public MapTile[,] TilesMap { get; private set; } = null!;
        public readonly List<(int, int)> EnemyPosition = new();
        private Room Start { get; set; }
        private (int, int) _startPosition;
        private int _seed;
        private readonly int _minRoomSize;
        private readonly int _maxRoomSize;
        private readonly int _minRoomCount;
        private readonly int _maxRoomCount;
        private int Width { get; }
        private int Height { get; }

        public TileDungeon(
            int seed,
            int width,
            int height,
            int minRoomCount,
            int maxRoomCount,
            int minRoomSize,
            int maxRoomSize)
        {
            _seed = seed;
            Width = width;
            Height = height;
            _minRoomSize = minRoomSize;
            _maxRoomSize = maxRoomSize;
            _minRoomCount = minRoomCount;
            _maxRoomCount = maxRoomCount;
        }

        public void Generation()
        {
            FillInWalls();

            var rooms = AddRooms();

            AddCorridors(rooms);
            AddEntry(rooms);
            DetermineWallTypes();
            GenerationRandomEnemy(rooms);
        }

        #region Generation Map

        private void FillInWalls()
        {
            TilesMap = new MapTile[Width, Height];
            for (var x = 0; x != Width; ++x)
            for (var y = 0; y != Height; ++y)
                TilesMap[x, y] = MapTile.EMPTY;
        }

        private List<Room> AddRooms()
        {
            var roomCenters = new List<Room>();
            var roomCount = RandomRange(_minRoomCount, _maxRoomCount);
            for (var i = 0; i != roomCount; ++i)
            {
                var room = new Room
                {
                    x = RandomRange(_maxRoomSize, Width - _maxRoomSize),
                    y = RandomRange(_maxRoomSize, Height - _maxRoomSize),
                    width = RandomRange(_minRoomSize, _maxRoomSize),
                    height = RandomRange(_minRoomSize, _maxRoomSize)
                };

                CarveOpen(
                    room.x - room.width / 2,
                    room.y - room.height / 2,
                    room.width,
                    room.height);

                roomCenters.Add(room);
            }

            return roomCenters;
        }

        private void AddCorridors(IReadOnlyList<Room> rooms)
        {
            for (var i = 0; i != rooms.Count - 1; ++i)
            {
                CarveOpen(
                    Math.Min(rooms[i].x, rooms[i + 1].x),
                    rooms[i].y,
                    1 + Math.Abs(rooms[i + 1].x - rooms[i].x),
                    1);

                CarveOpen(
                    rooms[i + 1].x,
                    Math.Min(rooms[i].y, rooms[i + 1].y),
                    1,
                    1 + Math.Abs(rooms[i + 1].y - rooms[i].y));
            }
        }

        private void AddEntry(List<Room> rooms)
        {
            var distHi = 0;
            var startIdx = -1;
            for (var i = 0; i != rooms.Count; ++i)
            for (var j = 0; j != rooms.Count; ++j)
            {
                var dist = Math.Abs(rooms[i].x - rooms[j].x) + Math.Abs(rooms[i].y - rooms[j].y);
                if (dist <= distHi) continue;
                distHi = dist;
                startIdx = i;
            }


            Start = rooms[startIdx];
            TilesMap[Start.x, Start.y] = MapTile.ENTRY;
            _startPosition = (Start.x, Start.y);
            rooms.Remove(Start);
        }

        private void DetermineWallTypes()
        {
            for (var x = 0; x != Width; ++x)
            for (var y = 0; y != Height; ++y)
                if (TilesMap[x, y] == MapTile.FLOOR)
                    for (var dx = -1; dx <= 1; ++dx)
                    for (var dy = -1; dy <= 1; ++dy)
                        if (TilesMap[x + dx, y + dy] == MapTile.EMPTY)
                            TilesMap[x + dx, y + dy] = MapTile.WALL;
        }

        private void CarveOpen(int x, int y, int width, int height)
        {
            if (width < 1 || height < 1)
            {
                return;
            }

            for (var dx = x; dx != x + width; ++dx)
            for (var dy = y; dy != y + height; ++dy)
                TilesMap[dx, dy] = MapTile.FLOOR;
        }

        #endregion

        #region Generation Enemies

        private void GenerationRandomEnemy(IEnumerable<Room> rooms)
        {
            foreach (var room in rooms)
            {
                var enemiesCountInThisRoom = room.height * room.width / 20;
                enemiesCountInThisRoom = enemiesCountInThisRoom > 0 ? enemiesCountInThisRoom : 1;
                for (var i = 0; i < enemiesCountInThisRoom; i++)
                {
                    while (true)
                    {
                        var position = (Random.Range(room.x, room.width), Random.Range(room.y, room.height));
                        if (EnemyPosition.Contains(position) ||
                            _startPosition.Item1 - 7 > room.x &&
                            _startPosition.Item1 + 7 < room.x &&
                            _startPosition.Item2 - 7 > room.y &&
                            _startPosition.Item2 + 7 < room.y)
                            continue;

                        EnemyPosition.Add(position);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Tools

        private static int IntHash(ref int x)
        {
            if (x == 0)
            {
                x = 1;
            }

            x = ((x >> 16) ^ x) * 0x45d9f3b;
            x = ((x >> 16) ^ x) * 0x45d9f3b;
            x = (x >> 16) ^ x;
            return x;
        }

        private int RandomRange(int min, int max)
        {
            var range = IntHash(ref _seed) % ((max + 1) - min);
            return min + range;
        }

        #endregion
    }
}
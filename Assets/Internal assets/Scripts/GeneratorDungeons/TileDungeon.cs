using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneratorDungeons
{
    public class TileDungeon
    {
        public enum Tile
        {
            FLOOR,
            INTERIOR_WALL,
            EXTERIOR_WALL,
            ENTRY,
            EXIT
        }

        private Tile[,] _tiles = null!;
        private int[] Start { get; set; } = null!;
        private int[] Exit { get; set; } = null!;

        private int _seed;
        private readonly int _width;
        private readonly int _height;
        private readonly int _minRoomSize;
        private readonly int _maxRoomSize;
        private readonly int _minRoomCount;
        private readonly int _maxRoomCount;

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
            _width = width;
            _height = height;
            _minRoomSize = minRoomSize;
            _maxRoomSize = maxRoomSize;
            _minRoomCount = minRoomCount;
            _maxRoomCount = maxRoomCount;
        }

        public Task<Tile[,]> GetDungeon()
        {
            FillInWalls();
            var rooms = AddRooms();

            AddCorridors(rooms);
            AddEntryExit(rooms);
            DetermineWallTypes();

            return Task.FromResult(_tiles);
        }

        private void FillInWalls()
        {
            _tiles = new Tile[_width, _height];

            for (var x = 0; x != _width; ++x)
            {
                for (var y = 0; y != _height; ++y)
                {
                    _tiles[x, y] = Tile.EXTERIOR_WALL;
                }
            }
        }

        private List<int[]> AddRooms()
        {
            var roomCenters = new List<int[]>();

            var roomCount = RandomRange(_minRoomCount, _maxRoomCount);
            for (var i = 0; i != roomCount; ++i)
            {
                var x = RandomRange(_maxRoomSize, _width - _maxRoomSize);
                var y = RandomRange(_maxRoomSize, _height - _maxRoomSize);
                var w = RandomRange(_minRoomSize, _maxRoomSize);
                var h = RandomRange(_minRoomSize, _maxRoomSize);
                CarveOpen(x - w / 2, y - h / 2, w, h);
                roomCenters.Add(new[] { x, y });
            }

            return roomCenters;
        }

        private void AddCorridors(IReadOnlyList<int[]> centers)
        {
            for (var i = 0; i != centers.Count - 1; ++i)
            {
                CarveOpen(Math.Min(centers[i][0], centers[i + 1][0]), centers[i][1],
                    1 + Math.Abs(centers[i + 1][0] - centers[i][0]), 1);
                CarveOpen(centers[i + 1][0], Math.Min(centers[i][1], centers[i + 1][1]), 1,
                    1 + Math.Abs(centers[i + 1][1] - centers[i][1]));
            }
        }

        private void AddEntryExit(IReadOnlyList<int[]> centers)
        {
            var distHi = 0;
            var startIdx = -1;
            var endIdx = -1;

            for (var i = 0; i != centers.Count; ++i)
            {
                for (var j = 0; j != centers.Count; ++j)
                {
                    var dist = Math.Abs(centers[i][0] - centers[j][0]) + Math.Abs(centers[i][1] - centers[j][1]);

                    if (dist > distHi)
                    {
                        distHi = dist;
                        startIdx = i;
                        endIdx = j;
                    }
                }
            }

            Start = centers[startIdx];
            Exit = centers[endIdx];

            _tiles[Start[0], Start[1]] = Tile.ENTRY;
            _tiles[Exit[0], Exit[1]] = Tile.EXIT;
        }

        private void DetermineWallTypes()
        {
            for (var x = 0; x != _width; ++x)
            {
                for (var y = 0; y != _height; ++y)
                {
                    if (_tiles[x, y] != Tile.FLOOR)
                    {
                        continue;
                    }

                    for (var dx = -1; dx <= 1; ++dx)
                    {
                        for (var dy = -1; dy <= 1; ++dy)
                        {
                            // If there's an exterior wall, mark it as interior, instead.
                            if (_tiles[x + dx, y + dy] == Tile.EXTERIOR_WALL)
                            {
                                _tiles[x + dx, y + dy] = Tile.INTERIOR_WALL;
                            }
                        }
                    }
                }
            }
        }

        private void CarveOpen(int x, int y, int width, int height)
        {
            if (width < 1 || height < 1)
            {
                return;
            }

            for (var dx = x; dx != x + width; ++dx)
            {
                for (var dy = y; dy != y + height; ++dy)
                {
                    _tiles[dx, dy] = Tile.FLOOR;
                }
            }
        }

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
    }
}
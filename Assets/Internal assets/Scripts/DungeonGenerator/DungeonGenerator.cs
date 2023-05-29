#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DungeonGenerator.Tiles;
using DungeonGenerator.Tiles.Interface;
using Random = UnityEngine.Random;

#endregion

namespace DungeonGenerator
{
    public static class DungeonGenerator
    {
        public static async Task<(ITile[,] dungeonMap, (int x, int y) playerPosition, List<(int x, int y)>
                enemiesPosition)>
            GetDungeon(int seed)
        {
            const int minRoomSize = 4;
            const int maxRoomSize = 8;
            const int minRoomCount = 6;
            const int maxRoomCount = 11;
            const int width = 45;
            const int height = 45;

            var map = await Generation(
                seed,
                minRoomSize,
                maxRoomSize,
                minRoomCount,
                maxRoomCount,
                width,
                height);

            return map;
        }

        #region Generation Enemy

        private static List<(int x, int y)> GetEnemiesPosition(
            List<DungeonRoomCharacteristic> rooms, DungeonRoomCharacteristic startDungeonRoomCharacteristic,
            ITile[,] dungeonMap, int height, int width)
        {
            var enemyPosition = new List<(int x, int y)>();
            var tilesMap = (ITile[,])dungeonMap.Clone();


            for (var y = startDungeonRoomCharacteristic.Y - startDungeonRoomCharacteristic.Height / 2 - 5;
                 y < startDungeonRoomCharacteristic.Y - startDungeonRoomCharacteristic.Height / 2 + 7;
                 y++)
            {
                if (y < 0 || y > height - 1) continue;
                for (var x = startDungeonRoomCharacteristic.X - startDungeonRoomCharacteristic.Width / 2 - 5;
                     x < startDungeonRoomCharacteristic.X - startDungeonRoomCharacteristic.Width / 2 + 7;
                     x++)
                {
                    if (x < 0 || x > width - 1) continue;
                    if (tilesMap[y, x] is FloorTile)
                        tilesMap[y, x] = new EmptyTile();
                }
            }

            foreach (var room in rooms)
            {
                for (var i = 0; i < (room.Height * room.Width / 20 == 0 ? 1 : room.Height * room.Width / 20); i++)
                for (var flag = 0; flag < 25; flag++)
                {
                    (int x, int y) position = (
                        Random.Range(room.X - room.Width / 2, room.X + room.Width / 2),
                        Random.Range(room.Y - room.Height / 2, room.Y + room.Height / 2));

                    if (tilesMap[position.x, position.y] is not FloorTile) continue;

                    enemyPosition.Add(position);
                    break;
                }
            }

            return enemyPosition;
        }

        #endregion

        #region Generation Map

        private static Task<(ITile[,] tilesMap, (int x, int y), List<(int, int)> enemyPosition)> Generation(
            int seed,
            int minRoomSize,
            int maxRoomSize,
            int minRoomCount,
            int maxRoomCount,
            int width,
            int height)
        {
            var tilesMap = FillInWalls(height, width);

            AddRooms(
                ref tilesMap,
                out var rooms,
                seed,
                minRoomSize,
                maxRoomSize,
                minRoomCount,
                maxRoomCount,
                width,
                height);

            AddCorridors(ref tilesMap, ref rooms);
            AddPlayer(ref rooms, out var startRoom);
            AddWalls(height, width, ref tilesMap);
            AddedLight(height, width, ref tilesMap);
            var enemyPosition = GetEnemiesPosition(rooms, startRoom, tilesMap, height, width);

            return Task.FromResult((tilesMap, (x: startRoom.X, y: startRoom.Y), enemyPosition));
        }

        private static ITile[,] FillInWalls(int height, int width)
        {
            var tilesMap = new ITile[height, width];
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                tilesMap[y, x] = new EmptyTile();
            }

            return tilesMap;
        }

        private static void AddRooms(ref ITile[,] tilesMap, out List<DungeonRoomCharacteristic> rooms,
            int seed, int minRoomSize,
            int maxRoomSize, int minRoomCount,
            int maxRoomCount, int width, int height)
        {
            rooms = new List<DungeonRoomCharacteristic>();
            var roomCount = RandomRange(ref seed, minRoomCount, maxRoomCount);
            for (var i = 0; i != roomCount; ++i)
            {
                var room = new DungeonRoomCharacteristic
                {
                    X = RandomRange(ref seed, maxRoomSize, width - maxRoomSize),
                    Y = RandomRange(ref seed, maxRoomSize, height - maxRoomSize),
                    Width = RandomRange(ref seed, minRoomSize, maxRoomSize),
                    Height = RandomRange(ref seed, minRoomSize, maxRoomSize)
                };

                CarveFloor(
                    ref tilesMap,
                    room.Y - room.Height / 2,
                    room.X - room.Width / 2,
                    room.Height,
                    room.Width);

                rooms.Add(room);
            }
        }


        private static void AddCorridors(ref ITile[,] tilesMap, ref List<DungeonRoomCharacteristic> rooms)
        {
            for (var i = 0; i != rooms.Count - 1; ++i)
            {
                CarveFloor(
                    ref tilesMap,
                    rooms[i].Y,
                    Math.Min(rooms[i].X, rooms[i + 1].X),
                    2,
                    1 + Math.Abs(rooms[i + 1].X - rooms[i].X));

                CarveFloor(
                    ref tilesMap,
                    Math.Min(rooms[i].Y, rooms[i + 1].Y),
                    rooms[i + 1].X,
                    1 + Math.Abs(rooms[i + 1].Y - rooms[i].Y),
                    2);
            }
        }

        //TODO: переделать, сделать поиск наименьшей комнаты, не пересекающейся с другими
        private static void AddPlayer(ref List<DungeonRoomCharacteristic> rooms,
            out DungeonRoomCharacteristic startDungeonRoomCharacteristic)
        {
            var distHi = 0;
            var startIdx = -1;
            for (var i = 0; i != rooms.Count; ++i)
            for (var j = 0; j != rooms.Count; ++j)
            {
                var dist = Math.Abs(rooms[i].X - rooms[j].X) + Math.Abs(rooms[i].Y - rooms[j].Y);
                if (dist <= distHi) continue;
                distHi = dist;
                startIdx = i;
            }

            startDungeonRoomCharacteristic = rooms[startIdx];
            rooms.Remove(startDungeonRoomCharacteristic);
        }

        private static void AddWalls(int height, int width, ref ITile[,] tilesMap)
        {
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                if (tilesMap[y, x] is FloorTile)
                {
                    for (var dy = -1; dy <= 1; ++dy)
                    for (var dx = -1; dx <= 1; ++dx)
                        if (tilesMap[y + dy, x + dx] is EmptyTile)
                            tilesMap[y + dy, x + dx] = new WallTile();
                }
        }

        private static void AddedLight(int height, int width, ref ITile[,] tilesMap)
        {
            for (var y = 2; y < height - 2; y++)
            for (var x = 2; x < width - 2; x++)
            {
                if (tilesMap[y, x] is not WallTile tile) continue;

                var isLightVisible = false;

                for (var dy = -2; dy <= 2; dy++)
                    if (tilesMap[y + dy, x] is WallTile { IsLight: true })
                    {
                        isLightVisible = true;
                        break;
                    }

                if (isLightVisible) continue;

                for (var dx = -2; dx <= 2; dx++)
                    if (tilesMap[y, x + dx] is WallTile { IsLight: true })
                    {
                        isLightVisible = true;
                        break;
                    }

                if (isLightVisible) continue;

                tile.IsLight = true;
                if (tilesMap[y + 1, x] is FloorTile)
                {
                    tile.LightDirectionType = WallTile.DirectionType.Up;
                }
                else if (tilesMap[y - 1, x] is FloorTile)
                {
                    tile.LightDirectionType = WallTile.DirectionType.Down;
                }
                else if (tilesMap[y, x + 1] is FloorTile)
                {
                    tile.LightDirectionType = WallTile.DirectionType.Right;
                }
                else if (tilesMap[y, x - 1] is FloorTile)
                {
                    tile.LightDirectionType = WallTile.DirectionType.Left;
                }
                else
                {
                    tile.IsLight = false;
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

            x = (x >> 16 ^ x) * 0x45d9f3b;
            x = (x >> 16 ^ x) * 0x45d9f3b;
            x = x >> 16 ^ x;
            return x;
        }

        private static int RandomRange(ref int seed, int min, int max)
        {
            var range = IntHash(ref seed) % (max + 1 - min);
            return min + range;
        }

        private static void CarveFloor(ref ITile[,] tilesMap, int y, int x, int height, int width)
        {
            if (width < 1 || height < 1)
            {
                return;
            }

            for (var dy = y; dy < y + height; dy++)
            for (var dx = x; dx < x + width; dx++)
                tilesMap[dy, dx] = new FloorTile();
        }

        #endregion
    }
}
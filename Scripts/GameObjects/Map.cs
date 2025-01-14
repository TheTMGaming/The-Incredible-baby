﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Controllers
{
    class TileData
    {
        public Point Point;
        public int Level;

        public TileData(Point point, int level)
        {
            Point = point;
            Level = level;
        }
    }

    class Map
    {
        public readonly GameObject[,] Tiles;
        public readonly List<GameObject> FreeTiles;

        public readonly int Width;
        public readonly int Height;

        private readonly Random randGenerator = new Random();

        private readonly int maxCountBoxStack = 2;
        private readonly int sizeBossZone = 3;
        private readonly int sizeViewedZoneTile = 3;
        private readonly float initialProbabilitySpawnBox = .8f;
        private readonly float increasingProbabilityLevels = .012f;

        public Map()
        {
            Width = GameSettings.MapWidth / GameSettings.TileSize;
            Height = GameSettings.MapHeight / GameSettings.TileSize;

            Tiles = new GameObject[Width, Height];
            FreeTiles = new List<GameObject>();

            CreateBlocks();
            CreateMap();
        }

        public GameObject GetTileIn(int x, int y) => Tiles[x / GameSettings.TileSize, y / GameSettings.TileSize];

        private void CreateMap()
        {
            var rand = new Random();

            var visited = new HashSet<Point>();
            var queue = new Queue<TileData>();

            queue.Enqueue(new TileData(new Point(Width / 2, Height / 2), 0));

            while (queue.Count > 0)
            {
                var tile = queue.Dequeue();
                visited.Add(tile.Point);

                var zone = GetTileZone(tile.Point);

                if (zone.Count(a => Tiles[a.X, a.Y] is Box) < maxCountBoxStack
                    && tile.Level > sizeBossZone
                    && randGenerator.NextDouble() > initialProbabilitySpawnBox + (tile.Level - sizeBossZone) * increasingProbabilityLevels
                    && !new Rectangle(tile.Point.X * GameSettings.TileSize, 
                        tile.Point.Y * GameSettings.TileSize, 
                        GameSettings.TileSize, GameSettings.TileSize).IntersectsWith(GameModel.Player.Collider.Transform))
                {
                    var box = new Box(tile.Point.X * GameSettings.TileSize + GameSettings.TileSize / 2,
                        tile.Point.Y * GameSettings.TileSize + GameSettings.TileSize / 2);

                    Tiles[tile.Point.X, tile.Point.Y] = box;
                    GameRender.AddRenderFor(box);
                    Physics.AddToTrackingColliders(box.Collider);
                    Physics.AddToTrackingHitBoxes(box.Collider);
                }
                else
                {                  
                    Tiles[tile.Point.X, tile.Point.Y] = new Grass(tile.Point.X * GameSettings.TileSize + GameSettings.TileSize / 2, 
                        tile.Point.Y * GameSettings.TileSize + GameSettings.TileSize / 2);

                    FreeTiles.Add(Tiles[tile.Point.X, tile.Point.Y]);

                    GameRender.AddRenderFor(Tiles[tile.Point.X, tile.Point.Y]);
                }

                foreach (var neighbour in GetNeighbors(tile.Point, zone, visited))
                {                  
                    queue.Enqueue(new TileData(neighbour, tile.Level + 1));
                    visited.Add(neighbour);                   
                }
            }
        }

        private void CreateBlocks()
        {
            foreach (var x in Enumerable.Range(0, Width))
            {
                Tiles[x, 0] = new Block(x * GameSettings.TileSize + GameSettings.TileSize / 2, GameSettings.TileSize / 2);
                Tiles[x, Height - 1] = new Block(x * GameSettings.TileSize + GameSettings.TileSize / 2,
                    (Height - 1) * GameSettings.TileSize + GameSettings.TileSize / 2);

                Physics.AddToTrackingColliders(Tiles[x, 0].Collider);
                Physics.AddToTrackingColliders(Tiles[x, Height - 1].Collider);
                Physics.AddToTrackingHitBoxes(Tiles[x, 0].Collider);
                Physics.AddToTrackingHitBoxes(Tiles[x, Height - 1].Collider);

                GameRender.AddRenderFor(Tiles[x, 0]);
                GameRender.AddRenderFor(Tiles[x, Height - 1]);
            }

            foreach (var y in Enumerable.Range(1, Height - 1))
            {
                Tiles[0, y] = new Block(GameSettings.TileSize / 2, y * GameSettings.TileSize + GameSettings.TileSize / 2);
                Tiles[Width - 1, y] = new Block((Width - 1) * GameSettings.TileSize + GameSettings.TileSize / 2,
                    y * GameSettings.TileSize + GameSettings.TileSize / 2);

                Physics.AddToTrackingColliders(Tiles[0, y].Collider);
                Physics.AddToTrackingColliders(Tiles[Width - 1, y].Collider);
                Physics.AddToTrackingHitBoxes(Tiles[0, y].Collider);
                Physics.AddToTrackingHitBoxes(Tiles[Width - 1, y].Collider);

                GameRender.AddRenderFor(Tiles[0, y]);
                GameRender.AddRenderFor(Tiles[Width - 1, y]);
            }
        }

        private IEnumerable<Point> GetTileZone(Point point)
        {
            return Enumerable
                .Range(-sizeViewedZoneTile, 1 + sizeViewedZoneTile * 2)
                .SelectMany(dx => Enumerable.Range(-sizeViewedZoneTile, 1 + sizeViewedZoneTile * 2),
                            (dx, dy) => new Point(point.X + dx, point.Y + dy))
                .Where(p => p.X > 0 && p.X < Width - 1 && p.Y > 0 && p.Y < Height - 1 && p != point);
        }


        private IEnumerable<Point> GetNeighbors(Point point, IEnumerable<Point> tileZone, HashSet<Point> visited)
        {
            return tileZone
                .Where(p => Math.Abs(p.X - point.X) < 2 && Math.Abs(p.Y - point.Y) < 2
                    && !visited.Contains(p));
        }

    }
}

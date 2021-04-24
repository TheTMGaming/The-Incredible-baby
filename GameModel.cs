﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.UI;


namespace Top_Down_shooter
{
    class GameModel
    {
        public readonly Player Player;
        public readonly HealthBar HealthBar;
        public readonly LinkedList<Bullet> Bullets;
        public readonly TileMapController Map;

        public GameModel()
        {
            Player = new Player(100, 100, 5);
            HealthBar = new HealthBar(100);

            Bullets = new LinkedList<Bullet>();
            Map = new TileMapController(20, 20);
        }

        public void Shoot()
        {
            var newSpawn = RotatePoint(Player.Gun.SpawnBullets, Player.Gun.Angle);

            Bullets.AddLast(new Bullet(
                Player.Gun.X + newSpawn.X, Player.Gun.Y + newSpawn.Y, 
                20, Player.Gun.Angle));
        }

        private Point RotatePoint(Point point, float angleInRadian)
        {
            return new Point(
                (int)(point.X * Math.Cos(angleInRadian) - point.Y * Math.Sin(angleInRadian)),
                (int)(point.Y * Math.Cos(angleInRadian) + point.X * Math.Sin(angleInRadian))
                );
        }
    }
}

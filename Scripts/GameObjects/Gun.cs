﻿using System.Drawing;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Gun : GameObject
    {
        public float Angle { get; set; }

        public readonly Point SpawnBullets = new Point(10, -10);

        public Gun(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

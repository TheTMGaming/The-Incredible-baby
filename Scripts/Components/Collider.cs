﻿using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Components
{
    class Collider
    {
        public int X => parent.X + localX;
        public int Y => parent.Y + localY;
        public Rectangle Transform => new Rectangle(X - Width / 2, Y - Height / 2, Width, Height);
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsTrigger { get; set; }

        private readonly GameObject parent;
        private readonly int localX;
        private readonly int localY;

        public Collider(GameObject parent, int localX, int localY, int width, int height)
        {
            this.parent = parent;
            this.localX = localX;
            this.localY = localY;
            Width = width;
            Height = height;
        }

        public bool IntersectsWith(Collider other) => Transform.IntersectsWith(other.Transform);
    }
}

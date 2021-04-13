﻿using System.Drawing;

namespace Top_Down_shooter
{
    class HealthBar
    {
        public int Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                if (percent < 0) percent = 0;
                if (percent > 100) percent = 100;
            }
        }

        private int percent;

        private readonly Sprite background;
        private readonly Sprite bar;
        private readonly Sprite heart;

        private readonly Point offsetBackground = new Point(45, 45);
        private readonly Point offsetBarRelativeBack = new Point(2, 3);

        public HealthBar(int xLeftPoint, int yTopPoint, int percent)
        {
            Percent = percent;
            
            heart = new Sprite
            {
                X = xLeftPoint,
                Y = yTopPoint,
                Image = new Bitmap(@"Sprites/Heart.png")
            };

            background = new Sprite
            {
                X = xLeftPoint + offsetBackground.X,
                Y = yTopPoint + offsetBackground.Y,
                Image = new Bitmap(@"Sprites/BackgroundHealthBar.png")
            };

            bar = new Sprite
            {
                X = xLeftPoint + offsetBackground.X + offsetBarRelativeBack.X,
                Y = yTopPoint + offsetBackground.Y + offsetBarRelativeBack.Y,
                Image = new Bitmap(@"Sprites/HealthBar.png")
            };
        }

        public void Draw(Graphics g)
        {
            background.Draw(g);

            bar.Draw(g, new Point(0, 0), new Size(bar.Image.Width * Percent / 100, bar.Image.Height));

            heart.Draw(g);
        }
        
    }
}

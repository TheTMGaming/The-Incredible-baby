﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Top_Down_shooter
{
    enum AnimationTypes
    {
        IdleRight,
        IdleLeft,
        RunRight,
        RunLeft
    }

    public class Form1 : Form
    {
        private GameModel gameModel;
        private int currentFrameAnimation;

        public Form1()
        {
            DoubleBuffered = true;

            gameModel = new GameModel();

            var timerGameLoop = new Timer();
            timerGameLoop.Interval = 30;
            timerGameLoop.Tick += new EventHandler(UpdateGameLoop);
            timerGameLoop.Start();

            var timerChangeAnimationFrame = new Timer();
            timerChangeAnimationFrame.Interval = 300;
            timerChangeAnimationFrame.Tick += new EventHandler((sender, args) =>
            {
                currentFrameAnimation = ++currentFrameAnimation % 2;
            });
            timerChangeAnimationFrame.Start();
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(
                gameModel.Player.Image,
                gameModel.Player.X - gameModel.Player.Image.Width / 4, gameModel.Player.Y - gameModel.Player.Image.Height / 8,
                new Rectangle(new Point(gameModel.Player.Image.Width / 2 * currentFrameAnimation,
                                        gameModel.Player.Image.Height / 4 * (int)GetAnimationType(gameModel.Player.DirectionX, gameModel.Player.DirectionY, gameModel.Player.Sight)),
                              new Size(gameModel.Player.Image.Width / 2, gameModel.Player.Image.Height / 4)),
                GraphicsUnit.Pixel);

            g.TranslateTransform(gameModel.Gun.X, gameModel.Gun.Y);
            g.RotateTransform((float)(gameModel.Gun.Angle * 180 / Math.PI));
            g.TranslateTransform(-gameModel.Gun.X, -gameModel.Gun.Y);
            g.DrawImage(
                Gun.Image,
                gameModel.Gun.X - Gun.Image.Width / 2, gameModel.Gun.Y - Gun.Image.Width / 2,
                new Rectangle(new Point(0, 0), new Size(Gun.Image.Size.Width, Gun.Image.Size.Height)),
                GraphicsUnit.Pixel);
            g.ResetTransform();

            foreach (var bullet in this.gameModel.MovedBullets)
            {
                g.TranslateTransform(bullet.X, bullet.Y);
                g.RotateTransform((float)(bullet.Angle * 180 / Math.PI));
                g.TranslateTransform(-bullet.X, -bullet.Y);
                g.DrawImage(Bullet.Image,
                    bullet.X, bullet.Y,
                    new Rectangle(new Point(0, 0), Bullet.Image.Size),
                    GraphicsUnit.Pixel);
                g.ResetTransform();
            }


        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                gameModel.Shoot();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    gameModel.Player.ChangeDirection(DirectionY.Up);
                    break;
                case Keys.A:
                    gameModel.Player.ChangeDirection(DirectionX.Left);
                    break;
                case Keys.S:
                    gameModel.Player.ChangeDirection(DirectionY.Down);
                    break;
                case Keys.D:
                    gameModel.Player.ChangeDirection(DirectionX.Right);
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.S:
                    gameModel.Player.ChangeDirection(DirectionY.Idle);
                    break;
                case Keys.A:
                case Keys.D:
                    gameModel.Player.ChangeDirection(DirectionX.Idle);
                    break;
            }
        }

        public void UpdateGameLoop(object sender, EventArgs args)
        {
            var mousePos = PointToClient(MousePosition);
            gameModel.Gun.Angle = (float)Math.Atan2(-gameModel.Gun.Y + mousePos.Y, -gameModel.Gun.X + mousePos.X);
            gameModel.Player.Move();


            for (var node = gameModel.MovedBullets.First; !(node is null); node = node.Next)
            {
                if (node.Value.X < 0 || node.Value.X > Size.Width || node.Value.Y < 0 || node.Value.Y > Size.Height)
                {
                    gameModel.MovedBullets.Remove(node);
                    continue;
                }

                node.Value.Move();
            }

            Invalidate();
        }

        private AnimationTypes GetAnimationType(DirectionX directionX, DirectionY directionY, Sight sight)
        {
            if (directionX == DirectionX.Idle && directionY == DirectionY.Idle)
                return sight == Sight.Left ? AnimationTypes.IdleLeft : AnimationTypes.IdleRight;

            return sight == Sight.Left ? AnimationTypes.RunLeft : AnimationTypes.RunRight;
        }
    }
}

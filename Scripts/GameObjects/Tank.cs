﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Tank : Enemy
    {
        private Stack<Point> path = new Stack<Point>();

        private Point nextCheckpoint;
        private Point prevCheckpoint;
        private int resetPath;

        public Tank(int x, int y, int health, int speed, int timeResetPath)
        {
            X = x;
            Y = y;
            Health = health;
            Speed = speed;
            resetPath = timeResetPath;

            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90);

            nextCheckpoint = GameModel.Player.Transform;
        }

        public override void Move(bool isReverse = false)
        {
            if (isReverse)
            {
                X = prevCheckpoint.X;
                Y = prevCheckpoint.Y;
            }

            prevCheckpoint = nextCheckpoint;

            if (path.Count < resetPath)
            { 
                path = NavMeshAgent.GetPath(Transform, GameModel.Player.Transform);
            }
            if (path.Count > 0)
                nextCheckpoint = path.Pop();

            var direction = MoveTowards(Transform, nextCheckpoint, Speed);
            LookAt(GameModel.Player.Transform);

            X = direction.X;
            Y = direction.Y;
        }


        
    }
}

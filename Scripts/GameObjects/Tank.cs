﻿using System.Drawing;
using System.Threading;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Tank : Enemy
    {
        public bool CanKick { get; set; }

        private Point nextCheckpoint;
        private Point prevCheckpoint;
        private readonly int resetPath;

        public Tank(int x, int y, int health, int speed, int timeResetPath, int delayCooldown)
        {
            X = x;
            Y = y;
            Health = health;
            Speed = speed;

            resetPath = timeResetPath;

            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90, isIgnoreNavMesh: true);
            Agent = new NavMeshAgent(this);

            nextCheckpoint = GameModel.Player.Transform;

            Cooldown = new Timer(new TimerCallback((e) => CanKick = true), null, delayCooldown, GameSettings.TankCooldown);
            HealthBar = new HealthBar(this);
        }

        public override void Move(bool isReverse = false)
        {
            Agent.Target = GameModel.Player.Transform;
            if (isReverse)
            {
                X = prevCheckpoint.X;
                Y = prevCheckpoint.Y;
            }

            prevCheckpoint = nextCheckpoint;


            if (Agent.Path.Count > 0)
            {
                nextCheckpoint = Agent.Path.Pop();

                var direction = MoveTowards(Transform, nextCheckpoint, Speed);
                X = direction.X;
                Y = direction.Y;
            }
            LookAt(GameModel.Player.Transform);

        }       
    }
}

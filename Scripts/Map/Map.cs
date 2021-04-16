﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Top_Down_shooter
{
    class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        private readonly int maxLeafSize = 150;
        private readonly int minLeafSize = 80;
        private readonly float roomSplitChance = 0.75f;

        private List<Leaf> leaves = new List<Leaf>();

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public List<Leaf> CreateRandomMap()
        {
            var randGenerator = new Random();
            
            var root = new Leaf(0, 0, Width, Height, minLeafSize);
            leaves.Add(root);

            var didSplite = true;
            while (didSplite)
            {
                didSplite = false;

                var index = 0;
                while (index < leaves.Count)
                {
                    var leaf = leaves[index];
                    if (leaf.LeftChild is null && leaf.RightChild is null)
                    {
                        if (leaf.Width > maxLeafSize || leaf.Height > maxLeafSize || randGenerator.NextDouble() >= 1 - roomSplitChance)
                        {
                            if (leaf.Split())
                            {
                                leaves.Add(leaf.RightChild);
                                leaves.Add(leaf.LeftChild);
                                didSplite = true;
                            }
                        }
                    }
                    index++;
                }
            }
            root.CreateRooms();

            return leaves;
        }
    }
}
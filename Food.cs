﻿using System.Drawing;

namespace SnakeGame
{
    class Food : ICreature
    {
        private Point pos;
        private string name;

        public Food(Point pos, string name)
        {
            this.name = name;
            this.pos = pos;
        }

        public void ActInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Box || conflictedObject is Snake)
            {
                game.FoodEaten();
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public string GetName()
        {
            return name;
        }

        public Point GetPosition()
        {
            return pos;
        }
    }
}

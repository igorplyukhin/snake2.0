using System.Drawing;

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
            if (conflictedObject is Box)
                game.map[pos.X, pos.Y] = null;
        }

        public void ActInConflict(ILiveCreature conflictedObject, Game game)
        {
            if (conflictedObject is Snake)
            {
                game.map[pos.X, pos.Y] = null;
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

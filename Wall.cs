using System.Drawing;

namespace SnakeGame
{
    class Wall : ICreature
    {
        public Point pos;
        public string name;

        public Wall(Point pos, string name)
        {
            this.name = name;
            this.pos = pos;
        }

        public void ActInConflict(ICreature conflictedObject, Game game)
        {
            return;
        }

        public void ActInConflict(IAliveCreature conflictedObject, Game game)
        {
            game.isOver = true;
            game.finishReason = string.Format("{0} is dead", conflictedObject.GetName());
            return;
        }

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, Game game)
        {
            if (conflictedObject is Box)
            {
                game.isOver = true;
                game.finishReason = "Dead snek(";
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
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
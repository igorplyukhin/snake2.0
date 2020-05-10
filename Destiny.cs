using System.Drawing;

namespace SnakeGame
{
    class Destiny : ICreature
    {
        private string name;
        private Point pos;

        public Destiny(Point pos, string name)
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
            return;
        }

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, Game game)
        {
            if (conflictedObject is Box)
            {
                game.map[pos.X, pos.Y] = conflictedObject;
                aliveConflictedObject.AddScore(10);
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


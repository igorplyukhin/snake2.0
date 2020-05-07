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
            if (conflictedObject is Box)
            {
                game.isOver = true;
                game.delayedFinish = true;
                game.finishReason = "Succesfull delievery";
            }
        }

        public void ActInConflict(ILiveCreature conflictedObject, Game game)
        {
            return;
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


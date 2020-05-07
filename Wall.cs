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
            if (conflictedObject is Box)
            {
                game.isOver = true;
                game.finishReason = "Dead snek(";
            }
        }

        public void ActInConflict(ILiveCreature conflictedObject, Game game)
        {
            game.isOver = true;
            game.finishReason = "Голова бо-бо";
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
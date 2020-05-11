using System.Drawing;

namespace SnakeGame
{
    class Wall : ICreature
    {
        private Point pos;
        private string name;

        public Wall(Point pos, string name)
        {
            this.name = name;
            this.pos = pos;
        }

        public void ActInConflict(ICreature conflictedObject, Game game) { }


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

        public string GetName() => name;

        public Point GetPosition() => pos;


        public void SetPosition(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
        }
    }
}
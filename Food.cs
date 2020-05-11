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

        public void ActInConflict(ICreature conflictedObject, Game game) { }

        public void ActInConflict(IAliveCreature conflictedObject, Game game)
        {
            if (conflictedObject is Snake)
            {
                game.map[pos.X, pos.Y] = null;
                game.Add(this);
            }
        }

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, Game game)
        {
            if (conflictedObject is Box)
                game.map[pos.X, pos.Y] = conflictedObject;
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

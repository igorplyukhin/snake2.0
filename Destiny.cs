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

        public void ActInConflict(ICreature conflictedObject, Game game) { }

        public void ActInConflict(IAliveCreature conflictedObject, Game game) { }

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, Game game)
        {
            if (conflictedObject is Box)
            {
                game.map[pos.X, pos.Y] = conflictedObject;
                aliveConflictedObject.AddScore(10);
                game.Add(this);
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


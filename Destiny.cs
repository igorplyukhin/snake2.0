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

        public void ActInConflict(IAliveCreature conflictedObject, int mapWidth, int mapHeight) { }

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, int mapWidth, int mapHeight)
        {
            if (conflictedObject is Box)
                aliveConflictedObject.AddScore(10);
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Box)
                return true;
            return false;
        }

        public bool DeadInConflict(IAliveCreature conflictedObject) => false;

        public string GetName() => name;

        public Point GetPosition() => pos;
    }
}


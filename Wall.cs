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

        public void ActInConflict(IAliveCreature conflictedObject, int mapWidth, int mapHeight) { }

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, int mapWidth, int mapHeight) { }

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public bool DeadInConflict(IAliveCreature conflictedObject) => false;

        public string GetName() => name;

        public Point GetPosition() => pos;
    }
}
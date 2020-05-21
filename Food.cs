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

        public void ActInConflict(IAliveCreature conflictedObject, int mapWidth, int mapHeight) =>
            conflictedObject.AddScore(1);

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, int mapWidth, int mapHeight) {}

        public bool DeadInConflict(ICreature conflictedObject) => true;

        public bool DeadInConflict(IAliveCreature conflictedObject) => true;

        public string GetName() => name;

        public Point GetPosition() => pos;    
    }
}

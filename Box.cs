using System.Collections.Generic;
using System.Drawing;

namespace SnakeGame
{
    class Box : ICreature
    {
        private string name;
        private Point pos;

        public Dictionary<Direction, Point> DirectionToPoint = new Dictionary<Direction, Point>
        {
            {Direction.Down, new Point(0,1)},
            {Direction.Up, new Point(0,-1)},
            {Direction.Left, new Point(-1,0)},
            {Direction.Right, new Point(1,0)}
        };

        public Box(Point pos, string name)
        {
            this.name = name;
            this.pos = pos;
        }

        public void ActInConflict(IAliveCreature conflictedObject, int mapWidth, int mapHeight)
        {
            if (conflictedObject is Snake)
            {
                var dir = DirectionToPoint[conflictedObject.GetDirection()];
                var x = (pos.X + dir.X + mapWidth) % mapWidth;
                var y = (pos.Y + dir.Y + mapHeight) % mapHeight;
                pos = new Point(x, y);
            }
        }

        public string GetName() => name;

        public Point GetPosition() => pos;

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, int mapWidth, int mapHeight) {}

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public bool DeadInConflict(IAliveCreature conflictedObject) => false;
    }
}

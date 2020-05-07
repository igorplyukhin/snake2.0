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

        public void ActInConflict(ICreature conflictedObject, Game game)
        {
            return;
        }

        public void ActInConflict(IAliveCreature conflictedObject, Game game)
        {
            if (conflictedObject is Snake)
            {
                var dir = DirectionToPoint[conflictedObject.GetDirection()];
                var x = (pos.X + dir.X + game.MapWidth) % game.MapWidth;
                var y = (pos.Y + dir.Y + game.MapHeight) % game.MapHeight;
                var creature = game.map[x, y];
                if (creature == null)
                {
                    game.map[pos.X, pos.Y] = null;
                    game.map[x, y] = this;
                    pos = new Point(x, y);
                    return;
                }
                creature.ActInConflict(this, game);
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

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

        public void ActInConflict(ICreature conflictedObject, Game game) {}

        public void ActInConflict(IAliveCreature conflictedObject, Game game)
        {
            if (conflictedObject is Snake)
            {
                var dir = DirectionToPoint[conflictedObject.GetDirection()];
                var x = (pos.X + dir.X + game.MapWidth) % game.MapWidth;
                var y = (pos.Y + dir.Y + game.MapHeight) % game.MapHeight;
                var creature = game.map[x, y];
                game.map[pos.X, pos.Y] = null;
                pos = new Point(x, y);
                if (CheckSnakeCollisions(pos, game))
                {
                    game.isOver = true;
                    game.finishReason = "Box-snake collision";
                }
                if (creature == null)
                    game.map[x, y] = this;
                else
                    creature.ActInConflict(this, conflictedObject, game);
            }
        }

        private bool CheckSnakeCollisions(Point point, Game game)
        {
            foreach (var s in game.aliveCreatures)
                if (s.GetBody().Contains(point))
                    return true;
            return false;
        }

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public string GetName() => name;

        public Point GetPosition() => pos;

        public void ActInConflict(ICreature conflictedObject, IAliveCreature aliveConflictedObject, Game game) {}

        public void SetPosition(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
        }
    }
}

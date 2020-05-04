using System.Drawing;

namespace SnakeGame
{
    class Box : ICreature
    {
        private string name;
        private Point pos;

        public Box(Point pos, string name)
        {
            this.name = name;
            this.pos = pos;
        }

        public void ActInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Snake)
            {
                var dir = Snake.DirectionToPoint[game.snake.direction];
                //var newPos = pos + new Size(dir);
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

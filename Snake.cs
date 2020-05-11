using System.Collections.Generic;
using System.Drawing;

namespace SnakeGame
{
    class Snake : IAliveCreature
    {
        private string name;
        private Direction direction;
        private bool adding = false;
        private int score;
        private LinkedList<Point> body;


        public Dictionary<Direction, Point> DirectionToPoint = new Dictionary<Direction, Point>
        {
            {Direction.Down, new Point(0,1)},
            {Direction.Up, new Point(0,-1)},
            {Direction.Left, new Point(-1,0)},
            {Direction.Right, new Point(1,0)}
        };

        public Snake(Point pos, Direction dir, string name)
        {
            this.name = name;
            body = new LinkedList<Point>();
            body.AddFirst(pos);
            direction = dir;
        }

        public void Move(Game game)
        {
            var dir = DirectionToPoint[direction];
            var curHead = GetPosition();
            var head = new Point((game.MapWidth + curHead.X + dir.X) % game.MapWidth,
                             (game.MapHeight + curHead.Y + dir.Y) % game.MapHeight);
            if (body.Contains(head))
            {
                game.isOver = true;
                game.finishReason = string.Format("Dead {0}", name);
                return;
            }
            if (!adding)
                body.RemoveLast();
            adding = false;
            body.AddFirst(head);
        }

        public void TryChangeDirection(Direction dir)
        {
            var p1 = DirectionToPoint[dir];
            var p2 = DirectionToPoint[direction];
            if (p1.X != p2.X && p1.Y != p2.Y)
                direction = dir;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Wall)
                return true;
            return false;
        }

        public void ActInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Food)
            {
                adding = true;
                score++;
            }
        }

        public Point GetPosition() => body.First.Value;

        public string GetName() => name;

        public LinkedList<Point> GetBody() => body;

        public int GetScore() => score;

        public Direction GetDirection() => direction;

        public void AddScore(int ads)
        {
            score += ads;
        }
    }
}


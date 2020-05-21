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
        private bool liveState = true;


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

        public void Move(int mapWidth,int mapHeight)
        {
            var dir = DirectionToPoint[direction];
            var curHead = GetPosition();
            var head = new Point((mapWidth + curHead.X + dir.X) % mapWidth,
                             (mapHeight + curHead.Y + dir.Y) % mapHeight);
            if (body.Contains(head))
            {
                liveState = false;
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

        public void ActInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Wall)
                liveState = false;
            if (conflictedObject is Food)
                adding = true;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Wall)
                return true;
            return false;
        }

        public Point GetPosition() => body.First.Value;

        public string GetName() => name;

        public LinkedList<Point> GetBody() => body;

        public int GetScore() => score;

        public Direction GetDirection() => direction;

        public void AddScore(int ads) => score += ads;

        public bool IsAlive() => liveState;
    }
}


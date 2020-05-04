using System.Collections.Generic;
using System.Drawing;

namespace SnakeGame
{
    class Snake : ICreature
    {
        private Point head;
        private string name;
        public Direction direction;
        private bool adding = false;
        
        public LinkedList<Point> body;
        
        public static Dictionary<Direction, Point> DirectionToPoint = new Dictionary<Direction, Point>
        {
            {Direction.Down, new Point(0,1)},
            {Direction.Up, new Point(0,-1)},
            {Direction.Left, new Point(-1,0)},
            {Direction.Right, new Point(1,0)}
        };


        public Snake(Point pos, Direction dir, string name)
        {
            this.name = name;
            
            head = pos;
            body = new LinkedList<Point>();
            body.AddFirst(pos);
            direction = dir;
        }

        public void Move(Game game)
        {
            var dir = DirectionToPoint[direction];
            head = new Point((game.MapWidth + head.X + dir.X) % game.MapWidth,
                             (game.MapHeight + head.Y + dir.Y) % game.MapHeight); 
            if (body.Contains(head))
            {
                game.isOver = true;
                game.finishReason = "Dead snake(";
                return;
            }

            if (!adding)
            {
                body.RemoveLast();
            }

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
        

        public void SnakeAdd()
        {
            adding = true;
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
                SnakeAdd();
        }
        

        public Point GetPosition()
        {
            return head;
        }

        public string GetName()
        {
            return name;
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    class Snake : ICreature
    {
        private Point head;
        private string name;
        public Direction direction;
        private bool adding = false;
        
        public LinkedList<Point> body;

        public Snake(Point pos, Direction dir, string name)
        {
            this.name = name;
            
            head = pos;
            body = new LinkedList<Point>();
            body.AddLast(pos);
            direction = dir;
        }

        public void Move(int mapWidth, int mapHeight, Game game)
        {
            var dir = GetDirection(direction);
            head = new Point((mapWidth + head.X + dir.X) % mapWidth,
                             (mapHeight + head.Y + dir.Y) % mapHeight); 
            if (body.Contains(head))
            {
                game.isOver = true;
                game.finishReason = "Dead snake(";
                return;
            }
            if (!adding)
                body.RemoveFirst();
            adding = false;
            body.AddLast(head);
        }

        public void TryChangeDirection(Direction dir)
        {
            var p1 = GetDirection(dir);
            var p2 = GetDirection(direction);
            if (p1.X != p2.X && p1.Y != p2.Y)
                direction = dir;
        }

        public Point GetDirection(Direction dir)
        {
            Point point = new Point(0,0);
            switch(dir)
            {
                case Direction.Down:
                    point = new Point(0, 1);
                    break;
                case Direction.Up:
                    point = new Point(0, -1);
                    break;
                case Direction.Left:
                    point = new Point(-1, 0);
                    break;
                case Direction.Right:
                    point = new Point(1, 0);
                    break;
            }
            return point;
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

        public void Draw(PaintEventArgs e)
        {
            foreach (var b in body)
            {
                e.Graphics.FillRectangle(Brushes.Bisque, b.X * 10, b.Y * 10, 10, 10);
            }
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

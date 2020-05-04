using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    class Game
    {
        public Snake snake;
        private int foodCount;
        public bool isOver;
        public List<ICreature> creatures;
        public ICreature[,] map;
        public string finishReason;
        public int MapWidth => map.GetLength(0);
        public int MapHeight => map.GetLength(1);

        public Game(string map)
        {
            creatures = new List<ICreature>();
            CreatureMapCreator.CreateMap(map, this);
            AddFood();
        }

        public void Restart()
        {
            isOver = false;
            snake = new Snake(new Point(1, 1), Direction.Down, "snake");
            creatures = new List<ICreature>();
            creatures.Add(snake);
            foodCount = 0;
        }

        public void GameIteration()
        {
            snake.Move(this);
            if (!CheckConflicts())
            {
                isOver = true;
                finishReason = "Dead snake(";
                return;
            }

            CheckFood();
        }

        public void KeyPressed(Keys key)
        {
            if (key == Keys.Left)
                snake.TryChangeDirection(Direction.Left);
            if (key == Keys.Up)
                snake.TryChangeDirection(Direction.Up);
            if (key == Keys.Down)
                snake.TryChangeDirection(Direction.Down);
            if (key == Keys.Right)
                snake.TryChangeDirection(Direction.Right);
        }

        public void CheckFood()
        {
            if (foodCount == 0)
                AddFood();
        }

        public void AddFood()
        {
            var rndm = new Random();
            var x = rndm.Next(0, MapWidth);
            var y = rndm.Next(0, MapHeight);
            while (snake.body.Contains(new Point(x,y)) || map[x, y] != null)
            {
                x = rndm.Next(0, MapWidth);
                y = rndm.Next(0, MapHeight);
            }

            var food = new Food(new Point(x, y), "food");
            map[x, y] = food;
            creatures.Add(food);
            foodCount++;
        }

        public void FoodEaten() =>
            foodCount--;

        public bool CheckConflicts()
        {
            var survived = new List<ICreature>();
            survived.Add(snake);
            foreach (var c in creatures)
            {
                if (snake.GetPosition() != c.GetPosition())
                {
                    survived.Add(c);
                    continue;
                }

                if (snake.DeadInConflict(c))
                    return false;
                if (!c.DeadInConflict(snake))
                    survived.Add(c);
                c.ActInConflict(snake, this);
                snake.ActInConflict(c, this);
                if (isOver)
                    return true;
            }

            creatures = survived;
            return true;
        }
    }
}
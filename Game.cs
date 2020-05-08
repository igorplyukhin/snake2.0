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
        private string mapSave;
        private int foodCount;
        public bool isOver;
        public bool delayedFinish;
        public List<IAliveCreature> aliveCreatures;
        public ICreature[,] map;
        public string finishReason;
        public int MapWidth => map.GetLength(0);
        public int MapHeight => map.GetLength(1);

        public Game(string map)
        {
            mapSave = map;
            aliveCreatures = new List<IAliveCreature>();
            CreatureMapCreator.CreateMap(map, this);
            AddFood();
        }

        public void Restart()
        {
            isOver = false;
            delayedFinish = false;
            aliveCreatures = new List<IAliveCreature>();
            CreatureMapCreator.CreateMap(mapSave, this);
            foodCount = 0;
        }

        public void GameIteration()
        {
            foreach (var s in aliveCreatures)
                s.Move(this);
            CheckFood();
        }

        public void KeyPressed(Keys key)
        {
            if (key == Keys.Left)
                aliveCreatures[0].TryChangeDirection(Direction.Left);
            if (key == Keys.Up)
                aliveCreatures[0].TryChangeDirection(Direction.Up);
            if (key == Keys.Down)
                aliveCreatures[0].TryChangeDirection(Direction.Down);
            if (key == Keys.Right)
                aliveCreatures[0].TryChangeDirection(Direction.Right);
            if (aliveCreatures.Count == 1)
                return;
            if (key == Keys.A)
                aliveCreatures[1].TryChangeDirection(Direction.Left);
            if (key == Keys.W)
                aliveCreatures[1].TryChangeDirection(Direction.Up);
            if (key == Keys.S)
                aliveCreatures[1].TryChangeDirection(Direction.Down);
            if (key == Keys.D)
                aliveCreatures[1].TryChangeDirection(Direction.Right);
        }

        public void CheckFood()
        {
            if (foodCount == 0)
                AddFood();
        }

        public void AddFood()
        {
            var rndm = new Random();
            var x = 0;
            var y = 0;
            var check = true;
            while (check)
            {
                var count = aliveCreatures.Count;
                x = rndm.Next(0, MapWidth);
                y = rndm.Next(0, MapHeight);
                foreach (var s in aliveCreatures)
                {
                    var body = s.GetBody();
                    if (map[x, y] == null && !body.Contains(new Point(x, y)))
                        count--;
                }
                check = count == 0 ? false : true;
            }
            var food = new Food(new Point(x, y), "food");
            map[x, y] = food;
            foodCount++;
        }

        public void FoodEaten() =>
            foodCount--;
    }
}
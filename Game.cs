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
            this.map = CreatureMapCreator.CreateMap(map, aliveCreatures);
            Add("food");
        }

        public void Restart()
        {
            finishReason = "";
            isOver = false;
            delayedFinish = false;
            aliveCreatures = new List<IAliveCreature>();
            map = CreatureMapCreator.CreateMap(mapSave, aliveCreatures);
            Add("food");
        }

        public void GameIteration()
        {
            foreach (var s in aliveCreatures)
            {
                s.Move(MapWidth, MapHeight);
                if (!s.IsAlive())
                {
                    finishReason += s.GetName() + " dead, ";
                    isOver = true;
                }                
            }
            if (isOver)
                return;
            CheckCollisions();
        }

        private void CheckCollisions()
        {
            if (aliveCreatures.Count > 1)
                CheckSnakeCollisions();
            foreach (var s in aliveCreatures)
            {
                var pos = s.GetPosition();
                var creature = map[pos.X, pos.Y];
                if (creature != null)
                {
                    if (s.DeadInConflict(creature))
                    {
                        finishReason += s.GetName() + " dead,";
                        isOver = true;
                        continue;
                    }
                    creature.ActInConflict(s, MapWidth, MapHeight);
                    s.ActInConflict(creature);
                    map[pos.X, pos.Y] = null;
                    if (creature.DeadInConflict(s))
                    {
                        if (creature is Food)
                            Add("food");
                        continue;
                    }
                    CheckSecondCollision(s, creature);
                }
            }
        }

        private void CheckSecondCollision(IAliveCreature snake, ICreature creature)
        {
            var cPos = creature.GetPosition();
            var conflictedCreature = map[cPos.X, cPos.Y];
            if (conflictedCreature != null)
            {
                if (conflictedCreature.DeadInConflict(creature))
                {
                    conflictedCreature.ActInConflict(creature, snake, MapWidth, MapHeight);
                    creature.ActInConflict(conflictedCreature, snake, MapWidth, MapHeight);
                    map[cPos.X, cPos.Y] = creature;
                    if (conflictedCreature is Destiny)
                        Add("destiny");
                    return;
                }
                isOver = true;
                finishReason += snake.GetName() + " dead,";
                return;
            }
            map[cPos.X, cPos.Y] = creature;
        }

        private void CheckSnakeCollisions()
        {
            var first = aliveCreatures[0];
            var second = aliveCreatures[1];
            var firstInSecond = second.GetBody().Contains(first.GetPosition());
            var secondInFirst = first.GetBody().Contains(second.GetPosition());
            if (firstInSecond && secondInFirst)
            {
                isOver = true;
                finishReason = "Snakes collided";
            }
            if (firstInSecond && !secondInFirst)
            {
                isOver = true;
                finishReason = String.Format("{0} is dead", first.GetName());
            }
            if (!firstInSecond && secondInFirst)
            {
                isOver = true;
                finishReason = String.Format("{0} is dead", second.GetName());
            }

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

        public void Add(string name)
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
            switch (name)
            {
                case "food":
                    {
                        map[x, y] = new Food(new Point(x,y), name);
                        break;
                    }
                case "destiny":
                    {
                        map[x, y] = new Destiny(new Point(x, y), name);
                        break;
                    }
            }
        }
    }
}
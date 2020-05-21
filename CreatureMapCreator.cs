using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SnakeGame
{
    static class CreatureMapCreator
    {
        public static ICreature[,] CreateMap(string map, List<IAliveCreature> aliveCreatures, string separator = "\r\n")
        {
            var rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong test map '{map}'");
            var gameMap = new ICreature[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                {
                    gameMap[x, y] = CreateLiveCreatureBySymbol(rows[y][x], x, y, aliveCreatures);
                    if (gameMap[x, y] == null)
                        gameMap[x, y] = CreateCreatureBySymbol(rows[y][x], x, y);
                }
            return gameMap;
        }

        private static ICreature CreateLiveCreatureBySymbol(char c, int x, int y, List<IAliveCreature> aliveCreatures)
        {
            switch (c)
            {
                case 'S':
                    var name = String.Format("snake{0}", aliveCreatures.Count + 1);
                    var creature = new Snake(new Point(x, y), Direction.Down, name);
                    aliveCreatures.Add(creature);
                    return null;
                default:
                    return null;
            }
        }

        private static ICreature CreateCreatureBySymbol(char c, int x, int y)
        {
            switch (c)
            {
                case 'N':
                    return null;
                case 'W':
                    {
                        var creature = new Wall(new Point(x, y), "wall");
                        return creature;
                    }
                case 'B':
                    {
                        var creature = new Box(new Point(x, y), "box");
                        return creature;
                    }
                case 'D':
                    {
                        var creature = new Destiny(new Point(x, y), "destiny");
                        return creature;
                    }
                default:
                    return null;
            }
        }
    }
}
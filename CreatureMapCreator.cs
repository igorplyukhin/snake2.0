using System;
using System.Drawing;
using System.Linq;

namespace SnakeGame
{
    static class CreatureMapCreator
    {
        public static void CreateMap(string map, Game game, string separator = "\r\n")
        {
            var rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong test map '{map}'");
            game.map = new ICreature[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                {
                    game.map[x, y] = CreateLiveCreatureBySymbol(rows[y][x], x, y, game);
                    if (game.map[x, y] == null)
                        game.map[x, y] = CreateCreatureBySymbol(rows[y][x], x, y, game);
                }

        }

        private static ICreature CreateLiveCreatureBySymbol(char c, int x, int y, Game game)
        {
            switch (c)
            {
                case 'S':
                    var name = String.Format("snake{0}", game.aliveCreatures.Count);
                    var creature = new Snake(new Point(x, y), Direction.Down, name);
                    game.aliveCreatures.Add(creature);
                    return null;
                default:
                    return null;
            }
        }

        private static ICreature CreateCreatureBySymbol(char c, int x, int y, Game game)
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
using System;
using System.Drawing;
using System.Linq;

namespace SnakeGame
{
    static class CreatureMapCreator
    {
        public static void CreateMap(string map,Game game, string separator = "\r\n")
        {
            var rows = map.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong test map '{map}'");
            game.map = new ICreature[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                    game.map[x, y] = CreateCreatureBySymbol(rows[y][x], x, y, game);
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
                        game.creatures.Add(creature);
                        return creature;
                    }
                case 'B':
                    {
                        var creature = new Box(new Point(x, y), "box");
                        game.creatures.Add(creature);
                        return creature;
                    }
                case 'D':
                    {
                        var creature = new Destiny(new Point(x, y), "destiny");
                        game.creatures.Add(creature);
                        return creature;
                    }
                case 'S':
                {
                    var creature = new Snake(new Point(x,y), Direction.Down, "snake" );
                    game.creatures.Add(creature);
                    game.snake = creature;
                    return creature;
                }
                default:
                    throw new Exception($"wrong character for ICreature {c}");
            }
        }
    }
}

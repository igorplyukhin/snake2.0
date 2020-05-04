using NUnit.Framework;
using SnakeGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tests
{
    [TestFixture]
    public class GameSpecification
    {
        private const string map1 = @"
NNNNNNNNNN
NNNNNNNNNN
NNNNNNNNNN
NNNNNNNNNN
WWWWWWWWWW";

        private const string map2 = @"
NNNNNNNNNN
NNNNNNNNNN
NNNNNNNNNN
NNNNNNNNNN
NNNNNNNNNN";

        private const string map3 = @"
NNNNNNNNNN
NNNNNNNNNN
NBNNNNNNNN
NNNNNNNNNN
NDNNNNNNNN";

        [Test]
        public void SnakeShouldMoveToDirectionWithoutClick()
        {
            var game = new Game(map1);
            var direction = game.snake.GetDirection(game.snake.direction);
            var firstPos = game.snake.GetPosition();
            game.GameIteration();
            var secondPos = game.snake.GetPosition();
            var move = secondPos - new Size(firstPos);
            Assert.AreEqual(direction, move, "Unexpected move");
        }

        [Test]
        public void GameIsOverBycollidingWithWall()
        {
            var game = new Game(map1);
            var stepsToWall = game.MapHeight - game.snake.GetPosition().Y;
            for (var i = 1; i < stepsToWall; i++)
                game.GameIteration();
            Assert.AreEqual(true, game.isOver, "No die");
        }

        [Test]
        public void SnakeShouldIncreaseLengthEatingFood()
        {
            var game = new Game(map1);
            game.map[1, 2] = new Food(new Point(1, 2), "food");
            game.creatures.Add(game.map[1, 2]);
            var startLength = game.snake.body.Count;
            for (var i = 0; i < 2; i++)
                game.GameIteration();
            Assert.AreEqual(startLength + 1, game.snake.body.Count);
        }

        [Test]
        public void SnakeShouldDelieverBox()
        {
            var game = new Game(map3);
            for (var i = 0; i < 2; i++)
                game.GameIteration();
            Assert.AreEqual(true, game.isOver);
            Assert.AreEqual("Succesfull delievery", game.finishReason);
        }

        [Test]
        public void SnakeShouldDieCollidingWithHerself()
        {
            var game = new Game(map2);
            for (var i = 2; i < game.MapHeight; i++)
            {
                game.map[1, i] = new Food(new Point(1, i), "food");
                game.creatures.Add(game.map[1, i]);
                game.GameIteration();
            }
            game.KeyPressed(Keys.Right);
            game.GameIteration();
            game.KeyPressed(Keys.Up);
            game.GameIteration();
            game.KeyPressed(Keys.Left);
            game.GameIteration();
            Assert.AreEqual(true, game.isOver);
            Assert.AreEqual("Dead snake(", game.finishReason);
        }

        [TestCase(Keys.Left, -1, 0)]
        [TestCase(Keys.Right, 1, 0)]
        [TestCase(Keys.Down, 0, 1)]
        //Начальное направление вниз
        public void SnakeMoveToDirectionWhereClickedFromDown(Keys pressed, int x, int y)
        {
            var game = new Game(map1);
            var firstPos = game.snake.GetPosition();
            game.KeyPressed(pressed);
            game.GameIteration();
            var secondPos = game.snake.GetPosition();
            var move = secondPos - new Size(firstPos);
            var expectedMove = new Point(x, y);
            Assert.AreEqual(expectedMove, move, "Incorrect move");
        }
    }
}

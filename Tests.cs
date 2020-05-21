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
    NNNNNSNNNN
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
    NSNNNNNNNN
    NBNNNNNNNN
    NNNNNNNNNN
    NDNNNNNNNN";

        [Test]
        public void SnakeShouldMoveToDirectionWithoutClick()
        {
            var game = new Game(map1);
            var direction = new Point(0, 1);
            var snake = game.aliveCreatures[0];
            var firstPos = snake.GetPosition();
            game.GameIteration();
            var secondPos = snake.GetPosition();
            var move = secondPos - new Size(firstPos);
            Assert.AreEqual(direction, move, "Unexpected move");
        }

        [Test]
        public void GameIsOverBycollidingWithWall()
        {
            var game = new Game(map1);
            var stepsToWall = game.MapHeight - game.aliveCreatures[0].GetPosition().Y;
            for (var i = 1; i < stepsToWall; i++)
                game.GameIteration();
            Assert.AreEqual(true, game.isOver, "No die");
        }

        [Test]
        public void SnakeShouldIncreaseLengthEatingFood()
        {
            var game = new Game(map2);
            game.aliveCreatures.Add(new SnakeGame.Snake(new Point(1, 1), Direction.Down, ""));
            game.map[1, 2] = new Food(new Point(1, 2), "food");
            var startLength = game.aliveCreatures[0].GetBody().Count;
            for (var i = 0; i < 3; i++)
                game.GameIteration();
            Assert.AreEqual(startLength + 1, game.aliveCreatures[0].GetBody().Count);
        }

        [Test]
        public void SnakeShouldDelieverBox()
        {
            var game = new Game(map3);
            for (var i = 0; i < 2; i++)
                game.GameIteration();
            Assert.AreEqual(10, game.aliveCreatures[0].GetScore()); ;
        }

        [Test]
        public void SnakeShouldDieCollidingWithHerself()
        {
            var game = new Game(map2);
            var snake = new SnakeGame.Snake(new Point(1, 1), Direction.Down, "");
            game.aliveCreatures.Add(snake);
            for (var i = 2; i < game.MapHeight; i++)
            {
                game.map[1, i] = new Food(new Point(1, i), "food");
                game.GameIteration();
            }
            game.KeyPressed(Keys.Right);
            game.GameIteration();
            game.KeyPressed(Keys.Up);
            game.GameIteration();
            game.KeyPressed(Keys.Left);
            game.GameIteration();
            Assert.AreEqual(true, game.isOver);
            Assert.AreEqual(false, snake.IsAlive());
        }

        [TestCase(Keys.Left, -1, 0)]
        [TestCase(Keys.Right, 1, 0)]
        [TestCase(Keys.Down, 0, 1)]
        //Начальное направление вниз
        public void SnakeMoveToDirectionWhereClickedFromDown(Keys pressed, int x, int y)
        {
            var game = new Game(map1);
            var snake = game.aliveCreatures[0];
            var firstPos = snake.GetPosition();
            game.KeyPressed(pressed);
            game.GameIteration();
            var secondPos = snake.GetPosition();
            var move = secondPos - new Size(firstPos);
            var expectedMove = new Point(x, y);
            Assert.AreEqual(expectedMove, move, "Incorrect move");
        }
    }

    [TestFixture]
    public class SnakeTests
    {
        private const string map = @"
    NNNNN
    NNNNN
    NNNNN";

        [TestCase(0, 1, 0)]//Up
        [TestCase(1, 1, 2)]//Down
        [TestCase(2, 2, 1)]//Right
        [TestCase(3, 0, 1)]//Left
        public void SnakeShouldMoveToDirrections(int index, int expectedX, int expectedY)
        {
            var game = new Game(map);
            var i = 0;
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (i == index)
                    game.aliveCreatures.Add(new SnakeGame.Snake(new Point(1, 1), dir, ""));
                i++;
            }
            var snake = game.aliveCreatures[0];
            snake.Move(5,3);
            Assert.AreEqual(expectedX, snake.GetPosition().X);
            Assert.AreEqual(expectedY, snake.GetPosition().Y);
        }

        [Test]
        public void InfiniteMapHorizontalRight()
        {
            var game = new Game(map);
            game.aliveCreatures.Add(new SnakeGame.Snake(new Point(0, 0), Direction.Right, ""));
            for (var i = 0; i < game.MapWidth; i++)
                game.aliveCreatures[0].Move(game.MapWidth, game.MapHeight);
            Assert.AreEqual(new Point(0, 0), game.aliveCreatures[0].GetPosition());
        }

        [Test]
        public void InfiniteMapHorizontalLeft()
        {
            var game = new Game(map);
            game.aliveCreatures.Add(new SnakeGame.Snake(new Point(0, 0), Direction.Left, ""));
            for (var i = 0; i < game.MapWidth; i++)
                game.aliveCreatures[0].Move(game.MapWidth, game.MapHeight);
            Assert.AreEqual(new Point(0, 0), game.aliveCreatures[0].GetPosition());
        }

        [Test]
        public void InfiniteMapVerticalUp()
        {
            var game = new Game(map);
            game.aliveCreatures.Add(new SnakeGame.Snake(new Point(0, 0), Direction.Up, ""));
            for (var i = 0; i < game.MapHeight; i++)
                game.aliveCreatures[0].Move(game.MapWidth, game.MapHeight);
            Assert.AreEqual(new Point(0, 0), game.aliveCreatures[0].GetPosition());
        }

        [Test]
        public void InfiniteMapVerticalDown()
        {
            var game = new Game(map);
            game.aliveCreatures.Add(new SnakeGame.Snake(new Point(0, 0), Direction.Down, ""));
            for (var i = 0; i < game.MapHeight; i++)
                game.aliveCreatures[0].Move(game.MapWidth, game.MapHeight);
            Assert.AreEqual(new Point(0, 0), game.aliveCreatures[0].GetPosition());
        }
    }
}

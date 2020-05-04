using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SnakeGame
{
    class MainForm : Form
    {
        private const string mapWithPlayerTerrain = @"
NNNNNNNNNNNNNNNNNNN
NNNNNNNNNNNNNNNNNNN
NNNNNNNNNNNNNNNNNNW
NNNNNNNNNNNNNNNNNNW
NNNNNNNNNNNNNNNNNNW
NNNNNNNNNNNNNNNNNNW
NNNNNNNNNNNNNNNNNNW
NNNNNNNNNNNNNNNNNNW
NNNNNNNNNNNNNNNNNNW
NNNNNBNNNNNNNNNNNNN
NNNNNNNNNNNNNNNNNNN
NNNNNNNNNNNNNNDNNNN
NNNNNNNNNNNNNNNNNNN
NNNNNNNNNNNNNNNNNNN";

        private readonly Timer timer;
        private Game game;
        private const int ElementSize = 30;

        public MainForm()
        {
            //game = new Game(15, 15);
            game = new Game(mapWithPlayerTerrain);
            ClientSize = new Size(
                ElementSize * game.MapWidth,
                ElementSize * game.MapHeight + ElementSize);
            timer = new Timer { Interval = 150 };
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            game.GameIteration();
            if (game.isOver)
            {
                timer.Stop();
                var res = MessageBox.Show("Restart ?", game.finishReason, MessageBoxButtons.YesNo);
                if (res != DialogResult.Yes)
                    this.Close();
                game.Restart();
                timer.Start();
            }
            Invalidate();
            Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (var x = 0; x < game.MapWidth; x++)
                for (var y = 0; y < game.MapHeight; y++)
                    e.Graphics.FillRectangle(creatureColor["background"],
                        x* ElementSize, y* ElementSize,
                        ElementSize, ElementSize);
            foreach (var c in game.creatures.Skip(1))
            {
                e.Graphics.FillRectangle(creatureColor[c.GetName()],
                    c.GetPosition().X*ElementSize, c.GetPosition().Y*ElementSize,
                    ElementSize, ElementSize);
            }
            foreach (var b in game.snake.body)
            {
                e.Graphics.FillRectangle(creatureColor[game.snake.GetName()],
                    b.X * ElementSize, b.Y * ElementSize,
                    ElementSize, ElementSize);
            }
        }

        private Dictionary<string, Brush> creatureColor = new Dictionary<string, Brush>
        {
            {"snake", Brushes.Gray },
            {"wall", Brushes.Black },
            {"food", Brushes.Red },
            {"background", Brushes.AliceBlue },
            {"teleport", Brushes.Violet },
            {"box", Brushes.Brown },
            {"destiny", Brushes.DarkBlue }
        };

        protected override void OnKeyDown(KeyEventArgs e)
        {
            game.KeyPressed(e.KeyCode);
        }        
    }
}

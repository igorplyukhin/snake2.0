using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Snake;

namespace SnakeGame
{
    class MainForm : Form
    {
        private readonly Timer timer;
        private Game game;
        private const int ElementSize = 32;
        public MainForm()
        {
            DoubleBuffered = true;
            game = new Game(Levels.mapWithTwoSnakes);
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
                if (game.delayedFinish)
                    Invalidate();
                timer.Stop();
                var res = MessageBox.Show("Restart ?", game.finishReason, MessageBoxButtons.YesNo);
                if (res != DialogResult.Yes)
                    this.Close();
                game.Restart();
                timer.Start();
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (var x = 0; x < game.MapWidth; x++)
                for (var y = 0; y < game.MapHeight; y++)
                {
                    e.Graphics.FillRectangle(creatureColor["background"],
                        x * ElementSize, y * ElementSize,
                        ElementSize, ElementSize);
                    if (game.map[x, y] != null)
                    {
                        var creature = game.map[x, y];
                        e.Graphics.FillRectangle(creatureColor[creature.GetName()],
                        creature.GetPosition().X * ElementSize, creature.GetPosition().Y * ElementSize,
                        ElementSize, ElementSize);
                    }

                }
            foreach (var creature in game.aliveCreatures)
                DrawAliveCreature(creature, e);
        }

        private void DrawAliveCreature(IAliveCreature creature, PaintEventArgs e)
        {
            foreach (var part in creature.GetBody())
                e.Graphics.FillRectangle(creatureColor[creature.GetName()],
                        part.X * ElementSize, part.Y * ElementSize,
                        ElementSize, ElementSize);
        }

        private Dictionary<string, Brush> creatureColor = new Dictionary<string, Brush>
        {
            {"snake0", Brushes.Gray },
            {"snake1", Brushes.Yellow },
            {"wall", Brushes.Black },
            {"food", Brushes.Red },
            {"background", Brushes.AliceBlue },
            {"teleport", Brushes.Violet },
            {"box", Brushes.Brown },
            {"destiny", Brushes.DarkBlue }
        };

        protected override void OnKeyDown(KeyEventArgs e)
        {
            game.KeyPressed(e.KeyCode, game.aliveCreatures[0]);
        }
    }
}

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
        private Timer timer;
        private Game game;
        private const int ElementSize = 32;

        public MainForm()
        {
            DoubleBuffered = true;
            MinimumSize = new Size(600,1000);
            MainMenuStrip = new MenuStrip();
            AddButtonsToMenu();
            Controls.Add(MainMenuStrip);
        }

        private void StartGame()
        {
            ClientSize = new Size(
                ElementSize * game.MapWidth,
                ElementSize * game.MapHeight + ElementSize);
            timer = new Timer {Interval = 150};
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void AddButtonsToMenu()
        {
            var statesBtn = new ToolStripDropDownButton("Level", null, BuildDropDownItems()) {Name = "Level"};
            var pauseBtn = new ToolStripButton("Pause", null, (sender, args) =>
            { 
                if (timer == null) 
                    return;
                if (timer.Enabled)
                    timer.Stop();
                else
                    timer.Start();
            }) {Name = "Pause"};

            var plusSpeedBtn = new ToolStripButton("+Speed", null,
                (sender, args) =>
                {
                    if (timer != null && timer.Interval > 25)
                        timer.Interval -= 25;
                }) {Name = "+Speed"};
            var minusSpeedBtn = new ToolStripButton("-Speed", null,
                (sender, args) =>
                {
                    if (timer != null)
                        timer.Interval += 25;
                }) {Name = "-Speed"};
            MainMenuStrip.Items.Add(statesBtn);
            MainMenuStrip.Items.Add(pauseBtn);
            MainMenuStrip.Items.Add(plusSpeedBtn);
            MainMenuStrip.Items.Add(minusSpeedBtn);
        }

        private ToolStripItem[] BuildDropDownItems()
        {
            return typeof(Levels).GetFields()
                .Select(level => (ToolStripItem) new ToolStripButton(level.Name, null, ChangeLevel)
                {
                    Name = level.Name
                })
                .ToArray();
        }

        private void ChangeLevel(object sender, EventArgs e)
        {
            var levelName = ((ToolStripButton) sender).Name;
            game = new Game(typeof(Levels).GetField(levelName).GetValue(null).ToString());
            StartGame();
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
                else
                {
                    game.Restart();
                    timer.Start();
                }
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (game == null)
                return;
            e.Graphics.TranslateTransform(0, ElementSize);
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
            {
                DrawAliveCreature(creature, e);
                e.Graphics.DrawString(creature.GetScore().ToString(), new Font("Arial", 16), Brushes.Green, 50, 0);
            }
                
            e.Graphics.ResetTransform();
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
            {"snake0", Brushes.Gray},
            {"snake1", Brushes.Yellow},
            {"wall", Brushes.Black},
            {"food", Brushes.Red},
            {"background", Brushes.AliceBlue},
            {"teleport", Brushes.Violet},
            {"box", Brushes.Brown},
            {"destiny", Brushes.DarkBlue}
        };

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (game != null)
                game.KeyPressed(e.KeyCode);
            switch (e.KeyCode)
            {
                case Keys.P:
                    MainMenuStrip.Items["Pause"].PerformClick();
                    break;
                case Keys.OemMinus:
                    MainMenuStrip.Items["-Speed"].PerformClick();
                    break;
                case Keys.Oemplus:
                    MainMenuStrip.Items["+Speed"].PerformClick();
                    break;
            }
        }
    }
}
using System.Diagnostics;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        private Game game;
        private Stopwatch stopwatch;
        public Form1()
        {
            InitializeComponent();
            game = new Game();
            stopwatch = new Stopwatch();
        }
        private bool active;
        private void Form1_Load(object sender, EventArgs e) => comboBox1.SelectedIndex = 1;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) => Start();
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (active) Drawer(e.X / game.ratiowidth, e.Y / game.ratioheight, e.Button);
        }
        private void CanvasDisplay() => pictureBox1.Image = game.CanvasImage;
        private void StatDisplay()
        {
            label3.Text = $"Games played: {game.played}";
            label4.Text = $"Victories: {game.won}";
            label5.Text = $"Defeats: {game.lost}";
            label6.Text = (game.won + game.lost) == 0 ? "" : $"Winning Ratio: {Math.Round((double)game.won / (game.won + game.lost < game.played ? game.played - 1 : game.played) * 100, 2)}%";
        }
        private void ActivSwitch(bool mod) => active = comboBox1.Enabled = mod;
        private void FlagDisplay() => label2.Text = $"Flags: {game.flags}";
        private void TimeDisplay(int time) => label7.Text = $"Time: {time} {(time < 2 ? "second" : "seconds")}.";
        private void timer1_Tick(object sender, EventArgs e) => TimeDisplay((int)stopwatch.Elapsed.TotalSeconds);
        private void MessageDisplay(dynamic d)
        {
            Form2 f2 = new Form2(stopwatch.Elapsed.TotalSeconds, d.message, game.ActualDifficulty);
            f2.ShowDialog();
            Invoke(() => { Start(); });
        }
        private void MessageThread(string message)
        {
            timer1.Stop();
            stopwatch.Stop();
            ActivSwitch(false);
            Thread result = new Thread(new ParameterizedThreadStart(MessageDisplay));
            result.IsBackground = true;
            result.Start(new { message });
        }
        private void Start()
        {
            game.Start(comboBox1.SelectedIndex);
            ActivSwitch(true);
            FlagDisplay();
            StatDisplay();
            CanvasDisplay();
            TimeDisplay(0);
            timer1.Start();
            stopwatch.Restart();
        }
        private void Drawer(int x, int y, MouseButtons button)
        {
            if (game.firstclick)
            {
                if (button == MouseButtons.Left)
                {
                    game.firstclick = false;
                    game.GenerateMines(x, y);
                    RemoveTitle(x, y);
                }
            }
            else if (!game.Unlocked(y, x))
            {
                switch (button)
                {
                    case MouseButtons.Left: if (!game.Flagged(y, x)) RemoveTitle(x, y); break;
                    case MouseButtons.Right:
                        if (game.PlaceFlag(x, y)) WinDisplay();
                        else CanvasDisplay();
                        FlagDisplay();
                        break;
                }
            }
            else if (button == MouseButtons.Left)
            {
                if (game.ExtraUnlocker(x, y, out byte overrides)) FlagDisplay();
                DisplayHandler(overrides);
            }
        }
        private void RemoveTitle(int x, int y)
        {
            if (game.RemoveTitle(x, y, out byte overrides)) FlagDisplay();
            DisplayHandler(overrides);
        }
        private void DisplayHandler(byte mod)
        {
            switch (mod)
            {
                case 0: CanvasDisplay(); break;
                case 1: LossHandler(); break;
                case 2: WinDisplay(); break;
            }
        }
        private void WinDisplay()
        {
            CanvasDisplay();
            StatDisplay();
            MessageThread("You won!");
        }
        private void LossHandler()
        {
            CanvasDisplay();
            StatDisplay();
            MessageThread("You lost!");
        }
    }
}
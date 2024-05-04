
namespace minesweeper
{
    internal class Game
    {
        private Render render;
        private Difficulty difficulty;
        private Table table;
        public Game()
        {
            render = new Render();
            difficulty = new Difficulty();
            table = new Table();
        }
        public bool firstclick;
        public int flags, won = 0, lost = 0, played = 0;
        public void Start(int selectedindex)
        {
            firstclick = true;
            if (selectedindex != difficulty.ActualDifficulty)
            {
                difficulty.SetDifficulty((byte)selectedindex);
                render.CreateNew(difficulty.width, difficulty.height);
            }
            table.CreateNew(difficulty.width, difficulty.height);
            render.AllDisplay();
            if (lost + won == played) played++;
            flags = difficulty.quantity;
        }
        public void GenerateMines(int ax, int ay) => table.GenerateMines(ax, ay, difficulty.quantity);
        public int ratiowidth => render.ratiowidth;
        public int ratioheight => render.ratioheight;
        public byte ActualDifficulty => difficulty.ActualDifficulty;
        public bool PlaceFlag(int x, int y)
        {
            if (!table.Flagged(y, x))
            {
                table.FlagModifier(x, y, true);
                render.ItemDisplay(x, y, 'f');
                flags--;
                return WinDeterminer();
            }
            table.FlagModifier(x, y, false);
            render.ItemDisplay(x, y, 't');
            flags++;
            return false;
        }
        private bool WinDeterminer()
        {
            if (flags != 0) return false;
            for (int y = 0; y < table.heightmatrix; y++)
            {
                for (int x = 0; x < table.widthmatrix; x++)
                {
                    if (table.Value(y, x) == -1)
                    {
                        if (!table.Flagged(y, x)) return false;
                    }
                    else if (!table.Unlocked(y, x)) return false;
                }
            }
            won++;
            return true;
        }
        public bool RemoveTitle(int x, int y, out byte overrides)
        {
            if (table.Value(y, x) == -1)
            {
                lost++;
                MineUncover(x, y);
                overrides = 1;
                return false;
            }
            bool flagmod = false;
            Unlocker(x, y, ref flagmod);
            overrides = WinDeterminer() ? (byte)2 : (byte)0;
            return flagmod;
        }
        public bool ExtraUnlocker(int x, int y, out byte overrides)
        {
            overrides = 3;
            bool flagmod = false;
            List<Point> unlockable = table.Around(x, y);
            if (unlockable != null)
            {
                foreach (Point item in unlockable)
                {
                    flagmod = flagmod || RemoveTitle(item.X, item.Y, out overrides);
                    if (overrides == 1) return flagmod;
                }
            }
            return flagmod;
        }
        private void Unlocker(int x, int y, ref bool flagchange)
        {
            table.Unlocker(x, y);
            if (table.Flagged(y, x))
            {
                table.FlagModifier(x, y, false);
                flags++;
                flagchange = true;
            }
            render.ItemDisplay(x, y, table.Value(y, x) > 0 && table.Value(y, x) < 9 ? table.Value(y, x).ToString()[0] : table.Value(y, x) == 0 ? 'n' : throw new KeyNotFoundException(table.Value(y, x).ToString()));
            if (table.Value(y, x) == 0)
            {
                bool up = y > 0;
                if (up && !table.Unlocked(y - 1, x)) Unlocker(x, y - 1, ref flagchange);
                bool down = (y + 1) < table.heightmatrix;
                if (down && !table.Unlocked(y + 1, x)) Unlocker(x, y + 1, ref flagchange);
                if (x > 0)
                {
                    if (!table.Unlocked(y, x - 1)) Unlocker(x - 1, y, ref flagchange);
                    if (up && !table.Unlocked(y - 1, x - 1)) Unlocker(x - 1, y - 1, ref flagchange);
                    if (down && !table.Unlocked(y + 1, x - 1)) Unlocker(x - 1, y + 1, ref flagchange);
                }
                if ((x + 1) < table.widthmatrix)
                {
                    if (!table.Unlocked(y, x + 1)) Unlocker(x + 1, y, ref flagchange);
                    if (up && !table.Unlocked(y - 1, x + 1)) Unlocker(x + 1, y - 1, ref flagchange);
                    if (down && !table.Unlocked(y + 1, x + 1)) Unlocker(x + 1, y + 1, ref flagchange);
                }
            }
        }
        private void MineUncover(int x, int y)
        {
            render.ItemDisplay(x, y, 'c');
            for (int my = 0; my < table.heightmatrix; my++)
            {
                for (int mx = 0; mx < table.widthmatrix; mx++)
                {
                    if (x != mx || y != my)
                    {
                        if (table.Value(my, mx) == -1)
                        {
                            if (!table.Flagged(my, mx)) render.ItemDisplay(mx, my, 'b');
                        }
                        else if (table.Flagged(my, mx)) render.ItemDisplay(mx, my, 'r');
                    }
                }
            }
        }
        public bool Unlocked(int y, int x) => table.Unlocked(y, x);
        public bool Flagged(int y, int x) => table.Flagged(y, x);
        public Bitmap CanvasImage => render.CanvasImage;
    }
}

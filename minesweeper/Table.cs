namespace minesweeper
{
    internal class Table
    {
        private class Cell
        {
            public int value;
            public bool flagged, unlocked;
        }
        private Cell[,] cells;
        private Random random = new Random();
        public int widthmatrix, heightmatrix;
        public void CreateNew(int width, int height)
        {
            widthmatrix = width;
            heightmatrix = height;
            cells = new Cell[heightmatrix, widthmatrix];
            for (int y = 0; y < heightmatrix; y++)
            {
                for (int x = 0; x < widthmatrix; x++)
                {
                    cells[y, x] = new Cell();
                }
            }
        }
        public int Value(int y, int x) => cells[y, x].value;
        public bool Flagged(int y, int x) => cells[y, x].flagged;
        public bool Unlocked(int y, int x) => cells[y, x].unlocked;
        public void FlagModifier(int x, int y, bool flagged) => cells[y, x].flagged = flagged;
        public void Unlocker(int x, int y) => cells[y, x].unlocked = true;
        public void GenerateMines(int ax, int ay, int quantity)
        {
            while (quantity > 0)
            {
                int x = random.Next(widthmatrix);
                int y = random.Next(heightmatrix);
                if (cells[y, x].value == 0 && (x < Math.Max(ax - 1, 0) || x > Math.Min(ax + 1, widthmatrix) || y < Math.Max(ay - 1, 0) || y > Math.Min(ay + 1, heightmatrix)))
                {
                    cells[y, x].value = -1;
                    quantity--;
                }
            }
            NumberFiller();
        }
        private void NumberFiller()
        {
            for (int y = 0; y < heightmatrix; y++)
            {
                for (int x = 0; x < widthmatrix; x++)
                {
                    if (cells[y, x].value == -1)
                    {
                        bool up = y > 0;
                        if (up && cells[y - 1, x].value != -1) cells[y - 1, x].value++;
                        bool down = (y + 1) < heightmatrix;
                        if (down && cells[y + 1, x].value != -1) cells[y + 1, x].value++;
                        if (x > 0)
                        {
                            if (cells[y, x - 1].value != -1) cells[y, x - 1].value++;
                            if (up && cells[y - 1, x - 1].value != -1) cells[y - 1, x - 1].value++;
                            if (down && cells[y + 1, x - 1].value != -1) cells[y + 1, x - 1].value++;
                        }
                        if ((x + 1) < widthmatrix)
                        {
                            if (cells[y, x + 1].value != -1) cells[y, x + 1].value++;
                            if (up && cells[y - 1, x + 1].value != -1) cells[y - 1, x + 1].value++;
                            if (down && cells[y + 1, x + 1].value != -1) cells[y + 1, x + 1].value++;
                        }
                    }
                }
            }
        }
        public List<Point> Around(int x, int y)
        {
            List<Point> unlockable = new List<Point>();
            int flagcount = 0;
            bool up = y > 0;
            if (up)
            {
                if (cells[y - 1, x].flagged) flagcount++;
                else if (!cells[y - 1, x].unlocked) unlockable.Add(new Point(x, y - 1));
            }
            bool down = (y + 1) < heightmatrix;
            if (down)
            {
                if (cells[y + 1, x].flagged) flagcount++;
                else if (!cells[y + 1, x].unlocked) unlockable.Add(new Point(x, y + 1));
            }
            if (x > 0)
            {
                if (cells[y, x - 1].flagged) flagcount++;
                else if (!cells[y, x - 1].unlocked) unlockable.Add(new Point(x - 1, y));
                if (up)
                {
                    if (cells[y - 1, x - 1].flagged) flagcount++;
                    else if (!cells[y - 1, x - 1].unlocked) unlockable.Add(new Point(x - 1, y - 1));
                }
                if (down)
                {
                    if (cells[y + 1, x - 1].flagged) flagcount++;
                    else if (!cells[y + 1, x - 1].unlocked) unlockable.Add(new Point(x - 1, y + 1));
                }
            }
            if ((x + 1) < widthmatrix)
            {
                if (cells[y, x + 1].flagged) flagcount++;
                else if (!cells[y, x + 1].unlocked) unlockable.Add(new Point(x + 1, y));
                if (up)
                {
                    if (cells[y - 1, x + 1].flagged) flagcount++;
                    else if (!cells[y - 1, x + 1].unlocked) unlockable.Add(new Point(x + 1, y - 1));
                }
                if (down)
                {
                    if (cells[y + 1, x + 1].flagged) flagcount++;
                    else if (!cells[y + 1, x + 1].unlocked) unlockable.Add(new Point(x + 1, y + 1));
                }
            }
            return cells[y, x].value == flagcount ? unlockable : null;
        }
    }
}

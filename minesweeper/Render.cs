namespace minesweeper
{
    internal class Render
    {
        private const int canvaswidth = 720, canvasheight = 560;
        public int ratiowidth, ratioheight;
        private int widthmatrix, heightmatrix;
        public Render()
        {
            cache = new Cache();
            canvas = new DirectBitmap(canvaswidth, canvasheight);
        }
        private Cache cache;
        private DirectBitmap canvas;
        public void ItemDisplay(int px, int py, char name)
        {
            int[,] input = cache.GetMatrix(name);
            for (int ty = 0; ty < ratioheight; ty++)
            {
                for (int tx = 0; tx < ratiowidth; tx++)
                {
                    canvas.SetPixel(px * ratiowidth + tx, py * ratioheight + ty, input[ty, tx]);
                }
            }
        }
        public void AllDisplay()
        {
            for (int fy = 0; fy < heightmatrix; fy++)
            {
                for (int fx = 0; fx < widthmatrix; fx++)
                {
                    ItemDisplay(fx, fy, 't');
                }
            }
        }
        public void CreateNew(int widthmatrix, int heightmatrix)
        {
            this.widthmatrix = widthmatrix;
            this.heightmatrix = heightmatrix;
            ratiowidth = canvaswidth / this.widthmatrix;
            ratioheight = canvasheight / this.heightmatrix;
            cache.CacheGenerator(ratiowidth, ratioheight);
        }
        public Bitmap CanvasImage => canvas.Picture;
    }
}

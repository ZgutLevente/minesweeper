namespace minesweeper
{
    internal class Cache
    {
        public Cache() => PreLoad();
        private Dictionary<char, Image> maincache = new Dictionary<char, Image>();
        private Dictionary<char, int[,]> store = new Dictionary<char, int[,]>();
        private void PreLoad()
        {
            foreach (string item in Directory.GetFiles($@"resources"))
            {
                maincache.Add(item.Replace($@"resources\", "")[0], Image.FromFile(item));
            }
        }
        public void CacheGenerator(int ratiowidth, int ratioheight)
        {
            store.Clear();
            foreach (KeyValuePair<char, Image> item in maincache)
            {
                int[,] convertedmatrix = new int[ratioheight, ratiowidth];
                Bitmap convertedimage = new Bitmap(item.Value, new Size { Width = ratiowidth, Height = ratioheight });
                for (int y = 0; y < ratioheight; y++)
                {
                    for (int x = 0; x < ratiowidth; x++)
                    {
                        convertedmatrix[y, x] = convertedimage.GetPixel(x, y).ToArgb();
                    }
                }
                store.Add(item.Key, convertedmatrix);
            }
        }
        public int[,] GetMatrix(char name) => store[name];
    }
}

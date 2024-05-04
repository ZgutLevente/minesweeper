using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace minesweeper
{
    public class DirectBitmap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Bitmap Picture { get; private set; }
        private int[] bits;
        private GCHandle bitsHandle;
        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            bits = new int[Width * Height];
            bitsHandle = GCHandle.Alloc(bits, GCHandleType.Pinned);
            Picture = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppArgb, bitsHandle.AddrOfPinnedObject());
        }
        public void SetPixel(int x, int y, int colour) => bits[x + (y * Width)] = colour;
    }
}
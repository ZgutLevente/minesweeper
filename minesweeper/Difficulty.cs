namespace minesweeper
{
    internal class Difficulty
    {
        private class DifficultyType
        {
            public int width, height, quantity;
        }
        private readonly DifficultyType[] difficultys =
        {
            new DifficultyType { width = 10, height = 8, quantity = 10 },
            new DifficultyType { width = 18, height = 14, quantity = 40 },
            new DifficultyType { width = 24, height = 20, quantity = 99 }
        };
        private DifficultyType actualType;
        public byte ActualDifficulty { get; private set; }
        public void SetDifficulty(byte newdifficulty)
        {
            ActualDifficulty = newdifficulty;
            actualType = difficultys[newdifficulty];
        }
        public int width => actualType.width;
        public int height => actualType.height;
        public int quantity => actualType.quantity;
    }
}

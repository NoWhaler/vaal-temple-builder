namespace Grid
{
    public class GridCellModel
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public bool IsStartCell => X == GridConstants.StartCellX && Y == GridConstants.StartCellY;

        public GridCellModel(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
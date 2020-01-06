using System.Linq;

namespace World.Square
{
    public class SquareWorld
    {
        private SquareCell[] cells;

        private readonly int lowerEdgeBoundInclusive;
        private readonly int upperEdgeBoundInclusive;
        private readonly int edge;
        private readonly int offset;

        public SquareWorld(int radius)
        {
            this.edge = radius * 2 - 1;
            var cellCount = this.edge * this.edge;

            int firstX = 1 - radius;
            int firstY = 1 - radius;

            this.lowerEdgeBoundInclusive = firstX;
            this.upperEdgeBoundInclusive = -this.lowerEdgeBoundInclusive;

            this.offset = 0 - (firstY * this.edge + firstX);

            this.cells = Enumerable
                .Range(0, cellCount)
                .Select(_ => new SquareCell())
                .ToArray();
        }

        private int ToAbsoluteCoord(int x, int y)
        {
            return this.edge * y + x + this.offset;
        }

        public SquareCell GetCell(int x, int y)
        {
            if (x < this.lowerEdgeBoundInclusive || x > this.upperEdgeBoundInclusive || y < this.lowerEdgeBoundInclusive || y > this.upperEdgeBoundInclusive)
            {
                return null;
            }

            var coord = this.ToAbsoluteCoord(x, y);

            var l = this.cells.Length;
            if (coord >= 0 && coord < l)
            {
                return this.cells[coord];
            }

            return null;
        }
    }
}

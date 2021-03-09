using System.Drawing;

namespace GraphAlgorithms
{
    public class Vertex
    {
        public Pen color = new Pen(Color.Black);
        public Vertex(int x, int y, int number)
        {
            X = x;
            Y = y;
            Number = number;
        }

        public int X { get; }

        public int Y { get; }

        public int Number { get; }
    }
}

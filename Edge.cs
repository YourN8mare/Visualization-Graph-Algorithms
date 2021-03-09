using System.Drawing;

namespace GraphAlgorithms
{
    public class Edge
    {
        public Pen color = new Pen(Color.DarkGoldenrod);

        public Edge(Vertex v1, Vertex v2, string weight)
        {
            V1 = v1;
            V2 = v2;
            Weight = weight;
        }

        public Vertex V1 { get; }

        public Vertex V2 { get; }

        public string Weight { get; set; }
    }
}

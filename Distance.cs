using System.Collections.Generic;

namespace GraphAlgorithms
{
    public class Distance
    {
        List<Vertex> V = new List<Vertex>();
        List<int> dist;

        public Distance(List<Vertex> V, List<int> dist)
        {
            this.V = V;
            this.dist = new List<int>(dist);
        }

        public override string ToString()
        {
            string s = "V \t\t Dist to V \n";
            for (int i = 0; i < V.Count; i++)
            {
                if (dist[i] == int.MaxValue)
                {
                    s += $"{i + 1} \t\t ∞\n";
                    continue;
                }
                s += $"{i + 1} \t\t {dist[i]}\n";
            }
            return s;
        }
    }
}

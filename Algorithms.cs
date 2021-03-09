using System.Collections.Generic;

namespace GraphAlgorithms
{
    public class Algorithms
    {
        bool[] used;
        List<Vertex> V;
        List<Edge> E;
        List<Vertex> queue;
        List<int> order;

        public Algorithms(List<Vertex> v, List<Edge> e)
        {
            V = v;
            E = e;
        }

        public List<int> DFSShow()
        {
            used = new bool[V.Count];
            order = new List<int>();
            for (int i = 0; i < V.Count; i++)
            {
                used[i] = false;
            }
            foreach (var i in V)
                if (!used[i.Number])
                    DFS(i);
            return order;
        }

        private void DFS(Vertex v)
        {
            used[v.Number] = true;
            order.Add(v.Number);
            foreach (var i in E)
            {
                if (i.V1 == v && !used[i.V2.Number])
                    DFS(i.V2);
                if (i.V2 == v && !used[i.V1.Number])
                    DFS(i.V1);
            }
        }

        public List<int> BFSShow()
        {
            used = new bool[V.Count];
            order = new List<int>();
            for (int i = 0; i < V.Count; i++)
            {
                used[i] = false;
            }
            queue = new List<Vertex>();
            foreach (var i in V)
                if (!used[i.Number])
                    BFS(i);
            return order;
        }

        private void BFS(Vertex v)
        {
            queue.Add(v);
            for (int i = 0; i < queue.Count; i++)
            {
                used[queue[i].Number] = true;
                order.Add(queue[i].Number);
                foreach (var j in E)
                {
                    if (j.V1 == queue[i] && !used[j.V2.Number])
                        queue.Add(j.V2);
                    if (j.V2 == queue[i] && !used[j.V1.Number])
                        queue.Add(j.V1);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace GraphAlgorithms
{
    public partial class Form1 : Form
    {
        List<Vertex> V;
        List<Edge> E;
        List<int> order; // порядок выводимых вершин
        WorkSpace space;
        Algorithms alg;
        int V1 = -1, V2; // первая вершина ребра
        int weight = 1000;
        bool dijkstraChoosen = false, flowChoosen = false; // выбрали алгоритм
        int dijkstraV = -1; // вершина дейкстры
        int start = -1, finish = -1; // for flow
        int now; // вершина(шаг), которую мы сейчас показываем
        List<int> dist = new List<int>();
        List<Distance> dijkstraSteps = new List<Distance>();
        List<List<int>> wayInDFS = new List<List<int>>(); //order of vertex in way from s to f
        List<List<Edge>> maxFlowSteps = new List<List<Edge>>();
        List<int> flow = new List<int>();
        bool canNotDrawVertex = false;

        public Form1()
        {
            InitializeComponent();
            V = new List<Vertex>();
            E = new List<Edge>();
            space = new WorkSpace(Sheet.Width, Sheet.Height);
            order = new List<int>();
            button6.Visible = false;
            button5.Visible = false;
            button7.Visible = false;
            label2.Text = "Information";
        }


        public bool IsEdgeExist(Vertex v1, Vertex v2)
        {
            foreach (var i in E)
                if ((i.V1 == v1 && i.V2 == v2) || (i.V1 == v2 && i.V2 == v1))
                    return true;
            return false;
        }

        public void TurnOffAlgButtons()
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
        }

        public void TurnOnAlgButtons()
        {
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
        }

        public void ShowGraph(List<Vertex> V, List<Edge> E)
        {
            Sheet.Image = space.GetBitmap();
            //Sheet.Image = null;
            Sheet.BackColor = Color.Transparent;
            space.DrawGraph(V, E);
        }

        private void Sheet_MouseClick(object sender, MouseEventArgs e)
        {
            if (canNotDrawVertex)
            {
                MessageBox.Show("You can not draw vertex now");
                return;
            }
            if (dijkstraChoosen)
            {
                for (int i = 0; i < V.Count; i++)
                    if (Math.Pow((V[i].X - e.X), 2) + Math.Pow((V[i].Y - e.Y), 2) <= (space.R) * (space.R))
                        dijkstraV = i;
                Dijkstra();
                button5.Visible = true;
                ShowByStepDijkstra();
                return;
            }

            if (flowChoosen)
            {
                for (int i = 0; i < V.Count; i++)
                    if (Math.Pow((V[i].X - e.X), 2) + Math.Pow((V[i].Y - e.Y), 2) <= (space.R) * (space.R))
                    {
                        if (start == -1)
                        {
                            start = i;
                            V[i].color = new Pen(Color.Red);
                            ShowGraph(V, E);
                            return;
                        }
                        if (i == start)
                        {
                            MessageBox.Show("Try to choose another vertex");
                            return;
                        }
                        finish = i;
                        V[i].color = new Pen(Color.Red);
                        ShowGraph(V, E);
                    }
                MaxFlowByScaling();
                button5.Visible = true;
                ShowByStepMaxFlow();
                return;
            }

            for (int i = 0; i < V.Count; i++)
            {
                if (Math.Pow((V[i].X - e.X), 2) + Math.Pow((V[i].Y - e.Y), 2) <= (space.R) * (space.R))
                {
                    if (V1 == -1)
                    {
                        V[i].color = new Pen(Color.Red);
                        ShowGraph(V, E);
                        V1 = i;
                        return;
                    }
                    if (i == V1)
                    {
                        MessageBox.Show("Try to choose another vertex");
                        return;
                    }
                    if (IsEdgeExist(V[V1], V[i]))
                    {
                        MessageBox.Show("This edge already exists");
                        return;
                    }
                    V2 = i;
                    V[i].color = new Pen(Color.Red);
                    ShowGraph(V, E);
                    TurnOffAlgButtons();
                    button6.Visible = true;
                    return;
                }
            }
            for (int i = 0; i < V.Count; i++)
            {
                if (Math.Pow((V[i].X - e.X), 2) + Math.Pow((V[i].Y - e.Y), 2) <= 2 * (space.R) * (space.R))
                {
                    MessageBox.Show("Try to create vertex in other place");
                    return;
                }
            }
            V.Add(new Vertex(e.X, e.Y, V.Count));
            ShowGraph(V, E);
        }

        /// <summary>
        /// DFS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            canNotDrawVertex = true;
            alg = new Algorithms(V, E);
            order = alg.DFSShow();
            now = 0;
            button5.Visible = true;
            TurnOffAlgButtons();
            ShowByStepDfsBfs();
        }

        /// <summary>
        /// BFS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            canNotDrawVertex = true;
            alg = new Algorithms(V, E);
            order.Clear();
            order = alg.BFSShow();
            now = 0;
            button5.Visible = true;
            TurnOffAlgButtons();
            ShowByStepDfsBfs();
        }

        /// <summary>
        /// dijkstra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click(object sender, EventArgs e)
        {
            if (V.Count < 1)
            {
                MessageBox.Show("Please, draw at least one vertex");
                return;
            }
            canNotDrawVertex = true;
            dijkstraChoosen = true;
            MessageBox.Show("Choose a vertex");
            TurnOffAlgButtons();
        }

        /// <summary>
        /// max flow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, EventArgs e)
        {
            if (V.Count < 2)
            {
                MessageBox.Show("Please, draw at least two vertex");
                return;
            }
            canNotDrawVertex = true;
            flowChoosen = true;
            MessageBox.Show("Choose two vertex: start and finish");
            TurnOffAlgButtons();
        }

        /// <summary>
        /// next
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button5_Click(object sender, EventArgs e)
        {
            now += 1;
            if (flowChoosen)
            {
                ShowByStepMaxFlow();
                return;
            }
            if (dijkstraChoosen)
            {
                ShowByStepDijkstra();
                return;
            }
            ShowByStepDfsBfs();
        }


        /// <summary>
        /// accepted weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button6_Click(object sender, EventArgs e)
        {
            while (!int.TryParse(textBox1.Text, out weight) || weight < 0 || weight > 100)
            {
                MessageBox.Show("введите корректный вес");
                textBox1.Text = "";
                return;
            }
            E.Add(new Edge(V[V1], V[V2], textBox1.Text));
            V[V1].color = new Pen(Color.Black);
            V[V2].color = new Pen(Color.Black);
            ShowGraph(V, E);
            V1 = -1;
            weight = 1000;
            textBox1.Text = "";
            button6.Visible = false;
            TurnOnAlgButtons();
        }

        /// <summary>
        /// previous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button7_Click(object sender, EventArgs e)
        {
            if (now == 1)
                button7.Visible = false;
            if (dijkstraChoosen)
            {
                now--;
                ShowByStepDijkstra();
                return;
            }
            if (flowChoosen)
            {
                now--;
                ShowByStepMaxFlow();
                return;
            }
            V[order[now]].color = new Pen(Color.Black);
            now--;
            ShowGraph(V, E);
        }

        public void ShowByStepDfsBfs()
        {
            if (now >= V.Count())
            {
                MessageBox.Show("Algorithm completed");
                foreach (var v in V)
                {
                    v.color = new Pen(Color.Black);
                }
                ShowGraph(V, E);
                button5.Visible = false;
                button7.Visible = false;
                TurnOnAlgButtons();
                canNotDrawVertex = false;
                return;
            }
            V[order[now]].color = new Pen(Color.Red);
            ShowGraph(V, E);
            if (now != 0)
                button7.Visible = true;
        }

        public void ShowByStepDijkstra()
        {
            foreach (var v in V)
                v.color = new Pen(Color.Black);
            foreach (var edge in E)
                edge.color = new Pen(Color.DarkGoldenrod);
            if (now >= dijkstraSteps.Count)
            {
                MessageBox.Show("Algorithm completed");
                ShowGraph(V, E);
                button5.Visible = false;
                button7.Visible = false;
                dijkstraV = -1;
                dijkstraChoosen = false;
                label2.Text = "Information";
                TurnOnAlgButtons();
                canNotDrawVertex = false;
                return;
            }
            if (now > 0)
                button7.Visible = true;
            foreach (var edge in E)
                if (edge.V1 == V[order[now]] || edge.V2 == V[order[now]])
                    edge.color = new Pen(Color.Red);
            V[order[now]].color = new Pen(Color.Red);
            label2.Text = dijkstraSteps[now].ToString();
            ShowGraph(V, E);
        }

        public void ShowByStepMaxFlow()
        {
            foreach (var v in V)
                v.color = new Pen(Color.Black);
            if (now == maxFlowSteps.Count)
            {
                int sum = 0;
                for (int i = 0; i < flow.Count; i++)
                    sum += flow[i];
                MessageBox.Show($"The largest flow equals {sum}");
                return;
            }
            if (now > maxFlowSteps.Count)
            {
                MessageBox.Show("Algorithm completed");
                ShowGraph(V, E);
                button5.Visible = false;
                button7.Visible = false;
                start = -1;
                finish = -1;
                flowChoosen = false;
                TurnOnAlgButtons();
                label2.Text = "Information";
                canNotDrawVertex = true;
                return;
            }
            foreach (var edge in maxFlowSteps[now])
                edge.color = new Pen(Color.DarkGoldenrod);
            if (now > 0)
                button7.Visible = true;
            foreach (var vertex in V)
                if (wayInDFS[now].Exists(x => x == vertex.Number))
                    vertex.color = new Pen(Color.Red);
            for (int i = 0; i < E.Count; i++)
            {
                for (int j = 0; j < wayInDFS[now].Count; j++)
                {
                    if (E[i].V1.Number == wayInDFS[now][j])
                    {
                        if (j == 0)
                        {
                            if (E[i].V2.Number == wayInDFS[now][j + 1])
                                maxFlowSteps[now][i].color = new Pen(Color.Red);
                            continue;
                        }
                        if (j == wayInDFS[now].Count - 1)
                        {
                            if (E[i].V2.Number == wayInDFS[now][j - 1])
                                maxFlowSteps[now][i].color = new Pen(Color.Red);
                            continue;
                        }
                        if (E[i].V2.Number == wayInDFS[now][j - 1] || E[i].V2.Number == wayInDFS[now][j + 1])
                        {
                            maxFlowSteps[now][i].color = new Pen(Color.Red);
                            continue;
                        }
                    }
                }
            }
            label2.Text += flow[now] + "\n";
            ShowGraph(V, maxFlowSteps[now]);
        }

        public void Dijkstra()
        {
            order.Clear();
            bool[] used = new bool[V.Count];
            dijkstraSteps.Clear();
            now = 0;
            dist.Clear();
            for (int i = 0; i < V.Count; i++)
            {
                dist.Add(int.MaxValue);
                used[i] = false;
            }
            dist[dijkstraV] = 0;
            for (int i = 0; i < V.Count; i++)
            {
                int v = -1, min = int.MaxValue;
                for (int j = 0; j < V.Count; j++)
                {
                    if (!used[j] && dist[j] < min)
                    {
                        v = j;
                        min = dist[j];
                    }
                }
                if (v == -1)
                    break;
                order.Add(v);
                used[v] = true;
                foreach (var edge in E)
                {
                    if (edge.V1.Number == v)
                        dist[edge.V2.Number] = Math.Min(dist[edge.V2.Number], dist[edge.V1.Number] + int.Parse(edge.Weight));
                    if (edge.V2.Number == v)
                        dist[edge.V1.Number] = Math.Min(dist[edge.V1.Number], dist[edge.V2.Number] + int.Parse(edge.Weight));
                }
                dijkstraSteps.Add(new Distance(V, dist));
            }
        }

        public int FindNearesrtDegOf2()
        {
            int maxWeight = 0;
            foreach (var edge in E)
            {
                if (int.Parse(edge.Weight) > maxWeight)
                    maxWeight = int.Parse(edge.Weight);
            }
            int nearestDeg = 1;
            while (nearestDeg <= maxWeight)
                nearestDeg *= 2;
            return nearestDeg / 2;
        }

        public void FindWay(Vertex v, List<Edge> E1, List<Edge> E2, int scale, ref int[] parents, ref bool[] used)
        {
            used[v.Number] = true;
            for (int i = 0; i < E.Count; i++)
            {
                if (E[i].V1 == v && !used[E[i].V2.Number] && int.Parse(E1[i].Weight) >= scale)
                {
                    parents[E[i].V2.Number] = v.Number;
                    FindWay(E[i].V2, E1, E2, scale, ref parents, ref used);
                }
                if (E[i].V2 == v && !used[E[i].V1.Number] && int.Parse(E2[i].Weight) >= scale)
                {
                    parents[E[i].V1.Number] = v.Number;
                    FindWay(E[i].V1, E1, E2, scale, ref parents, ref used);
                }
            }
        }

        public void MaxFlowByScaling()
        {
            label2.Text = "Found flows:\n";
            flow.Clear();
            flow.Add(0);
            now = 0;
            maxFlowSteps.Clear();
            wayInDFS.Clear();
            int[] parents = new int[V.Count];
            bool[] used = new bool[V.Count];
            int scale = FindNearesrtDegOf2();
            List<Edge> E1 = new List<Edge>(E);
            List<Edge> E2 = new List<Edge>();
            foreach (var edge in E)
                E2.Add(new Edge(edge.V2, edge.V1, edge.Weight));
            wayInDFS.Add(new List<int>());
            maxFlowSteps.Add(new List<Edge>());
            for (int i = 0; i < E.Count; i++)
            {
                maxFlowSteps[maxFlowSteps.Count - 1].Add(new Edge(E[i].V1, E[i].V2, $"{E[i].V1.Number + 1}->{E[i].V2.Number + 1}={E1[i].Weight}" +
                    $"\n{E[i].V2.Number + 1}->{E[i].V1.Number + 1}={E2[i].Weight}"));
            }
            while (scale >= 1)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    parents[i] = -1;
                    used[i] = false;
                }
                FindWay(V[start], E1, E2, scale, ref parents, ref used);
                while (used[finish])
                {
                    flow.Add(scale);
                    wayInDFS.Add(new List<int>());
                    maxFlowSteps.Add(new List<Edge>());
                    int j = finish;
                    while (j != start)
                    {
                        wayInDFS[wayInDFS.Count - 1] = wayInDFS[wayInDFS.Count - 1].Prepend(j).ToList();
                        j = parents[j];
                    }
                    wayInDFS[wayInDFS.Count - 1] = wayInDFS[wayInDFS.Count - 1].Prepend(start).ToList();

                    for (int i = 0; i < E.Count; i++)
                    {
                        if (wayInDFS[wayInDFS.Count - 1].Exists(x => x == E[i].V1.Number))
                        {
                            if (parents[E[i].V1.Number] == E[i].V2.Number)
                            {
                                E2[i].Weight = (int.Parse(E2[i].Weight) - scale).ToString();
                                E1[i].Weight = (int.Parse(E1[i].Weight) + scale).ToString();
                            }
                            if (parents[E[i].V2.Number] == E[i].V1.Number)
                            {
                                E1[i].Weight = (int.Parse(E1[i].Weight) - scale).ToString();
                                E2[i].Weight = (int.Parse(E2[i].Weight) + scale).ToString();
                            }
                        }
                    }
                    for (int i = 0; i < E.Count; i++)
                    {
                        maxFlowSteps[maxFlowSteps.Count - 1].Add(new Edge(E[i].V1, E[i].V2, $"{E[i].V1.Number + 1}→{E[i].V2.Number + 1}={E1[i].Weight}" +
                            $"\n{E[i].V2.Number + 1}→{E[i].V1.Number + 1}={E2[i].Weight}"));
                    }

                    for (int i = 0; i < V.Count; i++)
                    {
                        parents[i] = -1;
                        used[i] = false;
                    }
                    FindWay(V[start], E1, E2, scale, ref parents, ref used);
                }
                scale /= 2;
            }
        }
    }
}

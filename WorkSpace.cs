using System.Collections.Generic;
using System.Drawing;

namespace GraphAlgorithms
{
    public class WorkSpace
    {
        Bitmap bitmap;
        Graphics gr;
        Font fo;
        PointF point;
        public readonly int R = 18;

        public WorkSpace(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            gr = Graphics.FromImage(bitmap);
            ClearSheet();
            fo = new Font("Arial", 12);
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void ClearSheet()
        {
            gr.Clear(Color.White);
        }

        public void DrawVertex(Vertex V)
        {
            gr.FillEllipse(Brushes.White, (V.X - R), (V.Y - R), 2 * R, 2 * R);
            gr.DrawEllipse(V.color, (V.X - R), (V.Y - R), 2 * R, 2 * R);
            point = new PointF(V.X - 9, V.Y - 9);
            gr.DrawString((V.Number + 1).ToString(), fo, Brushes.Black, point);
        }

        public void DrawEdge(Edge E)
        {
            gr.DrawLine(E.color, E.V1.X, E.V1.Y, E.V2.X, E.V2.Y);
            point = new PointF((E.V1.X + E.V2.X) / 2, (E.V1.Y + E.V2.Y) / 2);
            gr.DrawString(E.Weight.ToString(), fo, Brushes.White, point);
        }

        public void DrawGraph(List<Vertex> V, List<Edge> E)
        {
            ClearSheet();
            for (int i = 0; i < E.Count; i++)
                DrawEdge(E[i]);
            for (int i = 0; i < V.Count; i++)
                DrawVertex(V[i]);
        }
    }
}

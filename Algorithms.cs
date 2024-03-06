using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace Lab6Math
{
    // Чисто для результата. Можно было сделать и через матрицу смежности в выводе, но тогда и ее передавать надо - впадлу.
    public class Edge
    {
        public int Source { get; set; }
        public int Destination { get; set; }
        public int Weight { get; set; }
    }

    public class Prima
    {
        public static List<Edge> PrimMST(int[,] graph)
        {
            var result = new List<Edge>();
            int V = graph.GetLength(0);

            int[] selected = new int[V];
            Array.Fill(selected, 0);

            int no_edge = 0;
            selected[0] = 1;

            int x, y;

            while (no_edge < V - 1)
            {
                int min = int.MaxValue;
                x = 0;
                y = 0;

                for (int i = 0; i < V; i++)
                {
                    if (selected[i] != 0)
                    {
                        for (int j = 0; j < V; j++)
                        {
                            if (selected[j] == 0 && graph[i, j] != 0)
                            {
                                if (min > graph[i, j])
                                {
                                    min = graph[i, j];
                                    x = i;
                                    y = j;
                                }
                            }
                        }
                    }
                }
                result.Add(new Edge { Source = x, Destination = y, Weight = graph[x, y] });
                selected[y] = 1;
                no_edge++;
            }

            return result; 
        }
    }

    public class Kruskal
    {
        public static List<Edge> KruskalMST(int[,] graph)
        {
            var matrixSize = graph.GetLength(0);

            var result = new List<Edge>();
            var edges = new List<Edge>();
            var parent = new int[matrixSize];

            // Создаем список всех ребер в графе
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = i + 1; j < matrixSize; j++)
                {
                    if (graph[i, j] != 0)
                    {
                        edges.Add(new Edge { Source = i, Destination = j, Weight = graph[i, j] });
                    }
                }
            }

            // Сортируем все ребра по возрастанию веса
            edges = edges.OrderBy(edge => edge.Weight).ToList();

            parent = new int[matrixSize];
            for (int i = 0; i < matrixSize; i++)
                parent[i] = i;
            

            int edgeCount = 0;
            int edgeIndex = 0;

            // Находим остов, пока не добавим достаточное количество ребер
            while (edgeCount < matrixSize - 1)
            {
                Edge nextEdge = edges[edgeIndex++];

                int x = Find(parent, nextEdge.Source);
                int y = Find(parent, nextEdge.Destination);

                if (x != y)
                {
                    result.Add(nextEdge);
                    Union(parent, x, y);
                    edgeCount++;
                }
            }

            return result;
        }

        private static int Find(int[] parent, int i)
        {
            if (parent[i] != i)
            {
                parent[i] = Find(parent, parent[i]);
            }
            return parent[i];
        }

        private static void Union(int[] parent, int x, int y)
        {
            int xSet = Find(parent, x);
            int ySet = Find(parent, y);
            parent[ySet] = xSet;
        }
    }
}

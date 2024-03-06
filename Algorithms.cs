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
            int n = graph.GetLength(0);

            bool[] visited = new bool[n];
            int[] parent = new int[n];
            int[] key = new int[n];

            for (int i = 0; i < n; i++)
            {
                key[i] = int.MaxValue;
                visited[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < n - 1; count++)
            {
                int u = MinKey(key, visited);
                visited[u] = true;

                for (int v = 0; v < n; v++)
                {
                    if (graph[u, v] != 0 && !visited[v] && graph[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = graph[u, v];
                    }
                }
            }

            for (int i = 1; i < n; i++)
                result.Add(new Edge { Source = parent[i], Destination = i, Weight = graph[i, parent[i]] });

            return result;
        }

        private static int MinKey(int[] key, bool[] visited)
        {
            int min = int.MaxValue;
            int minIndex = -1;
            int n = key.Length;

            for (int v = 0; v < n; v++)
            {
                if (!visited[v] && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }

            return minIndex;
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

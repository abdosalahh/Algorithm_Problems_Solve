using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Linq.Enumerable;
using System.Security.Cryptography;

namespace Problem
{
    public static class SumOfMin
    {
        public static int CalcSumOfMinInComps(int[] valuesOfVertices, KeyValuePair<int, int>[] edges)
        {
            int len = valuesOfVertices.Length;
            List<int>[] graph = new List<int>[len];
            for (int i = 0; i < len; i++)
            {
                graph[i] = new List<int>();
            }
            foreach (KeyValuePair<int, int> edge in edges)
            {
                int u = edge.Key;
                int v = edge.Value;
                graph[u].Add(v);
                graph[v].Add(u);
            }

            //var visited = new HashSet<int>(); 
            bool[] visited = new bool[len];
            int[] color = new int[len];
            int[] comp = new int[len];
            Queue<int> queue = new Queue<int>();

            int counter = 1;
            foreach (var vertex in Range(0, len - 1))
            {
                if (!visited[vertex])
                {
                    queue.Enqueue(vertex);
                    visited[vertex] = true;
                    comp[vertex] = counter;
                    while (queue.Count > 0)
                    {
                        int current = queue.Dequeue();
                        foreach (var neighbour in graph[current])
                        {
                            if (!visited[neighbour])
                            {
                                queue.Enqueue(neighbour);
                                visited[neighbour] = true;
                                comp[neighbour] = comp[current];
                            }
                        }
                    }
                    counter++;
                }
            }
            Dictionary<int, int> DEC = new Dictionary<int, int>(200);
            for (int i = 0; i < len; i++)
            {
                if (!DEC.ContainsKey(comp[i]))
                {
                    DEC.Add(comp[i], valuesOfVertices[i]);
                }
                else
                {
                    DEC[comp[i]] = Math.Min(DEC[comp[i]], valuesOfVertices[i]);
                }
            }
            int Final_result = 0;
            foreach (var item in DEC)
            {
                Final_result += item.Value;
            }
            return Final_result;
        }
    }
}
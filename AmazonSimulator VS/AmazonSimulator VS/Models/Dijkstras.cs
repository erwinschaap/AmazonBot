using System;
using System.Collections.Generic;

namespace Models
{
    class Graph
    {
        Dictionary<Node, Dictionary<Node, int>> vertices = new Dictionary<Node, Dictionary<Node, int>>();

        public void add_vertex(Node name, Dictionary<Node, int> edges)
        {
            vertices[name] = edges;
        }

        public List<Node> shortest_path(Node start, Node finish)
        {
            var previous = new Dictionary<Node, Node>();
            var distances = new Dictionary<Node, int>();
            var nodes = new List<Node>();

            List<Node> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                Node smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<Node>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }
}
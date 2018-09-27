using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    public class World : Calculations, IObservable<Command>, IUpdatable
    {
        private List<Model3D> worldObjects = new List<Model3D>();
        private List<Node> nodes = new List<Node>();
        private Graph  graph = new Graph();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private double moveTruck = 0;
        public char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private int alphabetIndex;

        public World()
        {
            CreateNodes(4, 10, 0, 10);

            foreach (Node node in nodes)
            {
                Console.WriteLine("Node " + node.name + ": " + node.x + ", " + node.y + ", " + node.z + ", " + node.nodes.Count());
                foreach (Node node1 in node.nodes)
                {
                    Console.WriteLine(" " + node1.name);
                }
            }
            Console.WriteLine("Nodes Count :" + nodes.Count());

            foreach (Node node in graph.ShortestPath('A', 'C', nodes))
            {
                Console.WriteLine("Node Path");
                Console.WriteLine("Node " + node.name + ": " + node.x + ", " + node.y + ", " + node.z );
            }
            
            
            Truck truck = CreateTruck(0, 0, 0);
            truck.Move(0, 0, 0);
            Robot robot = CreateRobot(0, 0, 0);
            robot.Move(0, 0, 0);

        }

        private void CreateNodes(int nrOfNodesLeftAndRight, int startZPositionLeftNodes, int startZPositionRightsNodes, int laneHight)
        {
            int laneHightLeft = laneHight;
            Node n = new Node(alphabet[0], 0, 0, ((startZPositionLeftNodes - startZPositionRightsNodes) / 2));
            nodes.Add(n);

            //Nodes in left row
            for (int i = 0; i < nrOfNodesLeftAndRight; i++)
            {
                foreach (Node node in nodes)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (node.name == alphabet[j])
                        {
                            alphabetIndex = j + 1;
                        }
                    }
                }
                Node n2 = new Node(alphabet[alphabetIndex], laneHightLeft, 0, startZPositionLeftNodes);
                nodes.Add(n2);
                laneHightLeft += laneHight;
            }

            int laneHightRight = laneHight;
            //Nodes in right row
            for (int i = 0; i < nrOfNodesLeftAndRight; i++)
            {
                foreach (Node node in nodes)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (node.name == alphabet[j])
                        {
                            alphabetIndex = j + 1;
                        }
                    }
                }
                Node node2 = new Node(alphabet[alphabetIndex], laneHightRight, 0, startZPositionRightsNodes);
                nodes.Add(node2);
                laneHightRight += laneHight;
            }

            //Adding Node Connections
            for (int i = 0; i < nodes.Count(); i++)
            {
                int nrOfNodes = nodes.Count() - 1;
                if( i == 0)
                {
                    Node n3 = nodes[i];
                    List<Node> nodeConnections;
                    nodeConnections = new List<Node>();
                    nodeConnections.Add(nodes[i + 1]);
                    nodeConnections.Add(nodes[(nrOfNodes / 2) + 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 1)//Left under corner node
                {
                    Node n3 = nodes[i];
                    List<Node> nodeConnections;
                    nodeConnections = new List<Node>();
                    nodeConnections.Add(nodes[i + 1]);
                    nodeConnections.Add(nodes[0]);
                    nodeConnections.Add(nodes[i + (nrOfNodes / 2)]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == (nrOfNodes / 2) + 1)//Right under corner node
                {
                    Node n3 = nodes[i];
                    List<Node> nodeConnections = new List<Node>();
                    nodeConnections.Add(nodes[i + 1]);
                    nodeConnections.Add(nodes[0]);
                    nodeConnections.Add(nodes[i - (nrOfNodes / 2)]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == (nrOfNodes / 2))//Left upper corner node
                {
                    Node n3 = nodes[i];

                    List<Node> nodeConnections = new List<Node>();
                    nodeConnections.Add(nodes[i - 1]);
                    nodeConnections.Add(nodes[i + (nrOfNodes / 2)]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == nrOfNodes)//Right upper corner node 
                {
                    Node n3 = nodes[i];

                    List<Node> nodeConnections = new List<Node>();
                    nodeConnections.Add(nodes[i - 1]);
                    nodeConnections.Add(nodes[i - (nrOfNodes / 2)]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i < (nrOfNodes / 2) && i != 0 && i != (nrOfNodes / 2))//All nodes on the left side whom are no corners
                {
                    Node n3 = nodes[i];

                    List<Node> nodeConnections = new List<Node>();
                    nodeConnections.Add(nodes[i + 1]);
                    nodeConnections.Add(nodes[i - 1]);
                    nodeConnections.Add(nodes[i + (nrOfNodes / 2)]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i > (nrOfNodes / 2) && i != ((nrOfNodes / 2) + 1) && i != nrOfNodes)//All nodes on the right side whom are no corners
                {
                    Node n3 = nodes[i];

                    List<Node> nodeConnections = new List<Node>();
                    nodeConnections.Add(nodes[i + 1]);
                    nodeConnections.Add(nodes[i - 1]);
                    nodeConnections.Add(nodes[i - (nrOfNodes / 2)]);
                    n3.SetNodeConnection(nodeConnections);
                }
            }

            foreach(Node node in nodes)
            {
                graph.AddVertex(node);
            }
        }

        private Scaffholding CreateScaffholding(double x, double y, double z)
        {
            Scaffholding scaffholding = new Scaffholding(x, y, z, 0, 0, 0);
            worldObjects.Add(scaffholding);
            return scaffholding;
        }

        private Truck CreateTruck(double x, double y, double z)
        {
            Truck truck = new Truck(x, y, z, 0, 0, 0);
            worldObjects.Add(truck);
            return truck;
        }

        private Robot CreateRobot(double x, double y, double z)
        {
            Robot robot = new Robot(x, y, z, 0, 0, 0);
            worldObjects.Add(robot);
            return robot;
        }

        public IDisposable Subscribe(IObserver<Command> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

                SendCreationCommandsToObserver(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        private void SendCommandToObservers(Command command)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].OnNext(command);
            }
        }

        private void SendCreationCommandsToObserver(IObserver<Command> obs)
        {
            foreach (Model3D m3d in worldObjects)
            {
                obs.OnNext(new UpdateModel3DCommand(m3d));
            }

        }

        public bool Update(int tick)
        {
            for (int i = 0; i < worldObjects.Count; i++)
            {
                Model3D u = worldObjects[i];


                //MoveTruck += 0.05;
                //if (MoveTruck >=24) {
                //    MoveTruck = -4;
                //}
                if (u is IUpdatable)
                {
                    if (u is Truck)
                    {
                        u.Move(moveTruck, 0, 0);
                    }
                    bool needsCommand = ((IUpdatable)u).Update(tick);

                    if (needsCommand)
                    {
                        SendCommandToObservers(new UpdateModel3DCommand(u));
                    }
                }
            }

            return true;
        }
    }

    internal class Unsubscriber<Command> : IDisposable
    {
        private List<IObserver<Command>> _observers;
        private IObserver<Command> _observer;

        internal Unsubscriber(List<IObserver<Command>> observers, IObserver<Command> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
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
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private double moveTruck = 0;
        public char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private int alphabetIndex;

        public World()
        {
            CreateNodes(2, 10, 2, 0, 10);
            
            Truck truck = CreateTruck(0, 0, 0);
            truck.Move(0, 0, 0);
            Robot robot = CreateRobot(0, 0, 0);
            robot.Move(0, 0, 0);

            foreach (Node node in nodes)
            {
                Console.WriteLine("Node " + node.name + ": " + node.x + ", " + node.y + ", " + node.z);
            }
        }

        private void ShortestPath()
        {
            Graph g = new Graph();
            g.AddVertex('A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } });
            g.AddVertex('B', new Dictionary<char, int>() { { 'A', 7 }, { 'F', 2 } });
            g.AddVertex('C', new Dictionary<char, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 4 } });
            g.AddVertex('D', new Dictionary<char, int>() { { 'F', 8 } });
            g.AddVertex('E', new Dictionary<char, int>() { { 'H', 1 } });
            g.AddVertex('F', new Dictionary<char, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 }, { 'H', 3 } });
            g.AddVertex('G', new Dictionary<char, int>() { { 'C', 4 }, { 'F', 9 } });
            g.AddVertex('H', new Dictionary<char, int>() { { 'E', 1 }, { 'F', 3 } });
            g.ShortestPath('A', 'H', nodes).ForEach(x => Console.WriteLine(x));
        }

        private void CreateNodes(int leftRow, int startYPositionLeftNodes, int rightRow, int startYPositionRightsNodes, int laneHight)
        {
            int laneHightLeft = laneHight;
            int laneHightRight = laneHight;

            //Nodes in left row
            for (int i = 0; i < leftRow; i++)
            {
                foreach (Node n2 in nodes)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (n2.name == alphabet[j])
                        {
                            alphabetIndex = j + 1;
                        }
                    }
                    Console.WriteLine(alphabetIndex);
                }
                Node n = new Node(alphabet[alphabetIndex], laneHightLeft, startYPositionLeftNodes, 0);
                nodes.Add(n);
                laneHightLeft += laneHightLeft;
            }

            //Nodes in right row
            for (int i = 0; i < rightRow; i++)
            {
                foreach (Node n2 in nodes)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (n2.name == alphabet[j])
                        {
                            alphabetIndex = j + 1;
                        }
                    }
                    Console.WriteLine(alphabetIndex);
                }
                Node n = new Node(alphabet[alphabetIndex], laneHightRight, startYPositionRightsNodes, 0);
                nodes.Add(n);
                laneHightRight += laneHightRight;
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
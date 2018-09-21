using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    public class World : IObservable<Command>, IUpdatable
    {
        private List<Model3D> worldObjects = new List<Model3D>();
        private List<Node> nodes = new List<Node>();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private double speed = 1;

        public World()
        {
            Truck truck = CreateTruck(0, 0, 0);
            truck.Move(0, 0, 0);
            Robot robot = CreateRobot(0, 0, 0);
            robot.Move(0, 0, 0);

            CreateNode('H', 30, 0, 0);
            CreateNode('M', 0, 0, 0);

            //ShortestPath();
        }

        //private void ShortestPath()
        //{
        //    g.add_vertex('B', new Dictionary<Node, int>() { { 'A', 7 }, { 'F', 2 } });
        //    g.add_vertex('C', new Dictionary<Node, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 4 } });
        //    g.add_vertex('D', new Dictionary<Node, int>() { { 'F', 8 } });
        //    g.add_vertex('E', new Dictionary<Node, int>() { { 'H', 1 } });
        //    g.add_vertex('F', new Dictionary<Node, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 }, { 'H', 3 } });
        //    g.add_vertex('G', new Dictionary<Node, int>() { { 'C', 4 }, { 'F', 9 } });
        //    g.add_vertex('H', new Dictionary<Node, int>() { { 'E', 1 }, { 'F', 3 } });
        //    g.shortest_path('A', 'H').ForEach(x => Console.WriteLine(x));
        //}

        private void CreateNode(char name, int x, int y, int z)
        {
            Node node = new Node(name, x, y ,z);

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
                        u.Move(MoveTruck, 0, 0);
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
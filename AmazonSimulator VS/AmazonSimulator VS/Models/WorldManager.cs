using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class WorldManager
    {
        private List<Robot> robots = new List<Robot>();
        private List<Scaffholding> scaffholdings = new List<Scaffholding>();
        private List<Truck> trucks = new List<Truck>();

        public WorldManager(List<Robot> robots, List<Scaffholding> scaffholdings, List<Truck> trucks)
        {
            this.robots = robots;
            this.scaffholdings = scaffholdings;
            this.trucks = trucks;
        }

        public List<Scaffholding> GetScaffholdingList()
        {
            return this.scaffholdings;
        }

        private void SetTasks()
        {
            IRobotTask rt = new RobotMove();


        }
    }
}

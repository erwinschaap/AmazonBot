using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class RobotMove : IRobotTask
    {
        private List<Node> path;

        public RobotMove(List<Node> path)
        {
            this.path = path;
        }

        public void StartTask(Robot robot)
        {
            robot.MoveOverPath(this.path);
        }

        public bool TaskComplete(Robot robot)
        {
            return robot.x == path.Last().x && robot.z == path.Last().y;
        }
    }
}

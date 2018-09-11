using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Truck : Model3D, IUpdatable
    {
        public Truck(double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("truck", x, y, z, rotationX, rotationY, rotationZ)
        {
        }

        public override bool Update(int tick)
        {
            //

            return base.Update(tick);
        }
    }
}
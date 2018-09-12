using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Scaffholding : Model3D, IUpdatable
    {
        public Scaffholding(double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("Scaffholding", x, y, z, rotationX, rotationY, rotationZ)
        {
        }

        //public override bool Update(int tick)
        //{
        //    //

        //    return base.Update(tick);
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bouncegame
{
    class Model
    {
        //Game Props
        public int score { get; set; }
        public bool isRunning { get; set; }


        //Ball Props
        public double windowWidth { get; set; }
        public double windowHeigth { get; set; }
        public double ballLeft { get; set; }
        public double ballTop { get; set; }
        public bool jumpRight { get; set; }
        public bool jumpDown { get; set; }
        public double ballJumpDist { get; set; }
        public double speed { get; set; }
        //ballGravity
        public double velocity{ get; set; }
        public int gravity { get; set; }
        public double friction { get; set; }



        public Model()
        {
            this.isRunning = false;
            this.jumpDown = true;
            this.jumpRight = false;
            ballJumpDist = 1;
            speed = 10;

            velocity = 1;
            gravity = 1;
            friction = 0.8;
        }
    }
}

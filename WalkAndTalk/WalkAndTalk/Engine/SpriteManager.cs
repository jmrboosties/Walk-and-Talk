using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WalkAndTalk.Engine
{
    public static class SpriteManager
    {
        //Direction, Y axis
        public static int Down = 0;
        public static int Right = 1;
        public static int Left = 2;
        public static int Up = 3;

        //Action, X axis
        public static int StepOne = 0;
        public static int Idle = 1;
        public static int StepTwo = 2;
    }
}

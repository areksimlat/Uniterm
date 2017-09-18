using System;

namespace Uniterm
{
    [Serializable]
    public class Uniterm
    {
        public int id = -1;

        public string name = "";

        public string description = "";

        public string sA = "";

        public string sB = "";

        public string sOp = "";

        public string eA = "";

        public string eB = "";

        public string eOp = "";

        public char switched = ' ';

        public string fontFamily = "Arial";

        public int fontSize = 15;

        public bool modified = false;

        public bool saved = false;


        public Uniterm()
        {

        }
    }
}
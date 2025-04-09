using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathStar
{
    internal class Rohamosztagos
    {
        static int idnumber = 999;  
        string id;
        public readonly Munkas Felugyelve;
        public string Id { get { return id; }set {id = "FN" + idnumber++; } }

        public Rohamosztagos()
        {
                
        }



    }
}

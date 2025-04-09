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
        public Munkas Felugyelve { get; private set; }

        public Rohamosztagos()
        {
            id = "FN" + idnumber++;
        }

        public void Felugyel(Munkas m)
        {
            Felugyelve = m;
            m.Figyelik = true;
        }

        public void FelugyeletVege()
        {
            Felugyelve.Figyelik = false;
            Felugyelve = null;
        }

        public override string ToString() 
        {
            return $"{this.id} felügyeli: {(this.Felugyelve != null ? this.Felugyelve.Nev : "Nem felügyel")}";
        }

    }
}

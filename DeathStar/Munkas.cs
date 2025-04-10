using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathStar
{
    internal class Munkas
    {
        string nev;
        int allapot;        //100 ha kész van
        int tempo { get; set; }
        public bool Elkeszult { get { return (allapot == 100 ? true : false); } }
        public string Nev { get { return nev; } set { nev = value; } }
        public int Allapot { get { return allapot; } set { allapot = value; } }

        public bool Figyelik { get; set; }

        public Munkas(string nev, int allapot, int tempo)
        {
            this.nev = nev;
            this.allapot = allapot;
            this.tempo = tempo;
        }

        public void Lep()
        {
            //dolgozik, ezért nő az állapot
            // ha figyelik akkor gyorsabban
            allapot++;
            if (!Figyelik)
            {
                Thread.Sleep(tempo);
            }
            else
            {
                Thread.Sleep((int)Math.Round((double)tempo / 2));
            }
        }

        public override string ToString()
        {
            return $"{this.Nev} készültség: {this.Allapot}";
        }


    }
}

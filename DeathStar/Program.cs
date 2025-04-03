namespace DeathStar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        class Munkas
        {
            string nev;
            int allapot;
            int tempo { get; set; }
            bool Elkeszult { get { return (allapot == 100 ? true : false); } }
            public string Nev { get { return nev; } set { nev = value; } }
            public int Allapot { get { return allapot; } set { allapot = value; } }

            public bool Figyelik { get;set; }

            public Munkas(string nev, int allapot, int tempo)
            {
                this.nev = nev;
                this.allapot = allapot;
                this.tempo = tempo;
            }

            public void Lep()
            {
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
        class Rohamosztagos
        {

        }
    }
}
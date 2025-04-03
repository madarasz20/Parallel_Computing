using System.Threading;

namespace waaaa
{
    internal class Program
    {
        const int cel = 8;
        static void Main(string[] args)
        {
            Random rnd = new Random();

            //Megvannak a lovak
            var lovak = Enumerable.Range(1, 9).Select(x => new Lo()
            {
                Nev = "Horse" + x,
                Lovas = new Zsoke()
                {
                    Magassag = rnd.Next(150, 180),
                    Suly = rnd.Next(45, 65)
                },
                Szerencse = rnd.Next(50, 101),
            }).ToList() ;

            //minden lo kap egy taskot, ha elér a pálya végére akkor áll le
            var tasks = lovak.Select(x => new Task( ()=>
            {
                while(x.MegtettTav < cel)
                {
                    Thread.Sleep(x.LepesVarakozas);
                    x.MegtettTav++;
                }
            },TaskCreationOptions.LongRunning)).ToList();

            //mindegyik taskon ki akarom írni a lónak a tostringjét

            tasks.Add(new Task(()=> {
                while (true)
                {
                    Console.Clear();
                    foreach(Lo l in lovak)
                    {

                        if (l.MegtettTav == cel)
                        {
                            Console.WriteLine($"\t{l.Nev} célba ért");
                        }
                        else
                        {
                            Console.WriteLine(l);
                        }

                    }
                    Thread.Sleep(100);

                }
                    
            }, TaskCreationOptions.LongRunning));

            tasks.ForEach(x => x.Start());

            Console.ReadLine();
            


        }
        class Lo
        {
            public string Nev { get; set; }
            public Zsoke Lovas { get; set; }
            public double Szerencse { get; set; }
            public int MegtettTav { get; set; }

            static Random rnd = new Random();
            public int LepesVarakozas
            {
                get
                {
                    return (int)Math.Round((Lovas.Magassag * Lovas.Suly * rnd.Next(7, 10)) / 33.0);
                }
            }

            public override string ToString()
            {
                return $"{Nev} : " + "*".PadLeft(MegtettTav, '-');
            }

        }

        class Zsoke
        {
            public int Magassag {  get; set; }
            public int Suly { get; set; }
        }

        static void ThreadMethod1(int num)
        {
            /*
             * Mainbe ezt pakold be
             * 
             Thread t1 = new Thread(()=>ThreadMethod1(5));
            Thread t2 = new Thread(() => ThreadMethod1(2));

            t1.Start();
            t2.Start();


            Console.ReadLine();
             */

            Thread.Sleep(num * 1000);
            Console.WriteLine($"thread{num} started");

        }

       
    }
}
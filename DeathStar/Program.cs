using System.Xml.Linq;

namespace DeathStar
{
    internal class Program
    {
        static int LOG = 100;
        static bool vege = false;
        public static object LockObject = new object();
        static void Main(string[] args)
        {
            //beolvas xml, munkas parse
            XDocument doc = XDocument.Load("deathstarXML.xml");
            List<Munkas> l = (from x in doc.Descendants("munkas")
                              select new Munkas(
                                  x.Attribute("nev").Value,
                                  int.Parse(x.Attribute("allapot").Value),
                                  int.Parse(x.Attribute("tempo").Value)
                                  )).ToList();

            //mined munkáshoz egy task
            List<Task> t = new List<Task>();
            foreach (Munkas m in l)
            {
                t.Add(new Task(()=>
                {
                    //ha nincs még kész akkor gyorsabban dolgozik
                    while(!m.Elkeszult)
                    {
                        m.Lep();        
                    }
                }, TaskCreationOptions.LongRunning));
            }

            //rohamosztagosok létrehozása
            List<Rohamosztagos> rl = new List<Rohamosztagos>()
            {
                new Rohamosztagos(),
                new Rohamosztagos(),
                new Rohamosztagos(),
            };

            //rohamosztagos task-ok
            var rtl = rl.Select(r => new Task(() => 
            { 
                //amíg nem készült el mindegyik munkás
                while(l.Where(x => !x.Elkeszult).Count() > 0)
                {
                    //egy rohamosztagost ráállítunk egy munkásra aki nincs kész és nem figyelik és  alegrosszabbul áll
                    Munkas m = null;

                    //ez a kritikus szakasz, mert egy munkást csak egy rohamosztagos figyelhet
                    lock(LockObject)
                    {
                        m = l.Where(x => !x.Elkeszult && !x.Figyelik)
                        .OrderBy(x => x.Allapot)
                        .FirstOrDefault();
                        if (m != null)
                        {
                            r.Felugyel(m);
                        }
                    }
                    if(m != null)       //3 "nap" ig felügyeljük
                    {
                        Thread.Sleep(3000);
                        r.FelugyeletVege();
                    }
                }
            },TaskCreationOptions.LongRunning)).ToList();

            //ha mindegyik munkás elkészül vége van
            Task.WhenAll(rtl).ContinueWith(t => { vege = true; });

            // naplozas
            // az eltelt napok számát, és a munkásokat és a rohamosztagosokat figyeli
            Task naploz = new Task(() => {
                int db = 0;
                while(!vege)
                {
                    Console.Clear();
                    Console.WriteLine("{0} nap", (db / 1000) + 1);
                    foreach(Munkas m  in l)
                    {
                        Console.WriteLine(m);
                    }
                    foreach(Rohamosztagos r in rl)
                    {
                        Console.WriteLine(r);
                        Thread.Sleep(LOG);
                        db += LOG;
                    }
                }
                Console.WriteLine("Vége");
            }, TaskCreationOptions.LongRunning);

            //indítás

            naploz.Start();

            foreach(Task ta in t)
            {
                ta.Start();
            }
            foreach (Task ta in rtl)
            {
                ta.Start();
            }
            Console.ReadLine();

        }

    }
}
using System.Xml.Linq;

namespace DeathStar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            XDocument doc = XDocument.Load("deathstarXML.xml");
            List<Munkas> l = (from x in doc.Descendants("munkas")
                              select new Munkas(
                                  x.Attribute("nev").Value,
                                  int.Parse(x.Attribute("tempo").Value),
                                  int.Parse(x.Attribute("allapot").Value)
                                  )).ToList();

            List<Task> t = new List<Task>();
            foreach (Munkas m in l)
            {
                t.Add(new Task(()=>
                {
                    while(!m.Elkeszult)
                    {
                        m.Lep();
                    }
                }, TaskCreationOptions.LongRunning));
            }


            



        }

    }
}
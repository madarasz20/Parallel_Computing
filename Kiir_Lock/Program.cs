using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace Kiir_Lock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            int sum = 0;
            //object sumLock = new object();
            
            //7. Iro példányok létrehozása (lista)
            List<Iro> list = Enumerable
                .Range(0,4)
                .Select(x => new Iro(x))
                .ToList();

            // 8. taskok felvétele, logika megírása
            List<Task> ts = list
                .Select(x => new Task(() =>
                    {
                        while(!x.Vege())
                        {
                            int val = rnd.Next(5, 15);
                            Interlocked.Add(ref sum, val);
                            Thread.Sleep(10);
                            x.Kiir(val);
                            
                        }
                    }, TaskCreationOptions.LongRunning))
                .ToList();

            //9. taskok indítása
            //ts.ForEach(x => x.Start());

            //12. korrekt működés

            //13. B feladat, a számok összegének kiírása
            // kap egy új számláló szálat
            ts.Add(new Task(() =>
            {
                while(true)
                {
                    lock (Iro.lockObject) { 
                    Console.SetCursorPosition(0, 20);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"A számok összege: {sum}");

                    }
                    Thread.Sleep(20);               // azért van a lockon kívül, mert akkor nem engedi el a lockot és belassítja

                }
            }, TaskCreationOptions.LongRunning));

            ts.ForEach(x => x.Start());

            Console.ReadLine();

        }

        // 1. Iro osztály létrehoz
        class Iro
        {
            // 2. mezők felvétele
            const int CW = 80;
            int sor; int oszlop;
            ConsoleColor color;

            // 10. lockObject inicializálás, mert összeakadtak a szálak a kritikus szakaszon belül (ez a console write művelet)
            public static object lockObject = new object();

            // 3. szín lehetőségek megadása
            static ConsoleColor[] szinek = new ConsoleColor[]
            {
                ConsoleColor.Yellow, ConsoleColor.Green,
                ConsoleColor.Red, ConsoleColor.Cyan
            };

            //6. tudjuk hogy mikor ért a sor végére
            public bool Vege() { return oszlop == 2*CW; }

            //4. konstruktor
            public Iro(int idx)
            {
                sor = 2 * idx;
                color = szinek[idx % szinek.Length];
                oszlop = 0;
            }

            //5. kiiratás
            public void Kiir(int szam)
            {
                // 11. kritikus szakasz lockolása, tehát csak egy szál mehet be egyszerre
                lock (lockObject) {
                    //ternary operator (feltétel ? Igaz : Hamis)
                    Console.SetCursorPosition(oszlop % CW, (oszlop >= CW ? sor + 1 : sor));
                    oszlop++;
                    Console.ForegroundColor = color;
                    Console.Write(szam);
                }
            }

        }
    }
}
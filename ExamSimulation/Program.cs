namespace ExamSimulation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var diakok = Enumerable.Range(0, 5)
                .Select(
                    x => new Student()
                ).ToList();

            Teacher t = new Teacher();

            var tasks = diakok.Select(x => new Task(
                () =>
                {
                    x.Vizsgazik();
                }, TaskCreationOptions.LongRunning)).ToList();

            Task v = new Task(() =>
            {
                t.Vizsgaztat(diakok);
            }, TaskCreationOptions.LongRunning);

            tasks.Add(v);

            tasks.ForEach(x => x.Start());

            Console.ReadLine();


        }

        enum state { felkeszul, elkeszult, vizsgazik, hazament };

        class Student
        {
            //állapotűtmenet: felkészül -> elkészült -> vizsgázik -> hazament
            //felkészüléskor várakozik, ha elkészült akkor várakozik a tanárra
            // egyszerre egy vizsgáztató kérdezheti

            static int _id = 1;
            string name;
            public string Name { get { return name; } set { name = value; } }
            public state allapot { get; set; }
            public Teacher vizsgaztato { get; set; }
            public object monitorobj = new object();
            static Random rand = new Random();

            public Student()
            {
                this.name = "Student" + _id++;
                this.allapot = state.felkeszul;
                this.vizsgaztato = null;
            }

            public void Vizsgazik()
            {

                Console.WriteLine($"{name} felkészül a vizsgára...");
                Thread.Sleep(rand.Next(1, 7) * 1000);

                this.allapot = state.elkeszult;
                Console.WriteLine($"{name} elkészült, vizsgáztatóra vár...");

                lock (monitorobj)
                {
                    while (this.allapot != state.hazament) // csak akkor megy tovább, ha tényleg eljut idáig
                    {
                        Monitor.Wait(monitorobj);
                    }
                }

                Console.WriteLine($"{name} befejezte a vizsgát, elmegy.");
            }

        }

        class Teacher
        {
            // ha a tanuló elkészült, vizsgáztatja valamennyi ideig
            //egyszerre egy hallgatót vizsgáztathat
            static int _id = 1;
            string name;
            public string Name { get { return name; } set { name = value; } }
            public bool vizsgaztat { get; set; }
            public Student diak_vizsgaztat { get; set; }

            Random rand = new Random();

            public int levizsgaztatottak = 0;

            public Teacher()
            {
                this.name = "Teacher" + _id++;
                this.vizsgaztat = false;
                this.diak_vizsgaztat = null;
            }

            public void Vizsgaztat(List<Student> diakok)
            {
                while (true)
                {
                    if (!this.vizsgaztat)
                    {
                        foreach (Student diak in diakok)
                        {
                            if (diak.vizsgaztato == null && diak.allapot == state.elkeszult)
                            {
                                this.vizsgaztat = true;
                                this.diak_vizsgaztat = diak;
                                diak.vizsgaztato = this;
                                diak.allapot = state.vizsgazik;
                                Console.WriteLine($"{this.Name} vizsgaztatja {diak.Name}");

                                lock (diak.monitorobj)
                                {
                                    Monitor.Pulse(diak.monitorobj);
                                }

                                Thread.Sleep(rand.Next(1, 3) * 1000);

                                diak.allapot = state.hazament;
                                Console.WriteLine($"{this.diak_vizsgaztat.Name} hazament");

                                this.vizsgaztat = false;
                                this.diak_vizsgaztat = null;

                                //Console.SetCursorPosition(40, 10);
                                this.levizsgaztatottak++;
                                Console.WriteLine($"{this.name} {this.levizsgaztatottak} diakot vizsgaztatott le"); ;
                            }
                        }
                    }

                    Thread.Sleep(200);
                }

            }
        }
    }
}

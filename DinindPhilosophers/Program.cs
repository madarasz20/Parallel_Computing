namespace DinindPhilosophers
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var tasks = Enumerable.Range(0, 5).Select(
                x => new Task(() =>
                {
                    Philosopher(x);
                }, TaskCreationOptions.LongRunning));


            tasks.ToList().ForEach(t => t.Start());

            Console.ReadLine();
        }

        static void Philosopher(int i)
        {
            Random rng = new Random(i + (int)DateTime.Now.Ticks);
            while (true)
            {
                FM.Kiirt("Hungry", i);                //Hungry
                FM.pickup(i);

                FM.Kiirt("Eating", i);
                Thread.Sleep(rng.Next(1000, 3000)); // Eating

                FM.putdown(i);

                FM.Kiirt("Thinking", i);
                Thread.Sleep(rng.Next(1000, 3000)); // Thinking



                FM.putdown(i);
            }
        }
    }
}
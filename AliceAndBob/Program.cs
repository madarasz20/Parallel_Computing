namespace AliceAndBob
{
    internal partial class Program
    {
        static void Main(string[] args)
        {

            Szomszed alice = new Szomszed("Alice", "Cirmi");
            Szomszed bob = new Szomszed("Bob", "Kuvasz");

            alice.szomszed = bob;
                bob.szomszed = alice;

                Task t1 = new Task(() =>
                {
                    while (true)
                    {
                        alice.Kint();
                        alice.Bent();
                        Thread.Sleep(500);
                    }
                }, TaskCreationOptions.LongRunning);

            Task t2 = new Task(() =>
            {
                while (true)
                {
                    bob.Kint();
                    bob.Bent();
                    Thread.Sleep(500);
                }
            }, TaskCreationOptions.LongRunning);

            t1.Start();
            t2.Start();

            Console.ReadLine();
            
        }
}
}
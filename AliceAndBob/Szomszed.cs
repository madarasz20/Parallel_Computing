namespace AliceAndBob
{
    internal partial class Program
    {
        class Szomszed
    {
        string name;
        public string Name { get; set; }

        public Szomszed szomszed { get; set; }

        string allat;
        public string Allat { get; set; }

        volatile bool flag = false;                  //false nincs kint, true kint van
        static volatile string turn = "none";
        object monitorobj = new object();
        Random rnd = new Random();

        public Szomszed(string name, string allat)
        {
            this.name = name;
            this.allat = allat;
        }

        //kint van az allat
        //meg kell nézni hogy a szmoszéd álllata kint van-e,
        //ha nem akkor mehet ki,
        //ha igen akkor jelezni kell hogy ki akar menni
        public void Kint()
        {
            flag = true;
            turn = szomszed.name;

            while (szomszed.flag && turn == szomszed.name)
            {
                Thread.Sleep(10);
            }
            Console.SetCursorPosition(0, 0);
            Console.Write($"{name} állata {allat} kiment");
            Thread.Sleep(rnd.Next(1, 5) * 1000);
            Console.Clear();


        }
        //bent van az allat
        public void Bent()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write($"{name} állata {allat} bement");
            Thread.Sleep(1000);
            Console.Clear();
            flag = false;
        }

    }
}
}
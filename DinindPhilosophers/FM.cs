using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinindPhilosophers
{
    enum state { thinking, eating, hungry };
    static class FM
    {
        //philosopher Monitor
        static object[] villa = new object[] {new object(), new object() , new object() , new object() , new object() };
        static state[] allapot = new state[] { state.thinking, state.thinking, state.thinking, state.thinking, state.thinking };

        public static void pickup(int i)
        {
            allapot[i] = state.hungry;
            test(i);
            if (allapot[i] != state.eating)
            {
                lock (villa[i]) { Monitor.Wait(villa[i]); }
            }
        }

        static void test(int i)
        {
            // ha  a mellete levők nem esznek és ő éhes, elkezd enni, lockolja az i-edik villát
            if (allapot[mod5(i + 1)] != state.eating && allapot[mod5(i - 1)] != state.eating && allapot[i] == state.hungry)
            {
                allapot[i] = state.eating;
                lock (villa[i])
                    Monitor.Pulse(villa[i]);
            }
        }

        private static int mod5(int i)
        {
            int rtv = i % 5;
            return (rtv < 0 ? rtv + 5 : rtv);
        }

        public static void putdown(int i)
        {
            allapot[i] = state.thinking;
            test(mod5(i + 1));
            test(mod5(i - 1));
        }

        static object lockobject = new object();
        public static void Kiirt(string str, int i)
        {
            lock (lockobject)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(i + ":" + str.PadRight(20));
            }
        }
    }
}

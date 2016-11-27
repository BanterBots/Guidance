using System;
using System.Threading;

namespace GDApp
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ThreadStart runLeftSideDelegate = new ThreadStart(RunLeftSide);
            Thread runLeftSideThread = new Thread(runLeftSideDelegate);
            runLeftSideThread.IsBackground = false;
            runLeftSideThread.Start();

            ThreadStart runRightSideDelegate = new ThreadStart(RunRightSide);
            Thread runRightSideThread = new Thread(runRightSideDelegate);
            runRightSideThread.IsBackground = false;
            runRightSideThread.Start();

            runLeftSideThread.Join();
            runRightSideThread.Join();
        }

        static void RunLeftSide()
        {
            int index = 1;
            
            using (Main game = new GDApp.Main(index))
            {
                game.Run();
            }
        }

        static void RunRightSide()
        {
            int index = 2;
            using (Main game = new GDApp.Main(index))
            {
                game.Run();
            }
        }
    }
#endif
}


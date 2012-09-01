using System;

namespace WalkAndTalk
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameLauncher game = new GameLauncher())
            {
                game.Run();
            }
        }
    }
#endif
}


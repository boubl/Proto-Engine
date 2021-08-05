using System;

namespace Proto_Engine
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ProtoEngine())
                game.Run();
        }
    }
}

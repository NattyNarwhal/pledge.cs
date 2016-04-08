using System;

namespace OpenBSD
{
    public class PledgeTest
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Pledging...");
            Pledge.Init(string.Join(" ", args));
            Console.WriteLine("Pledged!");
        }
    }
}

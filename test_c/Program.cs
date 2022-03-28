using System;

using System.Runtime.InteropServices;

public class Tester
{
        [DllImport("libtest.so", EntryPoint="valoresCryptos")]

        static extern void valoresCryptos(string crypto, string valor);

        public static void Main(string[] args)
        {
                valoresCryptos("Bitcoin", "44.000");
        }
}
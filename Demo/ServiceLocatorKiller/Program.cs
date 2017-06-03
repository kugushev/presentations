﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLocatorKiller
{
    class Program
    {
        static void Main(string[] args)
        {
            var fixer = new ServiceLocatorFixer();
            foreach (var filename in args)
            {
                string perfect = fixer.Fix(File.ReadAllText(filename));
                using (var file = new FileStream(filename + "fixed", FileMode.Create))
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(perfect);
                }
            }
        }
    }
}

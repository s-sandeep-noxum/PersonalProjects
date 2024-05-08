using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RandomClassList randomClasses = new RandomClassList();

            RandomClass randomClass1 = new RandomClass("Something");
            RandomClass randomClass2 = new RandomClass("Something Else");


            randomClasses.Add(randomClass1);
            randomClasses.Add(randomClass2);


            Console.WriteLine($"Count{randomClasses.Count}");
            Console.ReadLine();

        }
    }
}

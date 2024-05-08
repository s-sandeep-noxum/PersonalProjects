using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListChecker
{
    public class RandomClassList : List<RandomClass>
    {
        private readonly List<RandomClass> list;

        public RandomClassList()
        {
            if(list == null)
            {
                list = new List<RandomClass>();
            }
        }

        public new void Add(RandomClass random)
        {
            list.Add(random);
        }
    }
}

using System;
using System.Linq;
using Dyno.Interfaces;

namespace Dyno.Sort
{
    public class Sorter : IStringModifier
    {
        public string Modify(string input)
        {
            return new string(input.ToCharArray().OrderBy(x => x).ToArray());
        }
    }
}

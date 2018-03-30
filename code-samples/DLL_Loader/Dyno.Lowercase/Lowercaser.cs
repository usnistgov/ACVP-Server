using System;
using Dyno.Interfaces;

namespace Dyno.Lowercase
{
    public class Lowercaser : IStringModifier
    {
        public string Modify(string input)
        {
            return input.ToLower();
        }
    }
}

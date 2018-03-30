using Dyno.Interfaces;

namespace Dyno.Capitalize
{
    public class Capitalizer : IStringModifier
    {
        public string Modify(string input)
        {
            return input.ToUpper();
        }
    }
}

using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ExtensionMethods
{
    public class MightyResultsWithExpando<T>
    {
        public List<T> Data { get; }
        public dynamic ResultsExpando { get; }

        public MightyResultsWithExpando(List<T> data, dynamic resultsExpando)
        {
            Data = data;
            ResultsExpando = resultsExpando;
        }
    }
}
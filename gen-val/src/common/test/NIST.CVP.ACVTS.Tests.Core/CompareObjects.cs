using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NIST.CVP.ACVTS.Tests.Core
{
    public static class CompareObjects
    {
        /// <summary>
        /// This is most likely not perfect, but a quick way to compare objects for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="compareObj"></param>
        /// <returns></returns>
        public static bool JsonCompare(this object obj, object compareObj)
        {
            // If the references are equal, they have to be the same object
            if (ReferenceEquals(obj, compareObj))
                return true;
            // If either is null, return false
            if ((obj == null) || (compareObj == null))
                return false;
            // If different types, return false
            if (obj.GetType() != compareObj.GetType())
                return false;

            // convert both objects to json
            var objJson = JsonConvert.SerializeObject(obj);
            var anotherJson = JsonConvert.SerializeObject(compareObj);

            return objJson == anotherJson;
        }
    }
}

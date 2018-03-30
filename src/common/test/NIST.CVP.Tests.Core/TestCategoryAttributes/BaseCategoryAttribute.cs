using System;
using NUnit.Framework;

namespace NIST.CVP.Tests.Core.TestCategoryAttributes
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class BaseCategoryAttribute : CategoryAttribute { }
}

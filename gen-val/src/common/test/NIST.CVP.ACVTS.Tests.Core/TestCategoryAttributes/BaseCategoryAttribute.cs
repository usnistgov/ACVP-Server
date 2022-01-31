using System;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class BaseCategoryAttribute : CategoryAttribute { }
}

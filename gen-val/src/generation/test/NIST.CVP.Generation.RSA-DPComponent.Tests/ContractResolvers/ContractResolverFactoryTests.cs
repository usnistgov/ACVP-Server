﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.RSA.v1_0.DpComponent;
using NIST.CVP.Generation.RSA.v1_0.DpComponent.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_DPComponent.Tests.ContractResolvers
{
    [TestFixture, UnitTest]
    public class ContractResolverFactoryTests
    {
        private readonly ContractResolverFactory _subject = new ContractResolverFactory();

        [Test]
        [TestCase(Projection.Server, typeof(ServerProjectionContractResolver<TestGroup, TestCase>))]
        [TestCase(Projection.Prompt, typeof(PromptProjectionContractResolver))]
        [TestCase(Projection.Result, typeof(ResultProjectionContractResolver))]
        public void ShouldReturnAppropriateTypeBasedOnProjection(Projection projection, Type expectedType)
        {
            var result = _subject.GetContractResolver(projection);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldThrowArgumentExceptionWithInvalidEnum()
        {
            var projection = (Projection)(-42);

            Assert.Throws(typeof(ArgumentException), () => _subject.GetContractResolver(projection));
        }
    }
}
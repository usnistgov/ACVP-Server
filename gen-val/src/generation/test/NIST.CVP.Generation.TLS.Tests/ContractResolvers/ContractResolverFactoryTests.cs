﻿using System;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.KDF_Components.v1_0.TLS;
using NIST.CVP.Generation.KDF_Components.v1_0.TLS.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TLS.Tests.ContractResolvers
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
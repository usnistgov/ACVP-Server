using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.KeyConfirmation;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.KeyConfirmation;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.KeyConfirmation;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.SafePrimes;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KasValResult> GetKasValTestAsync(KasValParameters param, bool firstAttempt = false)
        {
            if (!firstAttempt)
            {
                var keyTasks = new List<Task<IDsaKeyPair>>();
                Task<IDsaKeyPair> serverEphemeralKeyTask = null;
                if (param.ServerGenerationRequirements.GeneratesEphemeralKeyPair)
                {
                    serverEphemeralKeyTask =
                        GetKasKeyAsync(param.DomainParameters, param.KasAlgorithm, param.KasDpGeneration);
                    keyTasks.Add(serverEphemeralKeyTask);
                }

                Task<IDsaKeyPair> serverStaticKeyTask = null;
                if (param.ServerGenerationRequirements.GeneratesStaticKeyPair)
                {
                    serverStaticKeyTask =
                        GetKasKeyAsync(param.DomainParameters, param.KasAlgorithm, param.KasDpGeneration);
                    keyTasks.Add(serverStaticKeyTask);
                }

                Task<IDsaKeyPair> iutEphemeralKeyTask = null;
                if (param.IutGenerationRequirements.GeneratesEphemeralKeyPair)
                {
                    iutEphemeralKeyTask =
                        GetKasKeyAsync(param.DomainParameters, param.KasAlgorithm, param.KasDpGeneration);
                    keyTasks.Add(iutEphemeralKeyTask);
                }

                Task<IDsaKeyPair> iutStaticKeyTask = null;
                if (param.IutGenerationRequirements.GeneratesStaticKeyPair)
                {
                    iutStaticKeyTask =
                        GetKasKeyAsync(param.DomainParameters, param.KasAlgorithm, param.KasDpGeneration);
                    keyTasks.Add(iutStaticKeyTask);
                }

                await Task.WhenAll(keyTasks);

                if (serverEphemeralKeyTask != null)
                {
                    param.ServerEphemeralKey = serverEphemeralKeyTask.Result;
                }

                if (serverStaticKeyTask != null)
                {
                    param.ServerStaticKey = serverStaticKeyTask.Result;
                }

                if (iutEphemeralKeyTask != null)
                {
                    param.IutEphemeralKey = iutEphemeralKeyTask.Result;
                }

                if (iutStaticKeyTask != null)
                {
                    param.IutStaticKey = iutStaticKeyTask.Result;
                }
            }

            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasValGrain, KasValResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param,
                    LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (InitialValuesInvalidException)
            {
                // This happens if the keys don't meet a testing expectation, try again to get new keys
                return await GetKasValTestAsync(param);
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasValTestAsync(param, true);
            }
        }

        public async Task<KasAftResult> GetKasAftTestAsync(KasAftParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasAftGrain, KasAftResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasAftTestAsync(param);
            }
        }

        private async Task<IDsaKeyPair> GetKasKeyAsync(IDsaDomainParameters domainParameters, KasAlgorithm kasAlgorithm, KasDpGeneration kasDpGeneration)
        {
            switch (kasAlgorithm)
            {
                case KasAlgorithm.Ecc:
                    var eccKey = await GetEcdsaKeyAsync(new EcdsaKeyParameters()
                    {
                        Curve = KasEnumMapping.GetCurveFromKasDpGeneration(kasDpGeneration)
                    });

                    return eccKey.Key;

                case KasAlgorithm.Ffc:
                    var ffcKey = await GetDsaKeyAsync(new DsaKeyParameters()
                    {
                        DomainParameters = (FfcDomainParameters)domainParameters
                    });

                    return ffcKey.Key;
            }

            return null;
        }

        public async Task<NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasCompleteDeferredAftGrain, NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3.KasAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasTestAsync(param);
            }
        }

        public async Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasValEccCaseGrain, KasValResultEcc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasValTestEccAsync(param);
            }
        }

        public async Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasAftEccCaseGrain, KasAftResultEcc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasAftTestEccAsync(param);
            }
        }

        public async Task<NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasCompleteDeferredAftEccCaseGrain, NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasTestAsync(param);
            }
        }

        public async Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasValFfcCaseGrain, KasValResultFfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasValTestFfcAsync(param);
            }
        }

        public async Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasAftFfcCaseGrain, KasAftResultFfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasAftTestFfcAsync(param);
            }
        }

        public async Task<NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasCompleteDeferredAftFfcCaseGrain, NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasTestAsync(param);
            }
        }

        public async Task<KasValResultIfc> GetKasValTestIfcAsync(KasValParametersIfc param, KeyPair serverKey, KeyPair iutKey)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasValIfcCaseGrain, KasValResultIfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(
                    observableGrain.Grain.BeginWorkAsync,
                    param, serverKey, iutKey, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (InitialValuesInvalidException)
            {
                // This happens if the keys don't meet a testing expectation, try again to get new keys
                return await GetKasValTestIfcAsync(param);
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasValTestIfcAsync(param, serverKey, iutKey);
            }
        }

        private async Task<KasValResultIfc> GetKasValTestIfcAsync(KasValParametersIfc param)
        {
            // Get RSA keys on behalf of the server and IUT.
            var serverRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.Scheme, param.KasMode, param.ServerKeyAgreementRole, param.ServerKeyConfirmationRole,
                param.KeyConfirmationDirection);

            var keyTasks = new List<Task<RsaKeyResult>>();
            Task<RsaKeyResult> serverKeyTask = null;
            KeyPair serverKey = null;
            if (serverRequirements.GeneratesEphemeralKeyPair)
            {
                serverKeyTask = GetRsaKeyAsync(new RsaKeyParameters()
                {
                    Standard = Fips186Standard.Fips186_5,
                    Modulus = param.Modulo,
                    KeyFormat = param.PrivateKeyMode,
                    KeyMode = PrimeGenModes.RandomProbablePrimes,
                    PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                    PublicExponentMode = param.PublicExponentMode,
                    PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? new BitString(param.PublicExponent) : null
                });
                keyTasks.Add(serverKeyTask);
            }

            var iutRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.Scheme, param.KasMode, param.IutKeyAgreementRole, param.IutKeyConfirmationRole,
                param.KeyConfirmationDirection);

            Task<RsaKeyResult> iutKeyTask = null;
            KeyPair iutKey = null;
            if (iutRequirements.GeneratesEphemeralKeyPair)
            {
                iutKeyTask = GetRsaKeyAsync(new RsaKeyParameters()
                {
                    Standard = Fips186Standard.Fips186_5,
                    Modulus = param.Modulo,
                    KeyFormat = param.PrivateKeyMode,
                    KeyMode = PrimeGenModes.RandomProbablePrimes,
                    PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                    PublicExponentMode = param.PublicExponentMode,
                    PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? new BitString(param.PublicExponent) : null
                });
                keyTasks.Add(iutKeyTask);
            }

            await Task.WhenAll(keyTasks);

            if (serverKeyTask != null)
            {
                serverKey = serverKeyTask.Result.Key;
            }

            if (iutKeyTask != null)
            {
                iutKey = iutKeyTask.Result.Key;
            }

            return await GetKasValTestIfcAsync(param, serverKey, iutKey);
        }

        public async Task<KasAftResultIfc> GetKasAftTestIfcAsync(KasAftParametersIfc param, KeyPair serverKey, KeyPair iutKey)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasAftIfcCaseGrain, KasAftResultIfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, serverKey, iutKey, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasAftTestIfcAsync(param, serverKey, iutKey);
            }
        }

        public async Task<NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersIfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasCompleteDeferredAftIfcCaseGrain, NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2.KasAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasTestAsync(param);
            }
        }

        public async Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasEccComponentCaseGrain, KasEccComponentResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasEccComponentTestAsync(param);
            }
        }

        public async Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKasEccComponentCompleteDeferredCaseGrain, KasEccComponentDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasComponentTestAsync(param);
            }
        }

        public async Task<SafePrimesKeyVerResult> GetSafePrimesKeyVerTestAsync(SafePrimesKeyVerParameters param)
        {
            try
            {
                var observableGrain = await GetObserverGrain<IObserverSafePrimesKeyVerGrain, SafePrimesKeyVerResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetSafePrimesKeyVerTestAsync(param);
            }
        }

        public async Task<KasSscAftResult> GetKasSscAftTestAsync(KasSscAftParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasSscAftGrain, KasSscAftResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasSscAftTestAsync(param);
            }
        }

        public async Task<KasSscAftDeferredResult> CompleteDeferredKasSscAftTestAsync(KasSscAftDeferredParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasSscCompleteDeferredAftGrain, KasSscAftDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasSscAftTestAsync(param);
            }
        }

        public async Task<KasSscValResult> GetKasSscValTestAsync(KasSscValParameters param, bool firstAttempt = false)
        {
            if (!firstAttempt)
            {
                var keyTasks = new List<Task<IDsaKeyPair>>();
                Task<IDsaKeyPair> serverEphemeralKeyTask = null;
                if (param.ServerGenerationRequirements.GeneratesEphemeralKeyPair)
                {
                    serverEphemeralKeyTask =
                        GetKasKeyAsync(param.DomainParameters, param.KasAlgorithm, param.KasDpGeneration);
                    keyTasks.Add(serverEphemeralKeyTask);
                }

                Task<IDsaKeyPair> serverStaticKeyTask = null;
                if (param.ServerGenerationRequirements.GeneratesStaticKeyPair)
                {
                    serverStaticKeyTask =
                        GetKasKeyAsync(param.DomainParameters, param.KasAlgorithm, param.KasDpGeneration);
                    keyTasks.Add(serverStaticKeyTask);
                }

                Task<IDsaKeyPair> iutEphemeralKeyTask = null;
                if (param.IutGenerationRequirements.GeneratesEphemeralKeyPair)
                {
                    iutEphemeralKeyTask =
                        GetKasKeyAsync(param.DomainParameters, param.KasAlgorithm, param.KasDpGeneration);
                    keyTasks.Add(iutEphemeralKeyTask);
                }

                Task<IDsaKeyPair> iutStaticKeyTask = null;
                if (param.IutGenerationRequirements.GeneratesStaticKeyPair)
                {
                    iutStaticKeyTask =
                        GetKasKeyAsync(param.DomainParameters, param.KasAlgorithm, param.KasDpGeneration);
                    keyTasks.Add(iutStaticKeyTask);
                }

                await Task.WhenAll(keyTasks);

                if (serverEphemeralKeyTask != null)
                {
                    param.ServerEphemeralKey = serverEphemeralKeyTask.Result;
                }

                if (serverStaticKeyTask != null)
                {
                    param.ServerStaticKey = serverStaticKeyTask.Result;
                }

                if (iutEphemeralKeyTask != null)
                {
                    param.IutEphemeralKey = iutEphemeralKeyTask.Result;
                }

                if (iutStaticKeyTask != null)
                {
                    param.IutStaticKey = iutStaticKeyTask.Result;
                }
            }

            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasSscValGrain, KasSscValResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param,
                    LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (InitialValuesInvalidException)
            {
                // This happens if the keys don't meet a testing expectation, try again to get new keys
                return await GetKasSscValTestAsync(param);
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasSscValTestAsync(param, true);
            }
        }

        public async Task<KasSscAftResultIfc> GetKasIfcSscAftTestAsync(KasSscAftParametersIfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasSscAftIfcCaseGrain, KasSscAftResultIfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (InitialValuesInvalidException)
            {
                // This happens if the keys don't meet a testing expectation, try again to get new keys
                return await GetKasIfcSscAftTestAsync(param);
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasIfcSscAftTestAsync(param);
            }
        }

        public async Task<KasSscAftDeferredResultIfc> CompleteDeferredKasIfcSscAftTestAsync(KasSscAftDeferredParametersIfc param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasSscCompleteDeferredAftIfcCaseGrain, KasSscAftDeferredResultIfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKasIfcSscAftTestAsync(param);
            }
        }

        public async Task<KasSscValResultIfc> GetKasIfcSscValTestAsync(KasSscValParametersIfc param, bool firstAttempt = false)
        {
            if (!firstAttempt)
            {
                var keyTasks = new List<Task<RsaKeyResult>>();
                Task<RsaKeyResult> serverKeyTask = null;
                if (param.ServerGenerationRequirements.GeneratesEphemeralKeyPair)
                {
                    serverKeyTask = GetRsaKeyAsync(new RsaKeyParameters()
                    {
                        Standard = Fips186Standard.Fips186_5,
                        Modulus = param.Modulo,
                        KeyFormat = param.PrivateKeyMode,
                        KeyMode = PrimeGenModes.RandomProbablePrimes,
                        PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                        PublicExponentMode = param.PublicExponentMode,
                        PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? new BitString(param.PublicExponent) : null
                    });
                    keyTasks.Add(serverKeyTask);
                }

                Task<RsaKeyResult> iutKeyTask = null;
                if (param.IutGenerationRequirements.GeneratesEphemeralKeyPair)
                {
                    iutKeyTask = GetRsaKeyAsync(new RsaKeyParameters()
                    {
                        Standard = Fips186Standard.Fips186_5,
                        Modulus = param.Modulo,
                        KeyFormat = param.PrivateKeyMode,
                        KeyMode = PrimeGenModes.RandomProbablePrimes,
                        PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                        PublicExponentMode = param.PublicExponentMode,
                        PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? new BitString(param.PublicExponent) : null
                    });
                    keyTasks.Add(iutKeyTask);
                }

                await Task.WhenAll(keyTasks);

                if (serverKeyTask != null)
                {
                    param.ServerKeyPair = serverKeyTask.Result.Key;
                }

                if (iutKeyTask != null)
                {
                    param.IutKeyPair = iutKeyTask.Result.Key;
                }
            }

            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKasSscValIfcCaseGrain, KasSscValResultIfc>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (InitialValuesInvalidException)
            {
                // This happens if the keys don't meet a testing expectation, try again to get new keys
                return await GetKasIfcSscValTestAsync(param);
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasIfcSscValTestAsync(param, true);
            }
        }

        public async Task<KasKcAftResult> GetKasKcAftTestAsync(KasKcAftParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleKasKcAftCaseGrain, KasKcAftResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKasKcAftTestAsync(param);
            }
        }

        public async Task<KdaAftOneStepResult> GetKdaAftOneStepTestAsync(KdaAftOneStepParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKdaAftOneStepCaseGrain, KdaAftOneStepResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKdaAftOneStepTestAsync(param);
            }
        }

        public async Task<KdaValOneStepResult> GetKdaValOneStepTestAsync(KdaValOneStepParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKdaValOneStepCaseGrain, KdaValOneStepResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKdaValOneStepTestAsync(param);
            }
        }

        public async Task<KdaAftOneStepNoCounterResult> GetKdaAftOneStepNoCounterTestAsync(KdaAftOneStepNoCounterParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKdaAftOneStepNoCounterCaseGrain, KdaAftOneStepNoCounterResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKdaAftOneStepNoCounterTestAsync(param);
            }
        }

        public async Task<KdaValOneStepNoCounterResult> GetKdaValOneStepNoCounterTestAsync(KdaValOneStepNoCounterParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKdaValOneStepNoCounterCaseGrain, KdaValOneStepNoCounterResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKdaValOneStepNoCounterTestAsync(param);
            }
        }

        public async Task<KdaAftTwoStepResult> GetKdaAftTwoStepTestAsync(KdaAftTwoStepParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKdaAftTwoStepCaseGrain, KdaAftTwoStepResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKdaAftTwoStepTestAsync(param);
            }
        }

        public async Task<KdaValTwoStepResult> GetKdaValTwoStepTestAsync(KdaValTwoStepParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKdaValTwoStepCaseGrain, KdaValTwoStepResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKdaValTwoStepTestAsync(param);
            }
        }

        public async Task<KdaAftHkdfResult> GetKdaAftHkdfTestAsync(KdaAftHkdfParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKdaAftHkdfCaseGrain, KdaAftHkdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKdaAftHkdfTestAsync(param);
            }
        }

        public async Task<KdaValHkdfResult> GetKdaValHkdfTestAsync(KdaValHkdfParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverKdaValHkdfCaseGrain, KdaValHkdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKdaValHkdfTestAsync(param);
            }
        }

        public virtual async Task<DsaKeyResult> GetSafePrimeKeyAsync(SafePrimesKeyGenParameters param)
        {
            return await GetDsaKeyAsync(new DsaKeyParameters()
            {
                DomainParameters = SafePrimesFactory.GetDomainParameters(param.SafePrime)
            });
        }
    }
}

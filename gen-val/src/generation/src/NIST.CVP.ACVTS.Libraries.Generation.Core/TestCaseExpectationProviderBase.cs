using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core;

public abstract class TestCaseExpectationProviderBase<T>
    where T : struct, Enum
{
    private ConcurrentQueue<T> _expectationReasons;
    public int ExpectationCount => _expectationReasons.Count;
    private bool _reasonsLoaded;

    protected void LoadExpectationReasons(List<T> expectationReasons)
    {
        _expectationReasons = new ConcurrentQueue<T>(expectationReasons.Shuffle());
        _reasonsLoaded = true;
    }
    
    public T GetRandomReason()
    {
        if (!_reasonsLoaded)
        {
            throw new IndexOutOfRangeException($"No {nameof(T)} loaded");
        }

        if (_expectationReasons.TryDequeue(out var reason))
        {
            return reason;
        }

        throw new IndexOutOfRangeException($"No {nameof(T)} remaining to pull");
    }
}

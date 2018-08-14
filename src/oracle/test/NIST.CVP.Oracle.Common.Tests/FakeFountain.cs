namespace NIST.CVP.Common.Oracle.Tests
{
    //public class FakeFountain : IFountain<int>
    //{
    //    private readonly Random _rand;
    //    private bool _isOperating = false;
    //    private int _fillTime;
    //    private int _requestTime;
    //    private readonly double _processingSkew;    // Higher value means faster machine

    //    public FakeFountain(double processingSkew)
    //    {
    //        _rand = new Random();
    //        _processingSkew = processingSkew;
    //    }

    //    public void FillPool(Pool<int> pool)
    //    {
    //        pool.AddWater(_rand.Next(0, 10));
    //    }

    //    public void GenerateAndFill(Pool<int> pool, int currentTime)
    //    {
    //        // If the fountain is available, then set the fill time
    //        if (!_isOperating)
    //        {
    //            if (pool.WaterLevelPercent >= 1)
    //            {
    //                return;
    //            }

    //            _isOperating = true;
    //            _requestTime = currentTime;

    //            // Generate some random fill time based on the expected value
    //            _fillTime = (int)((_rand.NextDouble() - _processingSkew) * .5 * pool.ExpectedFillTime) + pool.ExpectedFillTime;
    //        }

    //        // If the fill time has elapsed, then fill the pool
    //        if (currentTime > _requestTime + _fillTime)
    //        {
    //            FillPool(pool);
    //            _isOperating = false;
    //        }
    //    }
    //}
}

using NUnit.Framework;

namespace NIST.CVP.Common.Oracle.Tests
{
    [TestFixture]
    public class PoolSimulator
    {
        //private Pool<int> _pool;
        //private List<FakeFountain> _fountains;
        //private Random _rand;

        //private const double LOWER_RATIO = .20;
        //private const double UPPER_RATIO = .95;
        //private const double GROWTH_RATIO = .01;
        //private const double SHRINK_RATIO = .05;

        //[SetUp]
        //public void SetUp()
        //{
        //    _pool = new Pool<int>(100, 1000);
        //    _fountains = new List<FakeFountain>
        //    {
        //        new FakeFountain(0.20),
        //        new FakeFountain(0.75),
        //        new FakeFountain(0.10)
        //    };
        //    _rand = new Random();
        //}

        //[Test]
        //public void SimpleSimulation()
        //{
        //    var curTime = 0;
        //    var epoch = 500;
        //    var maxTime = epoch * 200;
        //    var averageValuesToGet = 250;

        //    var twentyPercentLower = (int)(averageValuesToGet - (averageValuesToGet * .20));
        //    var twentyPercentHigher = (int)(averageValuesToGet + (averageValuesToGet * .20));

        //    while (curTime < maxTime)
        //    {
        //        _fountains.ForEach(f => f.GenerateAndFill(_pool, curTime));

        //        if (curTime % (epoch/5) == 0)
        //        {
        //            var amountToGet = _rand.Next(twentyPercentLower, twentyPercentHigher);
        //            var values = _pool.GetNext(amountToGet);
        //            Assert.AreEqual(amountToGet, values.Count());
        //        }

        //        if (curTime % (epoch/2) == 1)
        //        {
        //            Console.WriteLine($"{_pool.WaterLevel}, {_fountains.Count}");
        //        }

        //        if (curTime % (epoch/10) == 2)
        //        {
        //            AdaptPool();
        //        }

        //        curTime++;
        //    }
        //}

        //// Goal is to have pools sit at around 80% capacity while being constantly depleted.
        //private void AdaptPool()
        //{
        //    // If there isn't enough water in the pool,
        //    //  then the pool is being depleted too quickly,
        //    //  so grow the pool in response and let it fill up.
        //    // Quickly queue up new jobs to fill in the missing water.
        //    if (_pool.WaterLevelPercent < LOWER_RATIO)
        //    {
        //        _fountains.Add(new FakeFountain(_rand.NextDouble()));
        //        return;
        //    }

        //    // If there is too much water in the pool,
        //    //  then shrink the pool a bit so no new jobs
        //    //  get queue up and time can be spent elsewhere.
        //    // Extra values can sit in the pool and deplete naturally.
        //    if (_pool.WaterLevelPercent > UPPER_RATIO)
        //    {
        //        if (_fountains.Count > 0)
        //        {
        //            _fountains.RemoveAt(0);
        //        }
        //        return;
        //    }
        //}
    }
}

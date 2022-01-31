using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Math;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS
{
    [TestFixture]
    public class TestGroupTests
    {
        public interface IFoo
        {
            int Id { get; }
        }

        public class Foo : IFoo, IEquatable<Foo>
        {
            public int Id => 1;
            public int SomePropertyForFoo { get; set; }
            public string Doot { get; set; }

            public bool Equals(Foo other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return SomePropertyForFoo == other.SomePropertyForFoo && Doot == other.Doot;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Foo)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(SomePropertyForFoo, Doot);
            }
        }

        public class Bar : IFoo, IEquatable<Bar>
        {
            public int Id => 2;
            public int SomePropertyForBar { get; set; }

            public bool Equals(Bar other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return SomePropertyForBar == other.SomePropertyForBar;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Bar)obj);
            }

            public override int GetHashCode()
            {
                return SomePropertyForBar;
            }
        }

        private static IEnumerable<object> _testData = new List<object>()
        {
            new object[]
            {
                "equal just foos",
                true,
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 5,
                    }
                },
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 5,
                    }
                },
            },
            new object[]
            {
                "not equal just foos",
                false,
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 5,
                    },
                },
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 6,
                    },
                },
            },
            new object[]
            {
                "equal foo and bar",
                true,
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 5,
                    },
                    new Bar()
                    {
                        SomePropertyForBar = 1
                    },
                },
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 5,
                    },
                    new Bar()
                    {
                        SomePropertyForBar = 1
                    }
                },
            },
            new object[]
            {
                "not equal foo and bar",
                false,
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 5,
                    },
                    new Bar()
                    {
                        SomePropertyForBar = 1
                    },
                },
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 6,
                    },
                    new Bar()
                    {
                    }
                },
            },
            new object[]
            {
                "equal foo and bar different order",
                false,
                new List<IFoo>()
                {
                    new Foo()
                    {
                        SomePropertyForFoo = 5,
                    },
                    new Bar()
                    {
                        SomePropertyForBar = 1
                    },
                },
                new List<IFoo>()
                {
                    new Bar()
                    {
                    },
                    new Foo()
                    {
                        SomePropertyForFoo = 6,
                    },
                },
            },
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldSequenceEqualAppropriately(string label, bool shouldEqual, IEnumerable<IFoo> listA, IEnumerable<IFoo> listB)
        {
            Assert.AreEqual(shouldEqual, listA.SequenceEqual(listB), label);
        }

        [Test]
        public void ShouldHaveSeparateHashCodesForDifferentCollections()
        {
            var list1 = new List<IFoo>()
            {
                new Foo()
                {
                    Doot = "test",
                },
            };
            var matchList1 = new List<IFoo>()
            {
                new Foo()
                {
                    Doot = "test",
                },
            };
            var list2 = new List<IFoo>()
            {
                new Foo()
                {
                    Doot = "test",
                },
                new Bar()
                {
                    SomePropertyForBar = 42,
                },
            };
            var matchList2 = new List<IFoo>()
            {
                new Foo()
                {
                    Doot = "test",
                },
                new Bar()
                {
                    SomePropertyForBar = 42,
                },
            };
            var list3 = new List<IFoo>()
            {
                new Foo()
                {
                    Doot = "test",
                },
                new Bar()
                {
                    SomePropertyForBar = 42,
                },
                new Bar()
                {
                    SomePropertyForBar = 22,
                },
            };

            Assert.IsTrue(list1.SequenceEqual(matchList1), "list1 equal sequences");
            Assert.IsTrue(list2.SequenceEqual(matchList2), "list2 equal sequences");

            Assert.IsFalse(list1.SequenceEqual(list2), "list1 and list2 different sequences");
            Assert.IsFalse(list1.SequenceEqual(list3), "list1 and list3 different sequences");
            Assert.IsFalse(list2.SequenceEqual(list3), "list2 and list3 different sequences");

            var dict = new Dictionary<List<IFoo>, string>();
            dict.TryAdd(list1, "1");
            dict.TryAdd(matchList1, "1");
            dict.TryAdd(list2, "2");
            dict.TryAdd(matchList2, "2");
            dict.TryAdd(list3, "3");

            /*
			 * This is unfortunate... even though the sequences are equal the gethashcode (which dictionary and hashset rely on)
			 * return different hashcodes for otherwise equal sets.
			 * Because of that it can't be relied on to determine "if a sequence already exists within the dictionary" without
			 * doing a full enumeration of the dictionary each time and doing a sequence comparison per iteration.
			 */
            // Assert.AreEqual(3, dict.Count);

            var shuffleQueues = new List<(ShuffleQueue<IFoo> queue, int occurences)>();

            AddOrIncrementShuffleQueueOnDictionary(shuffleQueues, list1);
            AddOrIncrementShuffleQueueOnDictionary(shuffleQueues, matchList1);
            AddOrIncrementShuffleQueueOnDictionary(shuffleQueues, list2);
            AddOrIncrementShuffleQueueOnDictionary(shuffleQueues, matchList2);
            AddOrIncrementShuffleQueueOnDictionary(shuffleQueues, list3);

            // This doesn't work either... order can't be guaranteed in a list so sequence equals will not necessarily return true.
            //Assert.AreEqual(3, shuffleQueues.Count, "shuffle queue list");
        }

        private void AddOrIncrementShuffleQueueOnDictionary(
            List<(ShuffleQueue<IFoo> queue, int occurences)> list,
            List<IFoo> collectionToAddOrUpdateInList)
        {
            collectionToAddOrUpdateInList = collectionToAddOrUpdateInList.OrderBy(ob => ob.Id).ToList();

            bool found = false;

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].queue.OriginalList.SequenceEqual(collectionToAddOrUpdateInList))
                {
                    var item = list[i];

                    found = true;
                    item.occurences++;
                    break;
                }
            }

            if (!found)
            {
                list.Add((new ShuffleQueue<IFoo>(collectionToAddOrUpdateInList), 1));
            }
        }
    }
}

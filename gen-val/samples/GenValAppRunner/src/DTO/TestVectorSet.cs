using System.Collections.Generic;
namespace GenValAppRunner.DTO
{
    public class TestVectorSet
    {
        public int VsId { get; set; }
        public string Algorithm { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; }
    }

    public class TestGroup
    {
        public int TgId { get; set; }
        public string TestType { get; set; }
        public string InternalTestType { get; set; }
        public string Direction { get; set; }
        public int KeyLen { get; set; }
        public List<TestCase> Tests { get; set; }
    }

    public class TestCase
    {
        public int TcId { get; set; }
        public string Iv { get; set; }
        public string Pt { get; set; }
        public string Key { get; set; }
        public string Ct { get; set; }
    }
}
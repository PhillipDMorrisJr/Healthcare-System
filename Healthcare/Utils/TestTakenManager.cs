using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class TestTakenManager
    {
        public static TestTaken CurrentTestTaken;

        public static List<TestTaken> TestsTaken = TestTakenDAL.GetTestTaken();

        public static List<TestTaken> GetRefreshedTestsTaken()
        {
            TestsTaken = TestTakenDAL.GetTestTaken();
            return TestsTaken;
        }
    }
}
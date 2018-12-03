using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class TestManager
    {
        public static readonly List<Test> Tests = TestDAL.GetTests();
    }
}
using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class TestResultManager
    {
        public static TestResult CurrentTestResult;

        public static List<TestResult> Results = TestResultDAL.GetTestResults();

        public static List<TestResult> GetRefresheResults()
        {
            Results = TestResultDAL.GetTestResults();
            return Results;
        }
    }
}
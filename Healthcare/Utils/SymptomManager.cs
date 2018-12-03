using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class SymptomManager
    {
        public static readonly List<Symptom> Symptoms = SymptomDAL.GetSymptoms();
    }
}
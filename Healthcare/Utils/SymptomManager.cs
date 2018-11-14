using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class SymptomManager
    {
        public static readonly List<Symptom> Symptoms = SymptomDAL.GetSymptoms();
    }
}

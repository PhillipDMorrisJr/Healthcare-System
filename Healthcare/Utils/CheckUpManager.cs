﻿using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class CheckUpManager
    {
        public static List<CheckUp> Checkups = CheckUpDAL.GetCheckups();

        public static CheckUp CurrentCheckUp;

        /// <summary>
        ///     Executes check up with the specified details.
        /// </summary>
        /// <param name="details">The check up details.</param>
        /// <returns></returns>
        public static CheckUp Execute(CheckUp details)
        {
            return CheckUpDAL.AddCheckUp(details);
        }

        public static List<CheckUp> GetRefreshedCheckUps()
        {
            Checkups = CheckUpDAL.GetCheckups();
            return Checkups;
        }
    }
}
using System.Collections.Generic;
using Healthcare.DAL;
using Healthcare.Model;

namespace Healthcare.Utils
{
    public static class PatientManager
    {
        public static Patient CurrentPatient;

        public static readonly List<Address> Addresses = PatientDAL.GetAddresses();

        public static Address GetAddressById(int id)
        {
            Address foundAddress = null;

            if (Addresses == null) return null;

            foreach (var address in Addresses)
                if (address.AddressId == id)
                    foundAddress = address;

            return foundAddress;
        }
    }
}
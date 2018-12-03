using System;
using System.Collections.Generic;
using Healthcare.Model;
using Healthcare.Utils;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class PatientDAL
    {
        /// <summary>
        ///     Gets the patients.
        /// </summary>
        /// <returns></returns>
        public static List<Patient> GetPatients()
        {
            var patients = new List<Patient>();
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from patients";
                    using (var cmd = new MySqlCommand(selectQuery, conn))
                    {
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var id = reader.GetInt32((int) Attributes.PatientId);
                            var ssn = reader.GetInt32((int) Attributes.PatientSsn);
                            var fname = reader.GetString((int) Attributes.FirstName);
                            var lname = reader.GetString((int) Attributes.LastName);
                            var phone = reader.GetString((int) Attributes.Phone);
                            var bdate = reader.GetDateTime((int) Attributes.BirthDate);
                            var addressId = reader.GetInt32((int) Attributes.AddressId);
                            var gender = reader.GetString((int) Attributes.Gender);

                            var patient = new Patient(ssn, fname, lname, phone, bdate, gender, addressId) {Id = id};
                            patients.Add(patient);
                        }

                        conn.Close();
                        return patients;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        /// <summary>
        ///     Gets the addresses.
        /// </summary>
        /// <returns></returns>
        public static List<Address> GetAddresses()
        {
            var addresses = new List<Address>();
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from address";
                    using (var cmd = new MySqlCommand(selectQuery, conn))
                    {
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var id = reader.GetInt32((int) AddressAttributes.AddressId);
                            var street = reader.GetString((int) AddressAttributes.Street);
                            var zip = reader.GetInt32((int) AddressAttributes.Zip);
                            var state = reader.GetString((int) AddressAttributes.State);

                            var address = new Address(street, state, zip) {AddressId = id};
                            addresses.Add(address);
                        }

                        conn.Close();
                        return addresses;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        /// <summary>
        ///     Adds the patient.
        /// </summary>
        /// <param name="ssn">The ssn.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="address">The address.</param>
        /// <returns>The patient created by the database</returns>
        public static Patient AddPatient(int ssn, string firstName, string lastName, string phoneNumber, DateTime dob,
            string gender, string address)
        {
            try
            {
                var street = address.Split(",")[0].Trim();
                var state = address.Split(",")[1].Trim();
                var zip = Convert.ToInt32(address.Split(",")[2].Trim());

                int addressId;

                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertAddressQuery =
                        "INSERT INTO `address` (`street`, `zip`, `state`) VALUE (@street, @zip, @state)";

                    using (var cmd1 = new MySqlCommand(insertAddressQuery, conn))
                    {
                        cmd1.Parameters.AddWithValue("@street", street);
                        cmd1.Parameters.AddWithValue("@state", state);
                        cmd1.Parameters.AddWithValue("@zip", zip);
                        cmd1.ExecuteNonQuery();
                    }

                    var selectAddressIDQuery = "select LAST_INSERT_ID()";

                    using (var cmd2 = new MySqlCommand(selectAddressIDQuery, conn))
                    {
                        var lastIndexReader = cmd2.ExecuteReader();
                        lastIndexReader.Read();
                        addressId = lastIndexReader.GetInt32((int) AddressAttributes.AddressId);
                        var newAddress = new Address(street, state, zip) {AddressId = addressId};
                        PatientManager.Addresses.Add(newAddress);
                    }

                    conn.Close();
                }

                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `patients` (`ssn`, `firstName`, `lastName`, `birthDate`, `gender`, `phoneNumber`, `addressID`) VALUES (@ssn, @firstName, @lastName, @dob, @gender, @phoneNumber, @addressId)";
                    using (var cmd3 = new MySqlCommand(insertQuery, conn))
                    {
                        cmd3.Parameters.AddWithValue("@ssn", ssn);
                        cmd3.Parameters.AddWithValue("@firstName", firstName);
                        cmd3.Parameters.AddWithValue("@lastName", lastName);
                        cmd3.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                        cmd3.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        cmd3.Parameters.AddWithValue("@addressId", addressId);
                        cmd3.Parameters.AddWithValue("@gender", gender);
                        cmd3.ExecuteNonQuery();
                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (var cmd4 = new MySqlCommand(selectQuery, conn))
                    {
                        var lastIndexReader = cmd4.ExecuteReader();
                        lastIndexReader.Read();
                        var id = lastIndexReader.GetInt32((int) Attributes.PatientId);
                        conn.Close();
                        return new Patient(ssn, firstName, lastName, phoneNumber, dob, gender, addressId) {Id = id};
                    }
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        public static List<Patient> SelectPatientsByName(string fname, string lname)
        {
            var patients = new List<Patient>();
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from patients WHERE firstName = @firstName AND lastName = @lastName";
                    using (var cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", fname);
                        cmd.Parameters.AddWithValue("@lastName", lname);
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var id = reader.GetInt32((int) Attributes.PatientId);
                            var ssn = reader.GetInt32((int) Attributes.PatientSsn);
                            var firstName = reader.GetString((int) Attributes.FirstName);
                            var lastName = reader.GetString((int) Attributes.LastName);
                            var phone = reader.GetString((int) Attributes.Phone);
                            var bdate = reader.GetDateTime((int) Attributes.BirthDate);
                            var addressId = reader.GetInt32((int) Attributes.AddressId);
                            var gender = reader.GetString((int) Attributes.Gender);

                            var patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, addressId)
                                {Id = id};
                            patients.Add(patient);
                        }

                        conn.Close();
                        return patients;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        public static List<Patient> SelectPatientsByDob(DateTime dob)
        {
            var patients = new List<Patient>();
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from patients WHERE CAST(birthDate AS DATE) = @dob";
                    using (var cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var id = reader.GetInt32((int) Attributes.PatientId);
                            var ssn = reader.GetInt32((int) Attributes.PatientSsn);
                            var firstName = reader.GetString((int) Attributes.FirstName);
                            var lastName = reader.GetString((int) Attributes.LastName);
                            var phone = reader.GetString((int) Attributes.Phone);
                            var bdate = reader.GetDateTime((int) Attributes.BirthDate);
                            var addressId = reader.GetInt32((int) Attributes.AddressId);
                            var gender = reader.GetString((int) Attributes.Gender);

                            var patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, addressId)
                                {Id = id};
                            patients.Add(patient);
                        }

                        conn.Close();
                        return patients;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        public static List<Patient> SelectPatientsByNameAndDob(string fname, string lname, DateTime dob)
        {
            var patients = new List<Patient>();
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery =
                        "select * from patients WHERE firstName = @firstName AND lastName = @lastName AND CAST(birthDate AS DATE) = @dob";
                    using (var cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", fname);
                        cmd.Parameters.AddWithValue("@lastName", lname);
                        cmd.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var id = reader.GetInt32((int) Attributes.PatientId);
                            var ssn = reader.GetInt32((int) Attributes.PatientSsn);
                            var firstName = reader.GetString((int) Attributes.FirstName);
                            var lastName = reader.GetString((int) Attributes.LastName);
                            var phone = reader.GetString((int) Attributes.Phone);
                            var bdate = reader.GetDateTime((int) Attributes.BirthDate);
                            var addressId = reader.GetInt32((int) Attributes.AddressId);
                            var gender = reader.GetString((int) Attributes.Gender);

                            var patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, addressId)
                                {Id = id};
                            patients.Add(patient);
                        }

                        conn.Close();
                        return patients;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        public static Patient UpdatePatient(int id, int ssn, string firstName, string lastName, string phoneNumber,
            DateTime dob, string gender, string address, int addressId)
        {
            try
            {
                var street = address.Split(",")[0].Trim();
                var state = address.Split(",")[1].Trim();
                var zip = Convert.ToInt32(address.Split(",")[2].Trim());

                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var updateAddressQuery =
                        "UPDATE `address` SET street = @street, zip = @zip, state = @state WHERE addressID = @addressId";

                    using (var cmd1 = new MySqlCommand(updateAddressQuery, conn))
                    {
                        cmd1.Parameters.AddWithValue("@street", street);
                        cmd1.Parameters.AddWithValue("@state", state);
                        cmd1.Parameters.AddWithValue("@zip", zip);
                        cmd1.Parameters.AddWithValue("@addressId", addressId);
                        cmd1.ExecuteNonQuery();

                        var updatedAddress = new Address(street, state, zip) {AddressId = addressId};
                        Address foundAddress = null;

                        foreach (var item in PatientManager.Addresses)
                            if (item.AddressId == addressId)
                                foundAddress = item;

                        if (foundAddress != null)
                        {
                            PatientManager.Addresses.Remove(foundAddress);
                            PatientManager.Addresses.Add(updatedAddress);
                        }
                    }

                    var updateQuery =
                        "UPDATE `patients` SET ssn = @ssn, firstName = @firstName, lastName = @lastName, birthDate = @dob, gender = @gender, phoneNumber = @phoneNumber WHERE patientID = @id";
                    using (var cmd3 = new MySqlCommand(updateQuery, conn))
                    {
                        cmd3.Parameters.AddWithValue("@id", id);
                        cmd3.Parameters.AddWithValue("@ssn", ssn);
                        cmd3.Parameters.AddWithValue("@firstName", firstName);
                        cmd3.Parameters.AddWithValue("@lastName", lastName);
                        cmd3.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                        cmd3.Parameters.AddWithValue("@gender", gender);
                        cmd3.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        cmd3.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var selectQuery = "select * from patients WHERE patientID = @id";

                    using (var cmd2 = new MySqlCommand(selectQuery, conn))
                    {
                        cmd2.Parameters.AddWithValue("@id", id);
                        var reader = cmd2.ExecuteReader();
                        reader.Read();

                        var patientId = reader.GetInt32((int) Attributes.PatientId);
                        var patientSsn = reader.GetInt32((int) Attributes.PatientSsn);
                        var patientFirstName = reader.GetString((int) Attributes.FirstName);
                        var patientLastName = reader.GetString((int) Attributes.LastName);
                        var patientPhone = reader.GetString((int) Attributes.Phone);
                        var patientBirthdate = reader.GetDateTime((int) Attributes.BirthDate);
                        var patientAddressId = reader.GetInt32((int) Attributes.AddressId);
                        var patientGender = reader.GetString((int) Attributes.Gender);

                        conn.Close();
                        return new Patient(patientSsn, patientFirstName, patientLastName, patientPhone,
                            patientBirthdate, patientGender, patientAddressId) {Id = patientId};
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        private enum Attributes
        {
            PatientId,
            PatientSsn,
            FirstName,
            LastName,
            BirthDate,
            Gender,
            Phone,
            AddressId
        }

        private enum AddressAttributes
        {
            AddressId,
            Street,
            Zip,
            State
        }
    }
}
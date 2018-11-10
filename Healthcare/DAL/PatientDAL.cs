using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using Healthcare.Utils;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public static class PatientDAL {
        private enum Attributes
        {
            PatientId, PatientSsn, FirstName, LastName, BirthDate, Gender, Phone, AddressId
        }

        private enum AddressAttributes
        {
            AddressId, Street, Zip, State
        }

        /// <summary>
        /// Gets the patients.
        /// </summary>
        /// <returns></returns>
        public static List<Patient> GetPatients()
        {
            List<Patient> patients = new List<Patient>();
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from patients";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32((int)Attributes.PatientId);
                            int ssn = reader.GetInt32((int)Attributes.PatientSsn);
                            string fname = reader.GetString((int) Attributes.FirstName);
                            string lname = reader.GetString((int) Attributes.LastName);
                            string phone = reader.GetString((int) Attributes.Phone);
                            DateTime bdate = reader.GetDateTime((int) Attributes.BirthDate);
                            int addressId = reader.GetInt32((int) Attributes.AddressId);
                            string gender = reader.GetString((int)Attributes.Gender);

                            Patient patient = new Patient(ssn, fname, lname, phone, bdate, gender, addressId) {Id = id};
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
        /// Gets the addresses.
        /// </summary>
        /// <returns></returns>
        public static List<Address> GetAddresses()
        {
            List<Address> addresses = new List<Address>();
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from address";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32((int)AddressAttributes.AddressId);
                            string street = reader.GetString((int)AddressAttributes.Street);
                            int zip = reader.GetInt32((int) AddressAttributes.Zip);
                            string state = reader.GetString((int) AddressAttributes.State);

                            Address address = new Address(street, state, zip) {AddressId = id};
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
        /// Adds the patient.
        /// </summary>
        /// <param name="ssn">The ssn.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="address">The address.</param>
        /// <returns>The patient created by the database</returns>
        public static Patient AddPatient(int ssn, string firstName, string lastName, string phoneNumber, DateTime dob, string gender, string address)
        {
            try
            {
                string street = address.Split(",")[0].Trim();
                string state = address.Split(",")[1].Trim();
                int zip = Convert.ToInt32(address.Split(",")[2].Trim());

                int addressId;

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertAddressQuery =
                        "INSERT INTO `address` (`street`, `zip`, `state`) VALUE (@street, @zip, @state)";

                    using (MySqlCommand cmd1 = new MySqlCommand(insertAddressQuery, conn))
                    {
                        cmd1.Parameters.AddWithValue("@street", street);
                        cmd1.Parameters.AddWithValue("@state", state);
                        cmd1.Parameters.AddWithValue("@zip", zip);
                        cmd1.ExecuteNonQuery();
                    }

                    var selectAddressIDQuery = "select LAST_INSERT_ID()";

                    using (MySqlCommand cmd2 = new MySqlCommand(selectAddressIDQuery, conn))
                    {
                        MySqlDataReader lastIndexReader = cmd2.ExecuteReader();
                        lastIndexReader.Read();
                        addressId = lastIndexReader.GetInt32((int) AddressAttributes.AddressId);
                        Address newAddress = new Address(street, state, zip) {AddressId = addressId};
                        PatientManager.Addresses.Add(newAddress);
                    }

                    conn.Close();
                }

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `patients` (`ssn`, `firstName`, `lastName`, `birthDate`, `gender`, `phoneNumber`, `addressID`) VALUES (@ssn, @firstName, @lastName, @dob, @gender, @phoneNumber, @addressId)";
                    using (MySqlCommand cmd3 = new MySqlCommand(insertQuery, conn))
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

                    using (MySqlCommand cmd4 = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader lastIndexReader = cmd4.ExecuteReader();
                        lastIndexReader.Read();
                        int id =  lastIndexReader.GetInt32((int) Attributes.PatientId);
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
            List<Patient> patients = new List<Patient>();
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from patients WHERE firstName = @firstName AND lastName = @lastName";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", fname);
                        cmd.Parameters.AddWithValue("@lastName", lname);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {                         
                            int id = reader.GetInt32((int)Attributes.PatientId);
                            int ssn = reader.GetInt32((int)Attributes.PatientSsn);
                            string firstName = reader.GetString((int)Attributes.FirstName);
                            string lastName = reader.GetString((int)Attributes.LastName);
                            string phone = reader.GetString((int)Attributes.Phone);
                            DateTime bdate = reader.GetDateTime((int)Attributes.BirthDate);
                            int addressId = reader.GetInt32((int)Attributes.AddressId);
                            string gender = reader.GetString((int)Attributes.Gender);

                            Patient patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, addressId) {Id = id};
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
            List<Patient> patients = new List<Patient>();
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from patients WHERE CAST(birthDate AS DATE) = @dob";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32((int)Attributes.PatientId);
                            int ssn = reader.GetInt32((int)Attributes.PatientSsn);
                            string firstName = reader.GetString((int)Attributes.FirstName);
                            string lastName = reader.GetString((int)Attributes.LastName);
                            string phone = reader.GetString((int)Attributes.Phone);
                            DateTime bdate = reader.GetDateTime((int)Attributes.BirthDate);
                            int addressId = reader.GetInt32((int)Attributes.AddressId);
                            string gender = reader.GetString((int)Attributes.Gender);

                            Patient patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, addressId) {Id = id};
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
            List<Patient> patients = new List<Patient>();
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from patients WHERE firstName = @firstName AND lastName = @lastName AND CAST(birthDate AS DATE) = @dob";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", fname);
                        cmd.Parameters.AddWithValue("@lastName", lname);
                        cmd.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32((int)Attributes.PatientId);
                            int ssn = reader.GetInt32((int)Attributes.PatientSsn);
                            string firstName = reader.GetString((int)Attributes.FirstName);
                            string lastName = reader.GetString((int)Attributes.LastName);
                            string phone = reader.GetString((int)Attributes.Phone);
                            DateTime bdate = reader.GetDateTime((int)Attributes.BirthDate);
                            int addressId = reader.GetInt32((int)Attributes.AddressId);
                            string gender = reader.GetString((int)Attributes.Gender);

                            Patient patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, addressId) {Id = id};
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
                string street = address.Split(",")[0].Trim();
                string state = address.Split(",")[1].Trim();
                int zip = Convert.ToInt32(address.Split(",")[2].Trim());

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
               
                    var updateAddressQuery = "UPDATE `address` SET street = @street, zip = @zip, state = @state WHERE addressID = @addressId";

                    using (MySqlCommand cmd1 = new MySqlCommand(updateAddressQuery, conn))
                    {
                        cmd1.Parameters.AddWithValue("@street", street);
                        cmd1.Parameters.AddWithValue("@state", state);
                        cmd1.Parameters.AddWithValue("@zip", zip);
                        cmd1.Parameters.AddWithValue("@addressId", addressId);
                        cmd1.ExecuteNonQuery();

                        Address updatedAddress = new Address(street, state, zip) {AddressId = addressId};
                        Address foundAddress = null;

                        foreach (var item in PatientManager.Addresses)
                        {
                            if (item.AddressId == addressId)
                            {
                                foundAddress = item;
                            }
                        }

                        if (foundAddress != null)
                        {
                            PatientManager.Addresses.Remove(foundAddress);
                            PatientManager.Addresses.Add(updatedAddress);
                        }
                    }

                    var updateQuery =
                        "UPDATE `patients` SET ssn = @ssn, firstName = @firstName, lastName = @lastName, birthDate = @dob, gender = @gender, phoneNumber = @phoneNumber WHERE patientID = @id";
                    using (MySqlCommand cmd3 = new MySqlCommand(updateQuery, conn))
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

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var selectQuery = "select * from patients WHERE patientID = @id";

                    using (MySqlCommand cmd2 = new MySqlCommand(selectQuery, conn))
                    {
                        cmd2.Parameters.AddWithValue("@id", id);
                        MySqlDataReader reader = cmd2.ExecuteReader();
                        reader.Read();
                     
                        int patientId = reader.GetInt32((int)Attributes.PatientId);
                        int patientSsn = reader.GetInt32((int)Attributes.PatientSsn);
                        string patientFirstName = reader.GetString((int) Attributes.FirstName);
                        string patientLastName = reader.GetString((int) Attributes.LastName);
                        string patientPhone = reader.GetString((int) Attributes.Phone);
                        DateTime patientBirthdate = reader.GetDateTime((int) Attributes.BirthDate);
                        int patientAddressId = reader.GetInt32((int) Attributes.AddressId);
                        string patientGender = reader.GetString((int)Attributes.Gender);
                        
                        conn.Close();
                        return new Patient(patientSsn, patientFirstName, patientLastName, patientPhone, patientBirthdate, patientGender, patientAddressId) {Id = patientId};
                    }
                }
            }      
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }
    }
}


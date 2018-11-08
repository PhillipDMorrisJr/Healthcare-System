using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    static class PatientDAL {
        private enum Attributes
        {
            PatientId, PatientSsn, FirstName, LastName, BirthDate, Phone, Address, Gender
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
                            string address = reader.GetString((int) Attributes.Address);
                            string gender = reader.GetString((int)Attributes.Gender);

                            Patient patient = new Patient(ssn, fname, lname, phone, bdate, gender, address) {Id = id};
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
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `patients` (`ssn`, `firstName`, `lastName`, `birthDate`, `gender`, `phoneNumber`, `address`) VALUES (@ssn, @firstName, @lastName, @dob, @gender, @phoneNumber, @address)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ssn", ssn);
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        cmd.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.ExecuteNonQuery();
                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                      MySqlDataReader lastIndexReader =  cmd.ExecuteReader();
                        lastIndexReader.Read();
                      int id =  lastIndexReader.GetInt32((int) Attributes.PatientId);
                      conn.Close();
                      return new Patient(ssn, firstName, lastName, phoneNumber, dob, gender, address) {Id = id};
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

                        while (reader.HasRows)
                        {
                            reader.Read();
                            int id = reader.GetInt32((int)Attributes.PatientId);
                            int ssn = reader.GetInt32((int)Attributes.PatientSsn);
                            string firstName = reader.GetString((int)Attributes.FirstName);
                            string lastName = reader.GetString((int)Attributes.LastName);
                            string phone = reader.GetString((int)Attributes.Phone);
                            DateTime bdate = reader.GetDateTime((int)Attributes.BirthDate);
                            string address = reader.GetString((int)Attributes.Address);
                            string gender = reader.GetString((int)Attributes.Gender);

                            Patient patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, address) {Id = id};
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

                        while (reader.HasRows)
                        {
                            reader.Read();
                            int id = reader.GetInt32((int)Attributes.PatientId);
                            int ssn = reader.GetInt32((int)Attributes.PatientSsn);
                            string firstName = reader.GetString((int)Attributes.FirstName);
                            string lastName = reader.GetString((int)Attributes.LastName);
                            string phone = reader.GetString((int)Attributes.Phone);
                            DateTime bdate = reader.GetDateTime((int)Attributes.BirthDate);
                            string address = reader.GetString((int)Attributes.Address);
                            string gender = reader.GetString((int)Attributes.Gender);

                            Patient patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, address) {Id = id};
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

                        while (reader.HasRows)
                        {
                            reader.Read();
                            int id = reader.GetInt32((int)Attributes.PatientId);
                            int ssn = reader.GetInt32((int)Attributes.PatientSsn);
                            string firstName = reader.GetString((int)Attributes.FirstName);
                            string lastName = reader.GetString((int)Attributes.LastName);
                            string phone = reader.GetString((int)Attributes.Phone);
                            DateTime bdate = reader.GetDateTime((int)Attributes.BirthDate);
                            string address = reader.GetString((int)Attributes.Address);
                            string gender = reader.GetString((int)Attributes.Gender);

                            Patient patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, address) {Id = id};
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

        //TODO: Add UPDATE PATIENT QUERY
    }
}


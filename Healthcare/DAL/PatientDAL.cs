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
            SSN = 0, FirstName, LastName, BirthDate, Phone, Address
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
                            string ssn = reader.GetString((int)Attributes.SSN);
                            string fname = reader.GetString((int) Attributes.FirstName);
                            string lname = reader.GetString((int) Attributes.LastName);
                            string phone = reader.GetString((int) Attributes.Phone);
                            DateTime bdate = reader.GetDateTime((int) Attributes.BirthDate);
                            string address = reader.GetString((int)Attributes.Address);

                            Patient patient = new Patient(fname,lname,phone,bdate, ssn, address);
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
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="dob">The dob.</param>
        /// <returns>The patient created by the database</returns>
        public static Patient AddPatient(string firstName, string lastName, string phoneNumber, DateTime dob, string ssn, string address)
        {

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `patients` (`firstName`, `lastName`, `birthDate`, `phoneNumber`, `ssn`, `address`) VALUES (@firstName, @lastName, @dob, @phoneNumber, @ssn, @address)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        cmd.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@ssn", ssn);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.ExecuteNonQuery();

                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                      MySqlDataReader lastIndexReader =  cmd.ExecuteReader();
                        lastIndexReader.Read();
                      int id =  lastIndexReader.GetInt32((int) Attributes.SSN);
                      conn.Close();
                      return new Patient(firstName,lastName,phoneNumber,dob, "", "");
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
    }
}


﻿using System;
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
    public class AppointmentDAL
    {
        private enum Attributes
        {
            ApptDay = 2, Description = 4, IsCheckedIn = 5, DoctorId = 6, TestOrdered = 8, TestTaken = 9
        }

        public static List<Appointment> GetAppointments(Patient patient)
        {

            List<Appointment> appointments = new List<Appointment>();
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from appointments Where patientID=@patientID";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@patientID", patient.Id);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            uint aID = (uint) reader["appointmentID"];
                            string dID = reader.GetString((int) Attributes.DoctorId);
                            DateTime apptDay = reader.GetDateTime((int) Attributes.ApptDay);
                            TimeSpan time2 = (TimeSpan) reader["apptTime"];
                            string description = reader.GetString((int) Attributes.Description);
                            bool checkedIn = reader.GetBoolean((int) Attributes.IsCheckedIn);
                            bool isTestOrdered = reader.GetBoolean((int) Attributes.TestOrdered);
                            bool isTestTaken = reader.GetBoolean((int) Attributes.TestTaken);



                            Doctor doctor = DoctorManager.Doctors.Find(aDoctor => aDoctor.Id.Equals(dID));
                            Appointment appointment = new Appointment(patient, doctor, apptDay,time2,description,checkedIn, aID, isTestOrdered, isTestTaken);
                            appointments.Add(appointment);
                        }
                        conn.Close();
                        return appointments;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return null;
            }
        }


        public static void AddAppointment(Patient patient, Doctor doctor, DateTime appointmentDateTime,
            TimeSpan appointmentTime, string description,bool isCheckedIn, bool isTestOrdered, bool isTestTaken)
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `appointments` (`doctorID`, `patientID`, `apptDay`, `apptTime`, `description`, `isCheckedIn`,`userID`, `testOrdered`, `testTaken`) VALUES (@doctorID, @patientID, @apptDay, @apptTime, @description, @isCheckedIn, @userID, @testOrdered, @testTaken)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorID", doctor.Id);
                        cmd.Parameters.AddWithValue("@patientID", patient.Id);
                        cmd.Parameters.AddWithValue("@apptDay", appointmentDateTime.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@apptTime", appointmentTime.ToString());
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@isCheckedIn", isCheckedIn);
                        cmd.Parameters.AddWithValue("@testOrdered", isTestOrdered);
                        cmd.Parameters.AddWithValue("@testTaken", isTestTaken);
                        cmd.Parameters.AddWithValue("@userID", AccessValidator.CurrentUser.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                DbConnection.GetConnection().Close();
            }
       
        }

        public List<TimeSpan> GetTimeSlots( DateTime date, Doctor doctor, Patient patient)
        {
            var slots = new List<TimeSpan>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select apptTime from appointments WHERE apptDay=@date AND doctorID=@dID OR patientID=@pID AND apptDay=@date";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.Add("@date", (DbType) MySqlDbType.Date).Value = date.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@dID", (DbType) MySqlDbType.Date).Value = doctor.Id;
                    cmd.Parameters.Add("@pID", (DbType) MySqlDbType.Date).Value = patient.Id;
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return slots;

                    while (reader.Read())
                    {
                        TimeSpan timeSlot = (TimeSpan) reader["apptTime"];
                        slots.Add(timeSlot);                       
                    }
                }
            }
            return slots;
        }

        public static bool updateTestOrdered(int apptId)
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var updateQuery =
                        "UPDATE `appointments` SET testOrdered = @testOrdered WHERE appointmentID = @apptID";
                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@testOrdered", 1);
                        cmd.Parameters.AddWithValue("@apptID", apptId);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                return true;
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                DbConnection.GetConnection().Close();
                return false;
            }
        }
    }
}

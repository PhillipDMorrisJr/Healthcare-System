using System;
using System.Collections.Generic;
using System.Data;
using Healthcare.Model;
using Healthcare.Utils;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    public class AppointmentDAL
    {
        public static List<Appointment> GetAppointments(Patient patient)
        {
            var appointments = new List<Appointment>();
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from appointments Where patientID=@patientID";
                    using (var cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@patientID", patient.Id);
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var aID = (uint) reader["appointmentID"];
                            var dID = reader.GetString((int) Attributes.DoctorId);
                            var apptDay = reader.GetDateTime((int) Attributes.ApptDay);
                            var time2 = (TimeSpan) reader["apptTime"];
                            var description = reader.GetString((int) Attributes.Description);
                            var checkedIn = reader.GetBoolean((int) Attributes.IsCheckedIn);


                            var doctor = DoctorManager.Doctors.Find(aDoctor => aDoctor.Id.Equals(dID));
                            var appointment = new Appointment(patient, doctor, apptDay, time2, description, checkedIn,
                                aID);
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

        public static void AddAppointment(Appointment details)
        {
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `appointments` (`doctorID`, `patientID`, `apptDay`, `apptTime`, `description`, `isCheckedIn`,`userID`) VALUES (@doctorID, @patientID, @apptDay, @apptTime, @description, @isCheckedIn, @userID)";
                    using (var cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorID", details.Doctor.Id);
                        cmd.Parameters.AddWithValue("@patientID", details.Patient.Id);
                        cmd.Parameters.AddWithValue("@apptDay",
                            details.AppointmentDateTime.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@apptTime", details.AppointmentTime.ToString());
                        cmd.Parameters.AddWithValue("@description", details.Description);
                        cmd.Parameters.AddWithValue("@isCheckedIn", details.IsCheckedIn);
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

        public List<TimeSpan> GetTimeSlots(DateTime date, Doctor doctor, Patient patient)
        {
            var slots = new List<TimeSpan>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery =
                    "select apptTime from appointments WHERE apptDay=@date AND doctorID=@dID OR patientID=@pID AND apptDay=@date";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.Add("@date", (DbType) MySqlDbType.Date).Value = date.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@dID", (DbType) MySqlDbType.Date).Value = doctor.Id;
                    cmd.Parameters.Add("@pID", (DbType) MySqlDbType.Date).Value = patient.Id;
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return slots;

                    while (reader.Read())
                    {
                        var timeSlot = (TimeSpan) reader["apptTime"];
                        slots.Add(timeSlot);
                    }
                }
            }

            return slots;
        }

        public static void UpdateAppointment(Appointment originalAppointment, Appointment newAppointment)
        {
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "UPDATE `appointments` SET `doctorID` = @doctorID, `patientID` = @patientID, `apptDay` = @apptDay, `apptTime` = @apptTime, `description` = @description, `isCheckedIn` = @isCheckedIn,`userID` = @userID WHERE `appointmentID` = @aID";
                    using (var cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorID", newAppointment.Doctor.Id);
                        cmd.Parameters.AddWithValue("@patientID", newAppointment.Patient.Id);
                        cmd.Parameters.AddWithValue("@apptDay",
                            newAppointment.AppointmentDateTime.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@apptTime", newAppointment.AppointmentTime.ToString());
                        cmd.Parameters.AddWithValue("@description", newAppointment.Description);
                        cmd.Parameters.AddWithValue("@isCheckedIn", newAppointment.IsCheckedIn);
                        cmd.Parameters.AddWithValue("@aID", originalAppointment.ID);
                        cmd.Parameters.AddWithValue("@userID", AccessValidator.CurrentUser.Id);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                DbConnection.GetConnection().Close();
            }
        }

        private enum Attributes
        {
            ApptDay = 2,
            Description = 4,
            IsCheckedIn = 5,
            DoctorId = 6
        }
    }
}
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
    class AppointmentDAL
    {
        private enum Attributes
        {
            doctorId = 6, patientId=1, apptDay, apptTime, description, isCheckedIn
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
                            string dID = reader.GetString((int)AppointmentDAL.Attributes.doctorId);
                            DateTime apptDay = reader.GetDateTime((int) AppointmentDAL.Attributes.apptDay);
                            TimeSpan time2 = (TimeSpan) reader["apptTime"];
                            string description = reader.GetString((int) AppointmentDAL.Attributes.description);
                            bool status = reader.GetBoolean((int) Attributes.isCheckedIn);

                            Doctor doctor = DoctorManager.Doctors.Find(aDoctor => aDoctor.Id.Equals(dID));
                            Appointment appointment = new Appointment(patient, doctor, apptDay,time2,description,status, aID);
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
            TimeSpan appointmentTime, string description,bool isCheckedIn)
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `appointments` (`doctorID`, `patientID`, `apptDay`, `apptTime`, `description`, `isCheckedIn`,`userID`) VALUES (@doctorID, @patientID, @apptDay, @apptTime, @description, @isCheckedIn, @userID)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorID", doctor.Id);
                        cmd.Parameters.AddWithValue("@patientID", patient.Ssn);
                        cmd.Parameters.AddWithValue("@apptDay", appointmentDateTime.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@apptTime", appointmentTime.ToString());
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@isCheckedIn", isCheckedIn);
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
    }
}

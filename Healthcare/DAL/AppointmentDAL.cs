using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Healthcare.Model;
using Healthcare.Utils;
using MySql.Data.MySqlClient;

namespace Healthcare.DAL
{
    static class AppointmentDAL
    {       
        private enum Attributes
        {
            doctorId = 0, patientId, apptDay, apptTime, description
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
                        cmd.Parameters.AddWithValue("@patientID", patient.SSN);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string dID = reader.GetString((int)AppointmentDAL.Attributes.doctorId);
                            DateTime apptDay = reader.GetDateTime((int) AppointmentDAL.Attributes.apptDay);
                            TimeSpan time2 = (TimeSpan) reader["apptTime"];
                            string description = reader.GetString((int) AppointmentDAL.Attributes.description);


                            Doctor doctor = DoctorManager.Doctors.Find(aDoctor => aDoctor.Id.Equals(dID));
                            Appointment appointment = new Appointment(patient, doctor, apptDay,time2,description);
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
            TimeSpan appointmentTime, string description)
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    var insertQuery =
                        "INSERT INTO `appointments` (`doctorID`, `patientID`, `apptDay`, `apptTime`, `description`) VALUES (@doctorID, @patientID, @apptDay, @apptTime, @description)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorID", doctor.Id);
                        cmd.Parameters.AddWithValue("@patientID", patient.SSN);
                        cmd.Parameters.AddWithValue("@apptDay", appointmentDateTime.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@apptTime", appointmentTime.ToString());
                        cmd.Parameters.AddWithValue("@description", description);
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
    }
}

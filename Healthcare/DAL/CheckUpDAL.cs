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
    public static class CheckUpDAL
    {
        private enum Attributes
        {
            CuId, ApptId, Systolic, Diastolic, PatientId, Temperature, ArrivalDate,
            Weight = 9, Pulse
        }

        private enum PatientAttributes
        {
            PatientId, PatientSsn, FirstName, LastName, BirthDate, Gender, Phone, AddressId
        }

        private enum AppointmentAttributes
        {
            ApptDay = 2, Description = 4, IsCheckedIn = 5, DoctorId = 6
        }

        public static CheckUp AddCheckUp(CheckUp details)
        {
            try
            {
                int cuid;
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    
                    var insertQuery =
                        "INSERT INTO `checkup` (`appointmentID`, `systolic`, `diastolic`, `pID`, `temperature`, `arrivalDate`, `arrivalTime`, `nurseID`, `weight`, `pulse`) VALUES (@apptID, @systolic, @diastolic, @pID, @temp, @arrivalDate, @arrivalTime, @nurseID, @weight, @pulse)";
                    using (MySqlCommand command = new MySqlCommand(insertQuery, conn))
                    {
                        command.Parameters.AddWithValue("@apptID", details.Appointment.ID);
                        command.Parameters.AddWithValue("@systolic", details.Systolic);
                        command.Parameters.AddWithValue("@diastolic", details.Diastolic);
                        command.Parameters.AddWithValue("@pID", details.Patient.Id);
                        command.Parameters.AddWithValue("@arrivalDate", details.ArrivalDate.ToString(("yyyy-MM-dd")));
                        command.Parameters.AddWithValue("@arrivalTime", details.ArrivalTime.ToString());
                        command.Parameters.AddWithValue("@temp", details.Temperature);
                        command.Parameters.AddWithValue("@nurseID", details.Nurse.Id);
                        command.Parameters.AddWithValue("@weight", details.Weight);
                        command.Parameters.AddWithValue("@pulse", details.Pulse);
                        command.ExecuteNonQuery();
                    }

                    var selectQuery = "select LAST_INSERT_ID()";

                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn))
                    {
                        MySqlDataReader lastIndexReader = selectCommand.ExecuteReader();
                        lastIndexReader.Read();
                        cuid = lastIndexReader.GetInt32(0);

                        details.cuID = cuid;
                        CheckUpManager.Checkups.Add(details);
                    }

                    conn.Close();
                }

                foreach (Symptom symptom in details.Symptoms)
                {
                    using (MySqlConnection conn = DbConnection.GetConnection())
                    {
                        conn.Open();
                        var insertQuery = "INSERT INTO `checkupSymptoms` (`cuID`, `sID`) VALUES (@cuid, @sid)";

                        using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, conn))
                        {
                            insertCommand.Parameters.AddWithValue("@cuid", cuid);
                            insertCommand.Parameters.AddWithValue("@sid", symptom.ID);
                            insertCommand.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var checkInQuery =
                        "UPDATE `appointments` SET `isCheckedIn` = @status WHERE appointmentID = @aID AND patientID = @pID";
                    using (MySqlCommand checkInCommand = new MySqlCommand(checkInQuery, conn))
                    {

                        checkInCommand.Parameters.AddWithValue("@status", 1);
                        checkInCommand.Parameters.AddWithValue("@aID", details.Appointment.ID);
                        checkInCommand.Parameters.AddWithValue("@pID", details.Patient.Id);
                        checkInCommand.ExecuteNonQuery();

                    }

                    conn.Close();
                }

                return details;

            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                DbConnection.GetConnection().Close();
                return null;
            }
        }

        public static List<CheckUp> GetCheckups() 
        {
            var checkups = new List<CheckUp>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery = "select * from checkup";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {                  
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return checkups;

                    while (reader.Read())
                    {
                        var cuId = reader.GetInt32((int) Attributes.CuId);
                        var systolic = reader.GetInt32((int) Attributes.Systolic);
                        var diastolic = reader.GetInt32((int) Attributes.Diastolic);
                        var patientId = reader.GetInt32((int) Attributes.PatientId);
                        var temperature = reader.GetDouble((int) Attributes.Temperature);
                        var arrivalDate = reader.GetDateTime((int) Attributes.ArrivalDate);
                        var arrivalTime = (TimeSpan) reader["arrivalTime"];
                        var weight = reader.GetDouble((int) Attributes.Weight);
                        var pulse = reader.GetInt32((int) Attributes.Pulse);
                        var apptId = reader.GetInt32((int) Attributes.ApptId);

                        var patient = FindPatient(patientId);
                        var nurse = AccessValidator.CurrentUser as Nurse;
                        var symptoms = FindSymptoms(cuId);
                        var appointment = FindAppointment(apptId, patient);

                        var newCheckUp = new CheckUp(systolic, diastolic, patient, temperature, arrivalDate, arrivalTime, nurse,
                        weight, pulse, symptoms, appointment) {cuID = cuId};
                        checkups.Add(newCheckUp);                       
                    }
                }
            }
            return checkups;
        }

        private static Patient FindPatient(int patientId)
        {
            Patient patient = null;

            try
            {            
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from patients WHERE patientID = @patientId";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@patientId", patientId);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {                         
                            int id = reader.GetInt32((int)PatientAttributes.PatientId);
                            int ssn = reader.GetInt32((int)PatientAttributes.PatientSsn);
                            string firstName = reader.GetString((int)PatientAttributes.FirstName);
                            string lastName = reader.GetString((int)PatientAttributes.LastName);
                            string phone = reader.GetString((int)PatientAttributes.Phone);
                            DateTime bdate = reader.GetDateTime((int)PatientAttributes.BirthDate);
                            int addressId = reader.GetInt32((int)PatientAttributes.AddressId);
                            string gender = reader.GetString((int)PatientAttributes.Gender);

                            patient = new Patient(ssn, firstName, lastName, phone, bdate, gender, addressId) {Id = id};                         
                        }
                        conn.Close();
                        return patient;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return patient;
            }
        }
        public static Appointment FindAppointment(int apptId, Patient patient)
        {
            Appointment appointment = null;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    var selectQuery = "select * from appointments Where appointmentID=@apptId";
                    using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@apptId", apptId);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            uint aID = (uint) reader["appointmentID"];
                            string dID = reader.GetString((int) AppointmentAttributes.DoctorId);
                            DateTime apptDay = reader.GetDateTime((int) AppointmentAttributes.ApptDay);
                            TimeSpan time = (TimeSpan) reader["apptTime"];
                            string description = reader.GetString((int) AppointmentAttributes.Description);
                            bool checkedIn = reader.GetBoolean((int) AppointmentAttributes.IsCheckedIn);
          

                            Doctor doctor = DoctorManager.Doctors.Find(aDoctor => aDoctor.Id.Equals(dID));
                            appointment = new Appointment(patient, doctor, apptDay,time,description,checkedIn, aID);
                        }
                        conn.Close();
                        return appointment;
                    }
                }
            }
            catch (Exception)
            {
                DbConnection.GetConnection().Close();
                return appointment;
            }
        }

        public static List<Symptom> FindSymptoms(int cuId)
        {
            var symptoms = new List<Symptom>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                const string selectQuery =
                    "select symptoms.sID, symptoms.name from symptoms WHERE sID IN (SELECT sID FROM checkupSymptoms where cuID = @cuid)";

                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@cuid", cuId);
                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows) return symptoms;

                    while (reader.Read())
                    {
                        var id = (uint) reader["sID"];
                        var name = (string) reader["name"];

                        var newSymptom = new Symptom(id, name);
                        symptoms.Add(newSymptom);                       
                    }
                }
            }
            return symptoms;
        }
    }  
}

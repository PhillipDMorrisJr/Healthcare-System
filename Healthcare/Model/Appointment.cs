using System;

namespace Healthcare.Model
{
    public class Appointment
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Appointment" /> class.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        /// <param name="appointmentTime">The appointment time.</param>
        /// <param name="description">The description.</param>
        /// <param name="checkedIn">The status for if the patient is checked in.</param>
        public Appointment(Patient patient, Doctor doctor, DateTime appointmentDateTime, TimeSpan appointmentTime,
            string description, bool checkedIn)
        {
            if (appointmentDateTime == null) throw new ArgumentException();

            Doctor = doctor ?? throw new ArgumentException("Doctor must not be null");
            Patient = patient ?? throw new ArgumentException("Patient must not be null");
            AppointmentDateTime = appointmentDateTime;
            AppointmentTime = appointmentTime;
            IsCheckedIn = checkedIn;
            Description = description;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Appointment" /> class.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="doctor">The doctor.</param>
        /// <param name="appointmentDateTime">The appointment date time.</param>
        /// <param name="appointmentTime">The appointment time.</param>
        /// <param name="description">The description.</param
        /// <param name="checkedIn">The status for if the patient is checked in.</param>
        public Appointment(Patient patient, Doctor doctor, DateTime appointmentDateTime, TimeSpan appointmentTime,
            string description, bool checkedIn, uint id)
        {
            Doctor = doctor ?? throw new ArgumentException("Doctor must not be null");
            Patient = patient ?? throw new ArgumentException("Patient must not be null");
            AppointmentDateTime = appointmentDateTime;
            AppointmentTime = appointmentTime;
            IsCheckedIn = checkedIn;
            Description = description;
            ID = id;
        }

        /// <summary>
        ///     Gets the patient.
        /// </summary>
        /// <value>
        ///     The patient.
        /// </value>
        public Patient Patient { get; }

        /// <summary>
        ///     Gets the doctor.
        /// </summary>
        /// <value>
        ///     The doctor.
        /// </value>
        public Doctor Doctor { get; }

        /// <summary>
        ///     Gets the appointment date time.
        /// </summary>
        /// <value>
        ///     The appointment date time.
        /// </value>
        public DateTime AppointmentDateTime { get; }

        /// <summary>
        ///     Gets the appointment time.
        /// </summary>
        /// <value>
        ///     The appointment time.
        /// </value>
        public TimeSpan AppointmentTime { get; }


        public uint ID { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is checked in.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked in; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckedIn { get; }

        public string Description { get; set; }

        /// <summary>
        ///     Formats this instance.
        /// </summary>
        /// <returns></returns>
        public string Format()
        {
            var time = DateTime.Today.Add(AppointmentTime);

            return Doctor.FullName + " on " + AppointmentDateTime.ToString("d") + " at " + time.ToString("hh:mm tt");
        }
    }
}
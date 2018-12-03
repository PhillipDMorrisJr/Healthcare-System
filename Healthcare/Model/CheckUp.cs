using System;
using System.Collections.Generic;

namespace Healthcare.Model
{
    public class CheckUp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckUp" /> class.
        /// </summary>
        /// <param name="systolic">The systolic.</param>
        /// <param name="diastolic">The diastolic.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="arrivalTime">The arrival time.</param>
        /// <param name="nurse">The nurse.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="pulse">The pulse.</param>
        /// <param name="symptoms">The symptoms.</param>
        /// <param name="appointment">The appointment.</param>
        public CheckUp(int systolic, int diastolic, Patient patient, double temperature, DateTime arrivalDate,
            TimeSpan arrivalTime, Nurse nurse,
            double weight, int pulse, List<Symptom> symptoms, Appointment appointment)
        {
            Systolic = systolic;
            Diastolic = diastolic;
            Patient = patient;
            Temperature = temperature;
            ArrivalTime = arrivalTime;
            Nurse = nurse;
            Weight = weight;
            Pulse = pulse;
            Symptoms = symptoms;
            Appointment = appointment;
            ArrivalDate = arrivalDate;
        }

        /// <summary>
        ///     Gets the systolic.
        /// </summary>
        /// <value>
        ///     The systolic.
        /// </value>
        public int Systolic { get; }

        /// <summary>
        ///     Gets the diastolic.
        /// </summary>
        /// <value>
        ///     The diastolic.
        /// </value>
        public int Diastolic { get; }

        /// <summary>
        ///     Gets the pulse.
        /// </summary>
        /// <value>
        ///     The pulse.
        /// </value>
        public int Pulse { get; }

        /// <summary>
        ///     Gets the temperature.
        /// </summary>
        /// <value>
        ///     The temperature.
        /// </value>
        public double Temperature { get; }

        /// <summary>
        ///     Gets the weight.
        /// </summary>
        /// <value>
        ///     The weight.
        /// </value>
        public double Weight { get; }

        /// <summary>
        ///     Gets the nurse.
        /// </summary>
        /// <value>
        ///     The nurse.
        /// </value>
        public Nurse Nurse { get; }

        /// <summary>
        ///     Gets the arrival time.
        /// </summary>
        /// <value>
        ///     The arrival time.
        /// </value>
        public TimeSpan ArrivalTime { get; }

        /// <summary>
        ///     Gets the patient.
        /// </summary>
        /// <value>
        ///     The patient.
        /// </value>
        public Patient Patient { get; }

        /// <summary>
        ///     Gets the symptoms.
        /// </summary>
        /// <value>
        ///     The symptoms.
        /// </value>
        public List<Symptom> Symptoms { get; }

        /// <summary>
        ///     Gets the appointment.
        /// </summary>
        /// <value>
        ///     The appointment.
        /// </value>
        public Appointment Appointment { get; }

        public int cuID { get; set; }

        public DateTime ArrivalDate { get; }
    }
}
namespace Healthcare.Model
{
    public class Symptom
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Symptom" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public Symptom(uint id, string name)
        {
            ID = id;
            Name = name;
        }

        public uint ID { get; }
        public string Name { get; }
    }
}
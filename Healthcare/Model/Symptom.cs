using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class Symptom
    {
        public uint ID { get; private set; }
        public string Name { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Symptom"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public Symptom(uint id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}

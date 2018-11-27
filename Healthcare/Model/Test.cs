using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Model
{
    public class Test
    {
        public int Code { get; private set; }
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class.
        /// </summary>
        /// <param name="code">The code for the test.</param>
        /// <param name="name">The name of the test.</param>
        public Test(int code, string name)
        {
            this.Code = code;
            this.Name = name;
        }
    }
}

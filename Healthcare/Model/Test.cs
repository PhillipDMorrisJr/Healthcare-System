namespace Healthcare.Model
{
    public class Test
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Test" /> class.
        /// </summary>
        /// <param name="code">The code for the test.</param>
        /// <param name="name">The name of the test.</param>
        public Test(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public int Code { get; }
        public string Name { get; }
    }
}
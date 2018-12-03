namespace Healthcare.Model
{
    internal class Administrator : User
    {
        public Administrator(string username, string password, int id) : base(username, password, id)
        {
        }
    }
}
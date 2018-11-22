namespace Uni.DataAccess.Models
{
    public abstract class Person : ITableObject
    {
        public int Id { get; set; }
        public string AvatarPath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}

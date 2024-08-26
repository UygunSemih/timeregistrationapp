using SQLite;

namespace TimeregistrationApp.Models
{
    public partial class DbVersion
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int Version { get; set; }
    }
}

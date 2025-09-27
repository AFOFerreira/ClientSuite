using ClientSuite.Model.Entities.Base;
using SQLite;

namespace ClientSuite.Model.Entities
{
    public sealed class Client : BaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string LastName { get; set; }
        public Guid AddressId { get; set; }
        [Ignore]
        public ClientAddress Address { get; set; }
        [Ignore]
        public string FullName => $"{Name} {LastName}";
        [Ignore]
        public string Initials
        {
            get
            {
                var first = Name?.Length > 0 ? Name[0].ToString() : "";
                var last = LastName?.Length > 0 ? LastName[0].ToString() : "";
                return $"{first}{last}".ToUpper();
            }
        }
        [Ignore]
        public string SafeCity => Address?.City ?? "Cidade não informada";
        [Ignore]
        public string SafeState => Address?.State ?? "Estado não informado";
    }
}

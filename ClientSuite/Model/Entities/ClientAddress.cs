using ClientSuite.Model.Entities.Base;

namespace ClientSuite.Model.Entities
{
    public sealed class ClientAddress : BaseEntity
    {
        public int ZipCode { get; set; }
        public int Number { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Complement { get; set; }
    }
}

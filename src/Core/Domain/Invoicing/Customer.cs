namespace src.Core.Domain.Invoicing
{
    public class Customer
    {
        public Customer(string name) => Name = name;
        public int Id { get; protected set; }
        public string Name { get; set; }
    }
}
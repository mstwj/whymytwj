namespace Blazor7server.Data
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public int Birthday { get; set; }
        public int Gender { get; set; } = 0;

        public int Data { get; set; } = 0;
    }
}

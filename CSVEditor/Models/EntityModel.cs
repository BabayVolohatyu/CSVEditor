namespace CSVEditor.Models
{
    public class EntityModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public bool Married { get; set; }
        public string Phone { get; set; } = string.Empty;
        public float Salary { get; set; }
    }
}

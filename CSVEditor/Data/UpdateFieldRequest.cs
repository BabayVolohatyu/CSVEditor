namespace CSVEditor.Data
{
    public class UpdateFieldRequest
    {
        public int Id { get; set; }
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}

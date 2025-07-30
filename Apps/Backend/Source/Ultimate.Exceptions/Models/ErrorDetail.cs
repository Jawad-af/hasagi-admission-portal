namespace Ultimate.Exceptions.Models
{
    public class ErrorDetail
    {
        public string Description { get; set; }

        public ErrorDetail(string description)
        {
            Description = description;
        }
    }
}

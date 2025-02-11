namespace PatientManagement.Api.Results
{
    public class ExceptionResult
    {
        public string? Message { get; set; }
        public int? StatusCode { get; set; }
        public List<string>? Errors { get; set; }
    }
}

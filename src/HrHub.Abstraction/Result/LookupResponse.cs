namespace HrHub.Abstraction.Result
{

    public class LookupResponse
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

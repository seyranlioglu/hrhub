namespace HrHub.Abstraction.Result
{
    public sealed class ResponseHeader
    {
        public ResponseHeader()
        {
            MsgId = Guid.NewGuid().ToString();
        }

        public string MsgId { get; set; }

        public bool Result { get; set; }

        public string Msg { get; set; }

        public int ResCode { get; set; }

        public DateTime Dt { get; set; } = DateTime.UtcNow;
    }
}

namespace HrHub.Domain.Contracts.Dtos.CommentVoteDtos
{
    public class CommentVoteDto
    {

        public long Id { get; set; }
        public long ContentCommentId { get; set; }
        public long UserId { get; set; }
        public bool Positive { get; set; }
        public bool Editable { get; set; }
    }
}

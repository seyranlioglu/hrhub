using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class FileType : TypeCardEntity<long>
    {
        public FileType()
        {
            ContentLibraries = new HashSet<ContentLibrary>();
        }

        public virtual ICollection<ContentLibrary> ContentLibraries { get; set; }
    }
}

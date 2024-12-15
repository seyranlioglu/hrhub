using HrHub.Abstraction.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Notification.BaseModel
{
    /// <summary>
    /// Message Template : Gönderilecek Mesaj template seçilecek (Register, Login vb)
    /// Parameters : ilgili Template içinde değiştirilecek parametreler için value ları taşıyacak.
    /// </summary>
    public interface IMessage
    {
        MessageTemplates MessageTemplate { get; set; }
        string Recipient { get; set; }
        string Content { get; set; }
        Dictionary<string, string> Parameters { get; set; }
    }
}

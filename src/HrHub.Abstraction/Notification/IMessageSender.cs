using HrHub.Abstraction.Notification.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Notification
{
    public interface IMessageSender
    {
        void Send(IMessage message);
        Task SendAsync(IMessage message);
    }
}

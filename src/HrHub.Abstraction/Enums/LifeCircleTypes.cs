using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Enums
{
    public enum LifeCycleTypes
    {
        Singleton,
        Transient,
        Scoped,
        ScopedAlone,
        SingletonAlone,
        TransientAlone,
        NotRegister
    }
}

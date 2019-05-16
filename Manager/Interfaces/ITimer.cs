using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Interfaces
{
    public interface ITimer
    {
        Task Wait(int milliseconds);
    }
}

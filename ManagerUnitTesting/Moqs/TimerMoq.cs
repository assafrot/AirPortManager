using Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerUnitTesting.Moqs
{
    public class TimerMoq : ITimer
    {
        public Task Wait(int milliseconds)
        {
            return Task.Run(()=>{ });
        }
    }
}

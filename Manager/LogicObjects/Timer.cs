using Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.LogicObjects
{
    public class Timer : ITimer
    {
        public Task Wait(int milliseconds)
        {
            return Task.Run(() =>
            {
                Random rnd = new Random();
                Thread.Sleep((rnd.Next(3) + 1) * milliseconds);
            });
        }
    }
}

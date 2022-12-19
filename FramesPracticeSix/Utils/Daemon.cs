using System;

namespace FramesPracticeSix.Utils
{
    public enum TypeOfDaemonInvocation
    {
        None,
        OnChangeSlotValue,
        OnDestroySlot
    }

    
    public readonly struct Daemon
    {
        private readonly TypeOfDaemonInvocation _trigger;
        private readonly Action _action;

        private Daemon(TypeOfDaemonInvocation trigger, Action action)
        {
            _trigger = trigger;
            _action = action;
        }

        public static Daemon CreateDaemon(TypeOfDaemonInvocation trigger, Action action)
        {
            return new Daemon(trigger, action);
        }

        public static Daemon Default => new Daemon(TypeOfDaemonInvocation.None, null);
        
        public void Call(TypeOfDaemonInvocation invocation)
        {
            if (invocation != _trigger || _action == null)
            {
                return;
            }
            
            _action?.Invoke();
        }
    }
}
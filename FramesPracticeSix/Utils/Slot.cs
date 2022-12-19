namespace FramesPracticeSix.Utils
{
    public enum TypeData
    {
        FRAME,
        PROCEDURE,
        INTEGER,
        REAL,
        BOOL,
        TEXT
    }
    
    public class Slot
    {
        public object Value
        {
            set
            {
                if (Equals(_value, value) == false)
                {
                    _value = value;
                    _daemon.Call(TypeOfDaemonInvocation.OnChangeSlotValue);
                }
            }
        }
        
        public bool Bool => _type == TypeData.BOOL && _value is bool value && value;
        public Frame Frame => _type == TypeData.FRAME ? _value as Frame : null;
        
        private object _value;
        private readonly TypeData _type;
        private Daemon _daemon;

        private Slot(TypeData type, object value)
        {
            _type = type;
            _value = value;
            _daemon = Daemon.Default;
        }
        
        public static Slot FrameSlot(Frame value)
        {
            return new Slot(TypeData.FRAME, value);
        }

        public static Slot BoolSlot(bool value)
        {
            return new Slot(TypeData.BOOL, value);
        }
        
        public Slot SetDaemon(Daemon daemon)
        {
            _daemon = daemon;
            return this;
        }
    }
}
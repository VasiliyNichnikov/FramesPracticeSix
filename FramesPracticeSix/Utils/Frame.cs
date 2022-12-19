using System;
using System.Collections.Generic;

namespace FramesPracticeSix.Utils
{
    public abstract class Frame
    {
        public string Name => _name;

        public Slot GetSlot(string name)
        {
            if (Slots.ContainsKey(name) == false)
            {
                throw new Exception();
            }

            return Slots[name];
        }

        /// <summary>
        /// Имя фрейма
        /// </summary>
        private readonly string _name;

        protected Frame(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Слоты
        /// </summary>
        protected Dictionary<string, Slot> Slots;
    }
}
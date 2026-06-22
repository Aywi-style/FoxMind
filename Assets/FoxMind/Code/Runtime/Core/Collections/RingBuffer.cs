using System;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Collections
{
    [Serializable]
    public class RingBuffer<T>
    {
        [ShowInInspector] private readonly T[] _buffer;
        private int _head;
        private int _count;

        public RingBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _buffer = new T[capacity];
        }

        public int Count => _count;
        public int Capacity => _buffer.Length;

        /// <summary>
        /// Добавляет новый элемент.
        /// При переполнении перезаписывает самый старый.
        /// </summary>
        public virtual void Push(T item)
        {
            _buffer[_head] = item;
            _head = (_head + 1) % Capacity;

            if (_count < Capacity)
                _count++;
        }

        /// <summary>
        /// Последний элемент = [0]
        /// Предпоследний = [1]
        /// И т.д.
        /// </summary>
        public T this[int indexFromEnd]
        {
            get
            {
                if (indexFromEnd < 0 || indexFromEnd >= _count)
                    throw new ArgumentOutOfRangeException(nameof(indexFromEnd));

                int index = (_head - 1 - indexFromEnd + Capacity) % Capacity;
                return _buffer[index];
            }
        }

        public void Clear()
        {
            _head = 0;
            _count = 0;
        }
    }
}
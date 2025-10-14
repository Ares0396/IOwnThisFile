using System.Buffers;

namespace Main.Support_Tools.Memory_Management
{
    public unsafe sealed class UnmanagedMemory<T> : MemoryManager<T> where T : unmanaged
    {
        private SafePtr<T> _buffer;
        private int _length;
        private bool _disposed;

        public UnmanagedMemory(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
            _length = length;
            _buffer = new SafePtr<T>(length);
        }
        public override Span<T> GetSpan()
        {
            ThrowIfDisposed();
            return new Span<T>(_buffer.GetRawPtr(_buffer.Generation), _length);
        }
        public override MemoryHandle Pin(int elementIndex = 0)
        {
            ThrowIfDisposed();
            if ((uint)elementIndex >= _length) throw new ArgumentOutOfRangeException(nameof(elementIndex));

            // Return a MemoryHandle pointing to the requested element
            return new MemoryHandle(_buffer.GetRawPtr(_buffer.Generation) + elementIndex);
        }
        public override void Unpin()
        {
            // Nothing to do, but crucial for MemoryManager<T> contract
        }
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _buffer?.Dispose();
                    _buffer = null!;
                }
                _disposed = true;
            }
        }

        public void Resize(int newLength)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newLength);

            var newBuffer = new SafePtr<T>(newLength);

            // Copy existing data to new buffer
            int copyLength = Math.Min(_length, newLength);
            Buffer.MemoryCopy(_buffer.GetRawPtr(_buffer.Generation), newBuffer.GetRawPtr(newBuffer.Generation), newLength * sizeof(T), copyLength * sizeof(T));

            // Invalidate old buffer
            _buffer.Dispose();

            // Swap buffers
            _buffer = newBuffer;
            _length = newLength;

            // newBuffer.Generation remains 1
        }

        private void ThrowIfDisposed()
        {
            if (_disposed || _buffer == null)
                throw new ObjectDisposedException(nameof(UnmanagedMemory<T>), "Cannot use after disposal.");
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}

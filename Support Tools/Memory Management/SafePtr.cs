using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace Main.Support_Tools.Memory_Management
{
    public sealed unsafe class SafePtr<T> : IDisposable where T : unmanaged
    {
        private IntPtr _ptr;
        private int _generation;
        private bool _disposed = false;

        public SafePtr(int count)
        {
            _ptr = Marshal.AllocHGlobal(sizeof(T) * count);
            _generation = 1; //Gen 1 means usable, gen 2+ means disposed of
        }
        public T* GetRawPtr(int generationCheck)
        {
            ThrowIfDisposed(); //Check if object is disposed
            if (generationCheck != _generation)
                throw new ObjectDisposedException(nameof(SafePtr<T>), "Pointer is dangling (memory has been freed).");
            return (T*)_ptr.ToPointer();
        }
        public void IncrementGeneration()
        {
            _generation++;
        }
        public IntPtr GetIntPtr
        {
            get
            {
                ThrowIfDisposed();
                return _ptr;
            }
        }
        public int Generation => _generation;

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_ptr);
                    _ptr = IntPtr.Zero;
                }
                IncrementGeneration(); //Mark this Ptr unusable
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SafePtr<T>), "Cannot use after disposal.");
        }
    }
}

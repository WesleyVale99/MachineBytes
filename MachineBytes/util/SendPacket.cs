using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MachineBytes
{
    public class SendPacket : IDisposable
    {
        public MemoryStream stream = new MemoryStream();
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            stream.Dispose();
            if (disposing)
                handle.Dispose();
            disposed = true;
        }
        protected internal void WriteB(byte[] value)
        {
            stream.Write(value, 0, value.Length);
        }
        protected internal void WriteD(int value)
        {
            WriteB(BitConverter.GetBytes(value));
        }
        protected internal void WriteB(byte[] value, int offset, int length)
        {
            stream.Write(value, offset, length);
        }
        protected internal void WriteD(uint value)
        {
            WriteB(BitConverter.GetBytes(value));
        }
        protected internal void WriteH(short val)
        {
            WriteB(BitConverter.GetBytes(val));
        }
        protected internal void WriteH(ushort val)
        {
            WriteB(BitConverter.GetBytes(val));
        }
        protected internal void WriteC(byte value)
        {
            stream.WriteByte(value);
        }
        protected internal void WriteF(double value)
        {
            WriteB(BitConverter.GetBytes(value));
        }
        protected internal void WriteT(float value)
        {
            WriteB(BitConverter.GetBytes(value));
        }
        protected internal void WriteQ(long value)
        {
            WriteB(BitConverter.GetBytes(value));
        }
        protected internal void WriteQ(ulong value)
        {
            WriteB(BitConverter.GetBytes(value));
        }
        protected internal void WriteS(string value)
        {
            if (value != null)
                WriteB(Encoding.Unicode.GetBytes(value));
            WriteH(0);
        }
        protected internal void WriteS(string name, int count)
        {
            if (name == null)
                return;
            WriteB(Encoding.GetEncoding(1251).GetBytes(name));
            WriteB(new byte[count - name.Length]);
        }
        protected internal void WriteS2(string name, int count)
        {
            if (name == null)
                return;
            WriteB(Encoding.Default.GetBytes(name));
            WriteB(new byte[count - name.Length]);
        }
    }
}

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Encrypter of server headers.
    ///     Would be used by servers.
    /// </summary>
    public interface IServerEncrypter : IEncrypter
    {
        internal bool IsWrath();

        /// <summary>
        ///     Create s server header.
        /// </summary>
        public byte[] CreateServerHeader(uint size, uint opcode)
        {
            var serverSizeField = Utils.ServerSizeFieldSize(size, IsWrath());
            var b = new byte[serverSizeField + Constants.ServerOpcodeLength];

            Utils.WriteBigEndian(size, b.AsSpan()[..serverSizeField]);
            if (IsWrath() && size > 0x7FFF)
            {
                b[0] = Utils.SetBigHeader(b[0]);
            }

            Utils.WriteLittleEndian(opcode, b.AsSpan()[serverSizeField..]);

            Encrypt(b);

            return b;
        }

        /// <summary>
        ///     Writes a server header.
        /// </summary>
        public void WriteServerHeader(Span<byte> w, uint size, uint opcode)
        {
            var b = CreateServerHeader(size, opcode);
            b.CopyTo(w);
        }

        /// <summary>
        ///     Writes a server header.
        /// </summary>
        public void WriteServerHeader(byte[] w, uint size, uint opcode)
        {
            var b = CreateServerHeader(size, opcode);
            b.CopyTo(w, 0);
        }

        /// <summary>
        ///     Writes a server header.
        /// </summary>
        public void WriteServerHeader(Stream w, uint size, uint opcode)
        {
            var b = CreateServerHeader(size, opcode);
            w.Write(b);
        }

        /// <summary>
        ///     Writes a server header.
        /// </summary>
        public async Task WriteServerHeaderAsync(Stream w, uint size, uint opcode,
            CancellationToken cancellationToken = default)
        {
            var b = CreateServerHeader(size, opcode);
            await w.WriteAsync(b, cancellationToken).ConfigureAwait(false);
        }
    }
}
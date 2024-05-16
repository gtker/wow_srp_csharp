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

        private int ServerSizeFieldSize(int size)
        {
            if (IsWrath() && size > 0x7FFF)
            {
                return Constants.ServerWrathLargeSizeLength;
            }

            return Constants.ServerNormalSizeLength;
        }


        /// <summary>
        ///     Create s server header.
        /// </summary>
        public byte[] CreateServerHeader(int size, int opcode)
        {
            var serverSizeField = ServerSizeFieldSize(size);
            var b = new byte[serverSizeField + Constants.ServerOpcodeLength];

            Utils.WriteBigEndian(size, b.AsSpan()[..serverSizeField]);
            Utils.WriteLittleEndian(opcode, b.AsSpan()[serverSizeField..]);

            Encrypt(b);

            return b;
        }

        /// <summary>
        ///     Writes a server header.
        /// </summary>
        public void WriteServerHeader(Span<byte> w, int size, int opcode)
        {
            var b = CreateServerHeader(size, opcode);
            b.CopyTo(w);
        }

        /// <summary>
        ///     Writes a server header.
        /// </summary>
        public void WriteServerHeader(byte[] w, int size, int opcode)
        {
            var b = CreateServerHeader(size, opcode);
            b.CopyTo(w, 0);
        }

        /// <summary>
        ///     Writes a server header.
        /// </summary>
        public void WriteServerHeader(Stream w, int size, int opcode)
        {
            var b = CreateServerHeader(size, opcode);
            w.Write(b);
        }

        /// <summary>
        ///     Writes a server header.
        /// </summary>
        public async Task WriteServerHeaderAsync(Stream w, int size, int opcode,
            CancellationToken cancellationToken = default)
        {
            var b = CreateServerHeader(size, opcode);
            await w.WriteAsync(b, cancellationToken).ConfigureAwait(false);
        }
    }
}
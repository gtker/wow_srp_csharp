using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     A decrypter of client headers.
    ///     Would be used by the client.
    /// </summary>
    public interface IClientEncrypter : IEncrypter
    {
        /// <summary>
        ///     Creates a client header.
        /// </summary>
        public byte[] CreateClientHeader(int size, int opcode)
        {
            var b = new byte[Constants.ClientHeaderLength];

            Utils.WriteBigEndian(size, b.AsSpan()[..Constants.ClientSizeLength]);
            Utils.WriteLittleEndian(opcode, b.AsSpan()[Constants.ClientSizeLength..]);

            Encrypt(b);

            return b;
        }

        /// <summary>
        ///     Writes a client header.
        /// </summary>
        public void WriteClientHeader(Span<byte> w, int size, int opcode)
        {
            var b = CreateClientHeader(size, opcode);
            b.CopyTo(w);
        }

        /// <summary>
        ///     Writes a client header.
        /// </summary>
        public void WriteClientHeader(byte[] w, int size, int opcode)
        {
            var b = CreateClientHeader(size, opcode);
            b.CopyTo(w, 0);
        }

        /// <summary>
        ///     Writes a client header.
        /// </summary>
        public void WriteClientHeader(Stream w, int size, int opcode)
        {
            var b = CreateClientHeader(size, opcode);
            w.Write(b);
        }

        /// <summary>
        ///     Writes a client header.
        /// </summary>
        public async Task WriteClientHeaderAsync(Stream w, int size, int opcode,
            CancellationToken cancellationToken = default)
        {
            var b = CreateClientHeader(size, opcode);
            await w.WriteAsync(b, cancellationToken).ConfigureAwait(false);
        }
    }
}
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Decrypter of server messages.
    ///     Would be used by the client.
    /// </summary>
    public interface IServerDecrypter : IDecrypter
    {
        internal bool IsWrath();

        private int ServerSizeFieldLength(byte span)
        {
            if (IsWrath() && (span & 0x80) == 1)
            {
                return Constants.ServerWrathLargeSizeLength;
            }

            return Constants.ServerNormalSizeLength;
        }

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public HeaderData ReadServerHeader(Span<byte> span)
        {
            var serverSizeLength = ServerSizeFieldLength(span[0]);
            var newSpan = span[..(serverSizeLength + Constants.ServerOpcodeLength)];
            Decrypt(newSpan);

            return Utils.ReadSpans(newSpan[..serverSizeLength], newSpan[serverSizeLength..]);
        }

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public HeaderData ReadServerHeader(Stream r)
        {
            // Do all this because Wrath server messages can have a 3 byte size field
            // Vanilla and TBC may suffer slightly but only when reading messages
            // sent by the server (e.g. on the client)
            var firstByte = new byte[1];

            r.ReadUntilBufferFull(firstByte);
            var serverSizeLength = ServerSizeFieldLength(firstByte[0]);

            var remainingHeader = new byte[serverSizeLength + Constants.ServerOpcodeLength - 1];
            r.ReadUntilBufferFull(remainingHeader);

            Span<byte> header = stackalloc byte[serverSizeLength + Constants.ServerOpcodeLength];

            header[0] = firstByte[0];
            for (var i = 0; i < remainingHeader.Length; i++)
            {
                header[i + 1] = remainingHeader[i];
            }

            return ReadServerHeader(header);
        }

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public async Task<HeaderData> ReadServerHeaderAsync(Stream r, CancellationToken cancellationToken = default)
        {
            // Do all this because Wrath server messages can have a 3 byte size field
            // Vanilla and TBC may suffer slightly but only when reading messages
            // sent by the server (e.g. on the client)
            var firstByte = new byte[1];

            await r.ReadUntilBufferFullAsync(firstByte, cancellationToken).ConfigureAwait(false);
            var serverSizeLength = ServerSizeFieldLength(firstByte[0]);

            var remainingHeader = new byte[serverSizeLength + Constants.ServerOpcodeLength - 1];
            await r.ReadUntilBufferFullAsync(remainingHeader, cancellationToken).ConfigureAwait(false);

            var header = new byte[serverSizeLength + Constants.ServerOpcodeLength];

            header[0] = firstByte[0];
            for (var i = 0; i < remainingHeader.Length; i++)
            {
                header[i + 1] = remainingHeader[i];
            }

            return ReadServerHeader(header);
        }
    }
}
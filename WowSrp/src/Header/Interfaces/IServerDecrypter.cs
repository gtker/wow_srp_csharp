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

        private HeaderData ReadUnencryptedHeader(Span<byte> span)
        {
            var serverSizeLength =
                span.Length == 5 ? Constants.ServerWrathLargeSizeLength : Constants.ServerNormalSizeLength;
            return Utils.ReadSpans(span[..serverSizeLength],
                span[serverSizeLength..]);
        }

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public HeaderData ReadServerHeader(Span<byte> span)
        {
            if (!IsWrath())
            {
                var newBytes = span[..Constants.ServerNormalHeaderLength];
                Decrypt(newBytes);

                return ReadUnencryptedHeader(newBytes);
            }

            Decrypt(span[..1]);
            var serverSizeLength = Utils.ServerSizeFieldLength(span[0], IsWrath());

            span[0] = Utils.ClearBigHeader(span[0]);
            var newSpan = span[..(serverSizeLength + Constants.ServerOpcodeLength)];
            Decrypt(newSpan[1..]);

            return ReadUnencryptedHeader(newSpan);
        }

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public HeaderData ReadServerHeader(Stream r)
        {
            if (!IsWrath())
            {
                var buf = new byte[Constants.ServerNormalHeaderLength];
                r.ReadUntilBufferFull(buf);
                return ReadServerHeader(buf);
            }

            var firstByte = new byte[1];

            r.ReadUntilBufferFull(firstByte);
            Decrypt(firstByte);
            var serverSizeLength = Utils.ServerSizeFieldLength(firstByte[0], IsWrath());

            var remainingHeader = new byte[serverSizeLength + Constants.ServerOpcodeLength - 1];
            r.ReadUntilBufferFull(remainingHeader);
            Decrypt(remainingHeader);

            Span<byte> header = stackalloc byte[serverSizeLength + Constants.ServerOpcodeLength];

            header[0] = Utils.ClearBigHeader(firstByte[0]);
            remainingHeader.CopyTo(header[1..]);

            return ReadUnencryptedHeader(header);
        }

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public async Task<HeaderData> ReadServerHeaderAsync(Stream r, CancellationToken cancellationToken = default)
        {
            if (!IsWrath())
            {
                var buf = new byte[Constants.ServerNormalHeaderLength];
                await r.ReadUntilBufferFullAsync(buf, cancellationToken).ConfigureAwait(false);
                return ReadServerHeader(buf);
            }

            var firstByte = new byte[1];

            await r.ReadUntilBufferFullAsync(firstByte, cancellationToken).ConfigureAwait(false);
            Decrypt(firstByte);
            var serverSizeLength = Utils.ServerSizeFieldLength(firstByte[0], IsWrath());

            var remainingHeader = new byte[serverSizeLength + Constants.ServerOpcodeLength - 1];
            await r.ReadUntilBufferFullAsync(remainingHeader, cancellationToken).ConfigureAwait(false);
            Decrypt(remainingHeader);

            var header = new byte[serverSizeLength + Constants.ServerOpcodeLength];

            header[0] =Utils.ClearBigHeader(firstByte[0]);
            remainingHeader.CopyTo(header.AsSpan()[1..]);

            return ReadUnencryptedHeader(header);
        }
    }
}
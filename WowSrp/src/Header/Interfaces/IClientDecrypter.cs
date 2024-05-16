using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     A decrypter of client headers.
    ///     Would be used by the server.
    /// </summary>
    public interface IClientDecrypter : IDecrypter
    {
        /// <summary>
        ///     Reads a client header.
        /// </summary>
        public HeaderData ReadClientHeader(Span<byte> span)
        {
            var newSpan = span[..Constants.ClientHeaderLength];
            Decrypt(newSpan);
            return Utils.ReadSpans(newSpan[..Constants.ClientSizeLength],
                newSpan[Constants.ClientSizeLength..]);
        }

        /// <summary>
        ///     Reads a client header.
        /// </summary>
        public HeaderData ReadClientHeader(Stream r)
        {
            var header = new byte[Constants.ClientHeaderLength];
            r.ReadUntilBufferFull(header);
            return ReadClientHeader(header);
        }

        /// <summary>
        ///     Reads a client header.
        /// </summary>
        public async Task<HeaderData> ReadClientHeaderAsync(Stream r, CancellationToken cancellationToken = default)
        {
            var header = new byte[Constants.ClientHeaderLength];
            await r.ReadUntilBufferFullAsync(header, cancellationToken).ConfigureAwait(false);
            return ReadClientHeader(header);
        }
    }
}
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WowSrp.Header
{
    /// <summary>
    ///     Decrypter of server messages.
    ///     Would be used by the client.
    /// </summary>
    public interface IServerDecrypter : IDecrypter
    {
        internal bool IsWrath();

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public HeaderData ReadServerHeader(Span<byte> span) =>
            HeaderImplementations.ReadServerHeader(span, IsWrath(), Decrypt);

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public HeaderData ReadServerHeader(Stream r) => HeaderImplementations.ReadServerHeader(r, IsWrath(), Decrypt);

        /// <summary>
        ///     Read a server header.
        /// </summary>
        public async Task<HeaderData> ReadServerHeaderAsync(Stream r, CancellationToken cancellationToken = default) =>
            await HeaderImplementations.ReadServerHeaderAsync(r, IsWrath(), Decrypt, cancellationToken)
                .ConfigureAwait(false);
    }
}
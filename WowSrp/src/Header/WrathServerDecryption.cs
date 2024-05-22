using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Decrypter of wrath server headers.
    /// </summary>
    public class WrathServerDecryption : IServerDecrypter
    {
        private readonly Arc4 _arc4;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public WrathServerDecryption(byte[] sessionKey)
        {
            _arc4 = new Arc4(HeaderImplementation.CreateTbcWrathKey(sessionKey, HeaderImplementation.R),
                HeaderImplementation.DropAmount);
        }

        /// <inheritdoc cref="IDecrypter.Decrypt" />
        public void Decrypt(Span<byte> data) => _arc4.ApplyKeyStream(data);

        bool IServerDecrypter.IsWrath() => true;

        // IServerDecrypter
        /// <inheritdoc cref="IServerDecrypter.ReadServerHeader(System.Span&lt;byte&gt;)" />
        public HeaderData ReadServerHeader(Span<byte> span) => ((IServerDecrypter)this).ReadServerHeader(span);

        /// <inheritdoc cref="IServerDecrypter.ReadServerHeader(Stream)" />
        public HeaderData ReadServerHeader(Stream r) => ((IServerDecrypter)this).ReadServerHeader(r);

        /// <inheritdoc cref="IServerDecrypter.ReadServerHeaderAsync(Stream, CancellationToken)" />
        public Task<HeaderData> ReadServerHeaderAsync(Stream r, CancellationToken cancellationToken = default) =>
            ((IServerDecrypter)this).ReadServerHeaderAsync(r, cancellationToken);
    }
}
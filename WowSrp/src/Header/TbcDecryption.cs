using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Decrypter for TBC (2.0 through to 2.4.3).
    /// </summary>
    public class TbcDecryption : IServerDecrypter, IClientDecrypter
    {
        private readonly byte[] _key;
        private int _index;
        private byte _lastValue;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public TbcDecryption(byte[] sessionKey)
        {
            Utils.AssertArrayLength(sessionKey, Constants.SessionKeyLength, nameof(sessionKey));
            _key = HeaderImplementation.CreateTbcKey(sessionKey);
        }

        // IClientDecrypter
        /// <inheritdoc cref="IClientDecrypter.ReadClientHeader(Span&lt;byte&gt;)" />
        public HeaderData ReadClientHeader(Span<byte> span) => ((IClientDecrypter)this).ReadClientHeader(span);

        /// <inheritdoc cref="IClientDecrypter.ReadClientHeader(Stream)" />
        public HeaderData ReadClientHeader(Stream r) => ((IClientDecrypter)this).ReadClientHeader(r);

        /// <inheritdoc cref="IClientDecrypter.ReadClientHeaderAsync(Stream, CancellationToken)" />
        public Task<HeaderData> ReadClientHeaderAsync(Stream r, CancellationToken cancellationToken = default) =>
            ((IClientDecrypter)this).ReadClientHeaderAsync(r, cancellationToken);

        bool IServerDecrypter.IsWrath() => false;

        /// <inheritdoc cref="IDecrypter.Decrypt" />
        public void Decrypt(Span<byte> data)
        {
            HeaderImplementation.VanillaTbcDecrypt(data, _key, ref _lastValue, ref _index);
        }


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
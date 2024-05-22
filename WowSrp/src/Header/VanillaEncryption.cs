using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Encrypter for Vanilla (1.0 through to 1.12).
    /// </summary>
    public class VanillaEncryption : IServerEncrypter, IClientEncrypter
    {
        private readonly byte[] _sessionKey;
        private int _index;
        private byte _lastValue;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public VanillaEncryption(byte[] sessionKey)
        {
            Utils.AssertArrayLength(sessionKey, Constants.SessionKeyLength, nameof(sessionKey));
            _sessionKey = sessionKey;
        }

        // IClientEncrypter
        /// <inheritdoc cref="IClientEncrypter.CreateClientHeader" />
        public byte[] CreateClientHeader(uint size, uint opcode) =>
            ((IClientEncrypter)this).CreateClientHeader(size, opcode);

        /// <inheritdoc cref="IClientEncrypter.WriteClientHeader(Span&lt;byte&gt;, uint, uint)" />
        public void WriteClientHeader(Span<byte> w, uint size, uint opcode) =>
            ((IClientEncrypter)this).WriteClientHeader(w, size, opcode);

        /// <inheritdoc cref="IClientEncrypter.WriteClientHeader(byte[], uint, uint)" />
        public void WriteClientHeader(byte[] w, uint size, uint opcode) =>
            ((IClientEncrypter)this).WriteClientHeader(w, size, opcode);

        /// <inheritdoc cref="IClientEncrypter.WriteClientHeader(Stream, uint, uint)" />
        public void WriteClientHeader(Stream w, uint size, uint opcode) =>
            ((IClientEncrypter)this).WriteClientHeader(w, size, opcode);

        /// <inheritdoc cref="IClientEncrypter.WriteClientHeaderAsync(Stream, uint, uint, CancellationToken)" />
        public Task WriteClientHeaderAsync(Stream w, uint size, uint opcode,
            CancellationToken cancellationToken = default) =>
            ((IClientEncrypter)this).WriteClientHeaderAsync(w, size, opcode, cancellationToken);

        /// <inheritdoc cref="IEncrypter.Encrypt" />
        public void Encrypt(Span<byte> data)
        {
            HeaderImplementation.VanillaTbcEncrypt(data, _sessionKey, ref _lastValue, ref _index);
        }

        bool IServerEncrypter.IsWrath() => false;

        // IServerEncrypter
        /// <inheritdoc cref="IServerEncrypter.CreateServerHeader" />
        public byte[] CreateServerHeader(uint size, uint opcode) =>
            ((IServerEncrypter)this).CreateServerHeader(size, opcode);

        /// <inheritdoc cref="IServerEncrypter.WriteServerHeader(System.Span&lt;byte&gt;, uint, uint)" />
        public void WriteServerHeader(Span<byte> w, uint size, uint opcode) =>
            ((IServerEncrypter)this).WriteServerHeader(w, size, opcode);

        /// <inheritdoc cref="IServerEncrypter.WriteServerHeader(byte[], uint, uint)" />
        public void WriteServerHeader(byte[] w, uint size, uint opcode) =>
            ((IServerEncrypter)this).WriteServerHeader(w, size, opcode);

        /// <inheritdoc cref="IServerEncrypter.WriteServerHeader(Stream, uint, uint)" />
        public void WriteServerHeader(Stream w, uint size, uint opcode) =>
            ((IServerEncrypter)this).WriteServerHeader(w, size, opcode);

        /// <inheritdoc cref="IServerEncrypter.WriteServerHeaderAsync(Stream, uint, uint, CancellationToken)" />
        public Task WriteServerHeaderAsync(Stream w, uint size, uint opcode,
            CancellationToken cancellationToken = default) =>
            ((IServerEncrypter)this).WriteServerHeaderAsync(w, size, opcode, cancellationToken);
    }
}
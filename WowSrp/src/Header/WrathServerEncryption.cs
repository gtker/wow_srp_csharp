using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Encrypter of wrath server headers.
    /// </summary>
    public class WrathServerEncryption : IServerEncrypter
    {
        private readonly Arc4 _arc4;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public WrathServerEncryption(byte[] sessionKey)
        {
            _arc4 = new Arc4(HeaderImplementation.CreateTbcWrathKey(sessionKey, HeaderImplementation.R),
                HeaderImplementation.DropAmount);
        }

        /// <inheritdoc cref="IEncrypter.Encrypt" />
        public void Encrypt(Span<byte> data) => _arc4.ApplyKeyStream(data);

        bool IServerEncrypter.IsWrath() => true;

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
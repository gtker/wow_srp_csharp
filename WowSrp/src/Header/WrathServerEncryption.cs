using System;
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

        void IEncrypter.Encrypt(Span<byte> data) => _arc4.ApplyKeyStream(data);

        bool IServerEncrypter.IsWrath() => true;
    }
}
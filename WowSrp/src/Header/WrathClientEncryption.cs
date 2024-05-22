using System;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Encrypter of client headers.
    /// </summary>
    public class WrathClientEncryption : IClientEncrypter
    {
        private readonly Arc4 _arc4;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public WrathClientEncryption(byte[] sessionKey)
        {
            _arc4 = new Arc4(HeaderImplementation.CreateTbcWrathKey(sessionKey, HeaderImplementation.S),
                HeaderImplementation.DropAmount);
        }

        void IEncrypter.Encrypt(Span<byte> data) => _arc4.ApplyKeyStream(data);
    }
}
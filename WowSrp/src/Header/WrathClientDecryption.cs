using System;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Decrypter of Wrath client messages.
    /// </summary>
    public class WrathClientDecryption : IClientDecrypter
    {
        private readonly Arc4 _arc4;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public WrathClientDecryption(byte[] sessionKey)
        {
            _arc4 = new Arc4(HeaderImplementation.CreateTbcWrathKey(sessionKey, HeaderImplementation.S),
                HeaderImplementation.DropAmount);
        }

        void IDecrypter.Decrypt(Span<byte> data) => _arc4.ApplyKeyStream(data);
    }
}
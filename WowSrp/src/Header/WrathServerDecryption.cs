using System;
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

        void IDecrypter.Decrypt(Span<byte> data) => _arc4.ApplyKeyStream(data);

        bool IServerDecrypter.IsWrath() => true;
    }
}
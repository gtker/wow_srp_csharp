using System;
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

        void IDecrypter.Decrypt(Span<byte> data)
        {
            HeaderImplementation.VanillaTbcDecrypt(data, _key, ref _lastValue, ref _index);
        }

        bool IServerDecrypter.IsWrath() => false;
    }
}
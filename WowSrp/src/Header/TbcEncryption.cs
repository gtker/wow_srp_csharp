using System;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Encrypter for TBC (2.0 through to 2.4.3).
    /// </summary>
    public class TbcEncryption : IServerEncrypter, IClientEncrypter
    {
        private readonly byte[] _key;
        private int _index;
        private byte _lastValue;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public TbcEncryption(byte[] sessionKey)
        {
            Utils.AssertArrayLength(sessionKey, Constants.SessionKeyLength, nameof(sessionKey));
            _key = HeaderImplementation.CreateTbcKey(sessionKey);
        }

        void IEncrypter.Encrypt(Span<byte> data)
        {
            HeaderImplementation.VanillaTbcEncrypt(data, _key, ref _lastValue, ref _index);
        }

        bool IServerEncrypter.IsWrath() => false;
    }
}
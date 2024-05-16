using System;
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

        void IEncrypter.Encrypt(Span<byte> data)
        {
            HeaderImplementation.VanillaTbcEncrypt(data, _sessionKey, ref _lastValue, ref _index);
        }

        bool IServerEncrypter.IsWrath() => false;
    }
}
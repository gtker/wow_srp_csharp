using System;
using WowSrp.Internal;

namespace WowSrp.Header
{
    /// <summary>
    ///     Decrypter for Vanilla (1.0 through to 1.12).
    /// </summary>
    public class VanillaDecryption : IClientDecrypter, IServerDecrypter
    {
        private readonly byte[] _sessionKey;
        private int _index;
        private byte _lastValue;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public VanillaDecryption(byte[] sessionKey)
        {
            Utils.AssertArrayLength(sessionKey, Constants.SessionKeyLength, nameof(sessionKey));
            _sessionKey = sessionKey;
        }

        void IDecrypter.Decrypt(Span<byte> data)
        {
            HeaderImplementation.VanillaTbcDecrypt(data, _sessionKey, ref _lastValue, ref _index);
        }

        bool IServerDecrypter.IsWrath() => false;
    }
}
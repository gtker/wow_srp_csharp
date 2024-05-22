namespace WowSrp.Header
{
    /// <summary>
    ///     Container for header data.
    /// </summary>
    public readonly struct HeaderData
    {
        /// <summary>
        ///     Size of the message including the opcode field.
        /// </summary>
        public uint Size { get; }

        /// <summary>
        ///     Opcode of the message.
        ///     Exact meaning depends on whether this is from the client or server.
        /// </summary>
        public uint Opcode { get; }

        internal HeaderData(uint size, uint opcode)
        {
            Size = size;
            Opcode = opcode;
        }
    }
}
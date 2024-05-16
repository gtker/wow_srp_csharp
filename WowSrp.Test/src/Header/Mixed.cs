using WowSrp.Header;

namespace Tests;

public class Mixed
{
    private static readonly byte[] SessionKey =
    [
        99, 131, 9, 219, 107, 35, 248, 24, 247, 161, 213, 174, 25, 135, 70, 253, 173, 103, 149,
        186, 85, 162, 130, 144, 129, 83, 118, 179, 93, 82, 160, 128, 165, 215, 35, 125, 224, 8,
        156, 140
    ];

    [Test]
    public async Task VanillaExtraMethods()
    {
        var enc = new VanillaEncryption(SessionKey);
        var dec = new VanillaDecryption(SessionKey);

        await ServerToClient(enc, dec);
        await ClientToServer(enc, dec);
    }

    [Test]
    public async Task TbcExtraMethods()
    {
        var enc = new TbcEncryption(SessionKey);
        var dec = new TbcDecryption(SessionKey);

        await ServerToClient(enc, dec);
        await ClientToServer(enc, dec);
    }

    private static async Task ServerToClient(IServerEncrypter s, IServerDecrypter c)
    {
        var buf = new byte[6];

        s.WriteServerHeader(buf, 0x1020, 0x3040);

        var d = c.ReadServerHeader(buf);
        Assert.Multiple(() =>
        {
            Assert.That(d.Size, Is.EqualTo(0x1020));
            Assert.That(d.Opcode, Is.EqualTo(0x3040));
        });

        s.WriteServerHeader(buf.AsSpan(), 0x1525, 0x3545);

        d = c.ReadServerHeader(buf.AsSpan());
        Assert.Multiple(() =>
        {
            Assert.That(d.Size, Is.EqualTo(0x1525));
            Assert.That(d.Opcode, Is.EqualTo(0x3545));
        });

        var mem = new MemoryStream(buf);
        s.WriteServerHeader(mem, 0x1121, 0x3141);

        mem = new MemoryStream(buf);
        d = c.ReadServerHeader(mem);
        Assert.Multiple(() =>
        {
            Assert.That(d.Size, Is.EqualTo(0x1121));
            Assert.That(d.Opcode, Is.EqualTo(0x3141));
        });

        mem = new MemoryStream(buf);
        await s.WriteServerHeaderAsync(mem, 0x1121, 0x3141);

        mem = new MemoryStream(buf);
        d = await c.ReadServerHeaderAsync(mem);
        Assert.Multiple(() =>
        {
            Assert.That(d.Size, Is.EqualTo(0x1121));
            Assert.That(d.Opcode, Is.EqualTo(0x3141));
        });
    }

    private static async Task ClientToServer(IClientEncrypter s, IClientDecrypter c)
    {
        var buf = new byte[6];

        s.WriteClientHeader(buf, 0x1020, 0x3040);

        var d = c.ReadClientHeader(buf);
        Assert.Multiple(() =>
        {
            Assert.That(d.Size, Is.EqualTo(0x1020));
            Assert.That(d.Opcode, Is.EqualTo(0x3040));
        });

        s.WriteClientHeader(buf.AsSpan(), 0x1525, 0x3545);

        d = c.ReadClientHeader(buf.AsSpan());
        Assert.Multiple(() =>
        {
            Assert.That(d.Size, Is.EqualTo(0x1525));
            Assert.That(d.Opcode, Is.EqualTo(0x3545));
        });

        var mem = new MemoryStream(buf);
        s.WriteClientHeader(mem, 0x1121, 0x3141);

        mem = new MemoryStream(buf);
        d = c.ReadClientHeader(mem);
        Assert.Multiple(() =>
        {
            Assert.That(d.Size, Is.EqualTo(0x1121));
            Assert.That(d.Opcode, Is.EqualTo(0x3141));
        });

        mem = new MemoryStream(buf);
        await s.WriteClientHeaderAsync(mem, 0x1121, 0x3141);

        mem = new MemoryStream(buf);
        d = await c.ReadClientHeaderAsync(mem);
        Assert.Multiple(() =>
        {
            Assert.That(d.Size, Is.EqualTo(0x1121));
            Assert.That(d.Opcode, Is.EqualTo(0x3141));
        });
    }

    [Test]
    public void TbcRegression()
    {
        var contents = File.ReadAllLines("./tests/encryption/calculate_tbc_encrypt_values.txt");
        Assert.That(contents, Is.Not.Empty);

        foreach (var line in contents)
        {
            var split = line.Split(' ');

            var sessionKey = TestUtils.StringToByteArray(split[0]);
            var data = TestUtils.StringToByteArray(split[1]);
            var originalData = (byte[])data.Clone();
            var expected = TestUtils.StringToByteArray(split[2]);

            var encrypt = new TbcEncryption(sessionKey);

            ((IEncrypter)encrypt).Encrypt(data);
            Assert.That(data, Is.EqualTo(expected));

            var decrypt =
                new TbcDecryption(sessionKey);
            ((IDecrypter)decrypt).Decrypt(data);

            Assert.That(data, Is.EqualTo(originalData));
        }
    }
}
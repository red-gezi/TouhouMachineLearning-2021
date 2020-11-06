using Extension;
using Network.Converter;
using Network.Extensions;
using Network.Packets;

namespace Network
{
    public static class NetExtern
    {
        public static void SendMessge(this ClientConnectionContainer client, string Tag, object data)
        {
            client.Send(RawDataConverter.FromUTF8String(Tag, data.ToJson()));
        }
    }
}
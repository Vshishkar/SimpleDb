namespace DistributedKeyValueStore.Node.Config
{
    public class ConsulConfig
    {
        public string ConsulHost { get; set; }
        public int ConsulPort { get; set;}
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
    }
}

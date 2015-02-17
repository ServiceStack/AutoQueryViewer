using AutoQueryViewer.ServiceModel;
using ServiceStack;
using ServiceStack.Caching;

namespace AutoQueryViewer.ServiceInterface
{
    public class ContentCache
    {
        public ICacheClient Client { get; set; }

        public ContentCache(ICacheClient cache = null)
        {
            this.Client = cache ?? new MemoryCacheClient();
        }

        private string ClearKey(string key, bool clear)
        {
            if (clear)
                Client.ClearCaches(key);

            return key;
        }

        public string GetAutoQueryServices(bool clear = false)
        {
            var key = typeof(GetAutoQueryServices).Name;
            if (clear)
                Client.FlushAll();

            return key;
        }

        public void ClearAll()
        {
            Client.FlushAll();
        }
    }
}
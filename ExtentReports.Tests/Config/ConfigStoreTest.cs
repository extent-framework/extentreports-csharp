using AventStack.ExtentReports.Config;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Config
{
    public class ConfigStoreTest
    {
        private ConfigStore _store = new ConfigStore();

        [Test]
        public void DuplicateConfig()
        {
            _store.AddConfig("config", "value1");
            Assert.AreEqual(_store.GetConfig("config"), "value1");

            _store.AddConfig("config", "value2");
            Assert.AreEqual(_store.GetConfig("config"), "value2");
        }

        [Test]
        public void Contains()
        {
            _store.AddConfig("config", "value1");
            Assert.True(_store.Contains("config"));
            Assert.False(_store.Contains("config2"));
        }

        [Test]
        public void RemoveConfig()
        {
            _store.AddConfig("config", "value1");
            Assert.True(_store.Contains("config"));
            _store.RemoveConfig("config");
            Assert.False(_store.Contains("config"));
        }

        [Test]
        public void ConfigValueTest()
        {
            _store.AddConfig("c", "v");
            _store.AddConfig("k", "z");
            Assert.True(_store.GetConfig("c").Equals("v"));
            Assert.True(_store.GetConfig("k").Equals("z"));
        }

        [Test]
        public void ExtendConfigWithStore()
        {
            var store1 = new ConfigStore();
            store1.AddConfig("config1", "value1");
            var store2 = new ConfigStore();
            store2.AddConfig("config2", "value2");
            store1.Extend(store2.Store);
            Assert.True(store1.Contains("config1"));
            Assert.True(store1.Contains("config2"));
            Assert.True(store2.Contains("config2"));
            Assert.False(store2.Contains("config1"));
        }

        [Test]
        public void ExtendConfigWithMap()
        {
            var store1 = new ConfigStore();
            store1.AddConfig("config1", "value1");
            var store2 = new ConfigStore();
            store2.AddConfig("config2", "value2");
            store1.Extend(store2.Store);
            Assert.True(store1.Contains("config1"));
            Assert.True(store1.Contains("config2"));
            Assert.True(store2.Contains("config2"));
            Assert.False(store2.Contains("config1"));
        }

        [Test]
        public void ConfigEmpty()
        {
            Assert.True(_store.IsEmpty);

            _store.AddConfig("config1", "value1");
            Assert.False(_store.IsEmpty);
        }
    }
}

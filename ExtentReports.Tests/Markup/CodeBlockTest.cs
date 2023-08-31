using System;
using AventStack.ExtentReports.MarkupUtils;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Markup
{
    public class CodeBlockTest
    {
        [Test]
        public void NullCodeBlockContent()
        {
            var m = MarkupHelper.CreateCodeBlock((string)null);
            Assert.AreEqual(m.GetMarkup(), "");
        }

        [Test]
        public void XmlCodeBlock()
        {
            var xml = "<tag>value</tag>";
            var m = MarkupHelper.CreateCodeBlock(xml);
            Assert.True(m.GetMarkup().Contains(xml));
        }

        [Test]
        public void XmlCodeBlockWithLang()
        {
            var xml = "<tag>value</tag>";
            var m = MarkupHelper.CreateCodeBlock(xml, CodeLanguage.Xml);
            Assert.True(m.GetMarkup().Contains(xml));
        }

        [Test]
        public void JsonCodeBlock()
        {
            var json = "{ 'key': 'value' }";
            var m = MarkupHelper.CreateCodeBlock(json);
            Assert.True(m.GetMarkup().Contains(json));
        }

        [Test]
        public void JsonCodeBlockWithLang()
        {
            var json = "{ 'key': 'value' }";
            var m = MarkupHelper.CreateCodeBlock(json, CodeLanguage.Json);
            Assert.True(m.GetMarkup().Contains(json));
            Assert.True(m.GetMarkup().Contains("jsonTreeCreate"));
            Assert.True(m.GetMarkup().Contains("<script>"));
            Assert.True(m.GetMarkup().Contains("</script>"));
        }

        [Test]
        public void JsonCodeBlockWithLangMultiple()
        {
            var json = "{ 'key': 'value' }";
            var m = MarkupHelper.CreateCodeBlock(json, CodeLanguage.Json);
            Assert.True(m.GetMarkup().Contains(json));
            Assert.True(m.GetMarkup().Contains("jsonTreeCreate"));
            Assert.True(m.GetMarkup().Contains("<script>"));
            Assert.True(m.GetMarkup().Contains("</script>"));
            m = MarkupHelper.CreateCodeBlock(json, CodeLanguage.Json);
            Assert.True(m.GetMarkup().Contains(json));
            Assert.True(m.GetMarkup().Contains("jsonTreeCreate"));
            Assert.True(m.GetMarkup().Contains("<script>"));
            Assert.True(m.GetMarkup().Contains("</script>"));
        }

        [Test]
        public void MultipleCodeBlocks1()
        {
            var xml = "<tag>value</tag>";
            var m = MarkupHelper.CreateCodeBlock(new String[] { xml });
            String s = m.GetMarkup();
            Assert.True(s.Contains("col-md-12"));
        }

        [Test]
        public void MultipleCodeBlocks2()
        {
            var xml = "<tag>value</tag>";
            var m = MarkupHelper.CreateCodeBlock(new String[] { xml, xml });
            String s = m.GetMarkup();
            Assert.True(s.Contains("col-md-6"));
        }

        [Test]
        public void MultipleCodeBlocks3()
        {
            var xml = "<tag>value</tag>";
            var m = MarkupHelper.CreateCodeBlock(new String[] { xml, xml, xml });
            String s = m.GetMarkup();
            Assert.True(s.Contains("col-md-4"));
        }

        [Test]
        public void MultipleCodeBlocks4()
        {
            var xml = "<tag>value</tag>";
            var m = MarkupHelper.CreateCodeBlock(new String[] { xml, xml, xml, xml });
            String s = m.GetMarkup();
            Assert.True(s.Contains("col-md-3"));
        }
    }
}

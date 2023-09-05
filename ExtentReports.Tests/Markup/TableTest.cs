using System;
using System.Collections.Generic;
using System.Linq;
using AventStack.ExtentReports.MarkupUtils;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Markup
{
    public class TableTest
    {
        [Test]
        public void TableWithNullText()
        {
            var m = MarkupHelper.CreateTable(null);
            Assert.AreEqual(m.GetMarkup(), "");
        }

        [Test]
        public void TableWithData()
        {
            string[,] data = new string[,] { { "h1", "h2" }, { "c1", "c2" } };
            var m = MarkupHelper.CreateTable(data);
            string s = m.GetMarkup();
            Assert.True(s.Contains("<td>h1</td>"));
            Assert.True(s.Contains("<td>h2</td>"));
            Assert.True(s.Contains("<td>c1</td>"));
            Assert.True(s.Contains("<td>c2</td>"));
            Assert.True(s.Contains("<table"));
            Assert.True(s.Contains("</table>"));
        }

        [Test]
        public void TableWithBeginningEndingTags()
        {
            var m = MarkupHelper.ToTable<Foo>(new Foo());
            string s = m.GetMarkup();
            Assert.True(s.Contains("<table"));
            Assert.True(s.Contains("</table>"));
        }

        [Test]
        public void TableWithHeaders()
        {
            var m = MarkupHelper.ToTable<Foo>(new Foo());
            string s = m.GetMarkup();
            Assert.True(s.Contains("<th>Names</th>"));
            Assert.True(s.Contains("<th>Stack</th>"));
        }

        [Test]
        public void TableWithCells()
        {
            var m = MarkupHelper.ToTable<Foo>(new Foo());
            string s = m.GetMarkup();
            Assert.True(s.Contains("<td>Anshoo</td>"));
            Assert.True(s.Contains("<td>Extent</td>"));
            Assert.True(s.Contains("<td>Klov</td>"));
            Assert.True(s.Contains("<td>Java</td>"));
            Assert.True(s.Contains("<td>C#</td>"));
            Assert.True(s.Contains("<td>Angular</td>"));
        }
    }
}

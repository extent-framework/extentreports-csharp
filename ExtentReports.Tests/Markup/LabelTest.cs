using System;
using AventStack.ExtentReports.MarkupUtils;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Markup
{
    public class LabelTest
    {
        [Test]
        public void LabelWithNullText()
        {
            var m = MarkupHelper.CreateLabel(null, ExtentColor.Transparent);
            Assert.AreEqual(m.GetMarkup(), "");
        }

        [Test]
        public void LabelWithEmptyText()
        {
            var m = MarkupHelper.CreateLabel("", ExtentColor.Transparent);
            Assert.AreEqual(m.GetMarkup(), "");
        }

        [Test]
        public void LabelWithText()
        {
            String text = "Extent";
            var m = MarkupHelper.CreateLabel(text, ExtentColor.Transparent);
            Assert.True(m.GetMarkup().Contains(text));
        }
    }
}

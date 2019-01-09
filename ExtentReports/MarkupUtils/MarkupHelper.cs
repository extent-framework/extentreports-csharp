namespace AventStack.ExtentReports.MarkupUtils
{
    public class MarkupHelper
    {
        public static IMarkup CreateLabel(string text, ExtentColor color)
        {
            var label = new Label
            {
                Text = text,
                Color = color
            };
            return label;
        }

        public static IMarkup CreateLabel(string text, ExtentColor color, ExtentColor textColor)
        {
            var label = new Label
            {
                Text = text,
                Color = color,
                TextColor = textColor
            };
            return label;
        }

        public static IMarkup CreateCodeBlock(string code, CodeLanguage lang = CodeLanguage.Xml)
        {
            var cb = new CodeBlock
            {
                Code = code,
                CodeLang = lang
            };
            return cb;
        }

        public static IMarkup CreateTable(string[][] data)
        {
            var t = new Table
            {
                ArrayData = data
            };
            return t;
        }

        public static IMarkup CreateTable(string[,] data)
        {
            var t = new Table
            {
                Data = data
            };
            return t;
        }
    }
}

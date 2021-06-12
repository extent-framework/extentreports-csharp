using System.Collections;
using System.Collections.Generic;

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

        public static IMarkup CreateCodeBlock(string code, CodeLanguage lang = CodeLanguage.Xml)
        {
            if (code == null)
            {
                return new CodeBlock();
            }

            var cb = new CodeBlock
            {
                Code = new string[] { code },
                CodeLang = lang
            };
            return cb;
        }

        public static IMarkup CreateCodeBlocks(params string[] code)
        {
            var cb = new CodeBlock
            {
                Code = code
            };
            return cb;
        }

        public static IMarkup CreateTable(string[,] data)
        {
            var t = new Table<string[]>
            {
                Data = data
            };
            return t;
        }

        public static IMarkup ToTable<T>(object obj)
        {
            var t = new Table<T>
            {
                Object = obj
            };
            return t;
        }

        public static IMarkup CreateOrderedList<T>(IEnumerable<T> enumerable)
        {
            var x = new ListMarkup<T>
            {
                Type = "ol",
                Object = enumerable
            };
            return x;
        }

        public static IMarkup CreateUnorderedList<T>(IEnumerable<T> enumerable)
        {
            var x = new ListMarkup<T>
            {
                Type = "ul",
                Object = enumerable
            };
            return x;
        }
    }
}

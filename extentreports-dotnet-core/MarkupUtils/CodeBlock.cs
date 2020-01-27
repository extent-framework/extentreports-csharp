using System.Threading;

namespace AventStack.ExtentReports.MarkupUtils
{
    internal class CodeBlock : IMarkup
    {
        public string Code { get; set; }
        public CodeLanguage CodeLang { get; set; }

        private int _id { get; } = Interlocked.Increment(ref _cntr);
        private static int _cntr;

        public string GetMarkup()
        {
            if (CodeLang == CodeLanguage.Json)
            {
                return "<div class='json-tree' id='code-block-json-" + _id + "'></div>" +
                "<script>" +
                    "function jsonTreeCreate" + _id + "() { document.getElementById('code-block-json-" + _id + "').innerHTML = JSONTree.create(" + Code + "); }" +
                    "jsonTreeCreate" + _id + "();" +
                "</script>";
            }

            var lhs = "<textarea disabled class='code-block'>";
            var rhs = "</textarea>";
            return lhs + Code + rhs;
        }
    }
}

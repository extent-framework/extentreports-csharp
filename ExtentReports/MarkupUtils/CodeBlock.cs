using System.Threading;

namespace AventStack.ExtentReports.MarkupUtils
{
    internal class CodeBlock : IMarkup
    {
        public string[] Code { get; set; }
        public CodeLanguage CodeLang { get; set; } = CodeLanguage.Xml;

        private int _id { get; } = Interlocked.Increment(ref _cntr);
        private static int _cntr;

        public string GetMarkup()
        {
            if (Code == null)
            {
                return "";
            }

            if (CodeLang == CodeLanguage.Json)
            {
                return "<div class='json-tree' id='code-block-json-" + _id + "'></div>" +
                "<script>" +
                    "function jsonTreeCreate" + _id + "() { document.getElementById('code-block-json-" + _id + "').innerHTML = JSONTree.create(" + Code[0] + "); }" +
                    "jsonTreeCreate" + _id + "();" +
                "</script>";
            }

            var col = (12 / Code.Length) % 1 == 0 ? 12 / Code.Length : 12;
            var lhs = "<div class='col-md-" + col + "'><textarea readonly class='code-block'>";
            var rhs = "</textarea></div>";
            var code = string.Empty;

            foreach (var c in Code)
            {
                code += lhs + c + rhs;
            }

            return "<div class='row'>" + code + "</div>";
        }
    }
}

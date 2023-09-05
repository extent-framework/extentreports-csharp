namespace AventStack.ExtentReports.MarkupUtils
{
    internal class Label : IMarkup
    {
        public string Text { get; set; }
        public ExtentColor Color { get; set; }

        public string GetMarkup()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return "";
            }

            var textColor = Color != ExtentColor.White ? "white-text" : "black-text";
            var lhs = "<span class='badge " + textColor + " " + Color.ToString().ToLower() + "'>";
            var rhs = "</span>";
            return lhs + Text + rhs;
        }
    }
}

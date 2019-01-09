namespace AventStack.ExtentReports.MarkupUtils
{
    internal class Label : IMarkup
    {
        public string Text { get; set; }
        public ExtentColor Color { get; set; }
        public ExtentColor TextColor { get; set; } = ExtentColor.Black;

        public string GetMarkup()
        {
            var textColor = TextColor.ToString().ToLower() + "-text";
            var lhs = "<span class='badge " + textColor + " " + Color.ToString().ToLower() + "'>";
            var rhs = "</span>";
            return lhs + Text + rhs;
        }
    }
}

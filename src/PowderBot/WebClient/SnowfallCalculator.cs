using System.Globalization;
using Common.Converters;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace WebClient
{
    public static class SnowfallCalculator
    {
        public static int GetSnowfall(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            bool isCm = document
                .QuerySelectorAll("#forecast-table .snowu")
                ?.Any(e => e.InnerText == "cm")
                ?? true;
            var snowfall = document
                .QuerySelectorAll("#forecast-table .snow-amount")
                ?.Select(SnowfallCalculator.parse)
                .Sum() ?? 0;
            return isCm ? (int)snowfall : snowfall.InchToCm();
        }

        private static float parse(HtmlNode node)
        {
            if (float.TryParse(node.InnerText, CultureInfo.InvariantCulture, out float value))
            {
                return value;
            }
            return 0;
        }
    }
}

using Common.Converters;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Linq;

namespace WebClient
{
    public static class SnowfallCalculator
    {
        public static int GetSnowfall(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            bool isCm = document
                .QuerySelectorAll("tr.forecast-table-snow span.snowu")
                ?.Any(e => e.InnerText == "cm")
                ?? true;
            var snowfall = document
                .QuerySelectorAll("tr.forecast-table-snow span.snow")
                ?.Select(SnowfallCalculator.parse)
                .Sum() ?? 0;
            return isCm ? (int)snowfall : snowfall.InchToCm();
        }

        private static float parse(HtmlNode node)
        {
            if (float.TryParse(node.InnerText, out float value))
            {
                return value;
            }
            return 0;
        }
    }
}

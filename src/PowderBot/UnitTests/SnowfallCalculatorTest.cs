using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebClient;

namespace UnitTests
{
    [TestClass]
    public class SnowfallCalculatorTest
    {
        [DataTestMethod]
        [DataRow(
            @"<body>
                <tr class=""forecast-table-snow forecast-table__row"">
                    <th><div><span class=""snowu"">cm</span></div></th>
                    <td><div><span class=""snow"">-</span></div></td>
                </tr>
            </body>", 0)]
        [DataRow(
            @"<body>
                <tr class=""forecast-table-snow forecast-table__row"">
                    <th><div><span class=""snowu"">cm</span></div></th>
                    <td><div><span class=""snow"">-</span></div></td>
                    <td><div>
                        <span class=""snow has-value snow-larger-metric forecast-table-snow__value"">
                            14
                        </span>
                    </div></td>
                </tr>
            </body>", 14)]
        [DataRow(
            @"<body>
                <tr class=""forecast-table-snow forecast-table__row"">
                    <th><div><span class=""snowu"">in</span></div></th>
                    <td><div><span class=""snow"">-</span></div></td>
                    <td><div><span class=""snow"">5.5</span></div></td>
                </tr>
            </body>", 14)]
        public void GetGmtTest(string html, int snowfall)
        {
            Assert.AreEqual(snowfall, SnowfallCalculator.GetSnowfall(html));
        }
    }
}

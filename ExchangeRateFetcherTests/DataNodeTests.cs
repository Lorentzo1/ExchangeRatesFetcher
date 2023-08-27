using ExchangeRateFetcherTest.Test_Data;
using ExchangeRateWebApi.Models;
using HtmlAgilityPack;

namespace ExchangeRateFetcherTest
{
    [TestClass]
    public class DataNodeTests
    {
        public DataNodeTests()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
        new System.Globalization.CultureInfo("cs-CZ");
        }

        [TestMethod]
        [DataRow(TestData.DataNodeDataCorrectFormat)]
        public void DataNode_ConstructingWithCorrectData_ValidParse(string data)
        {
            //Arrange
            var node = Setup(data);
            //Act
            DataNode dataNode = new DataNode(node);

            //Assert
            Assert.IsNotNull(dataNode);
            Assert.AreEqual(dataNode.IsoCode, "CNY");
            Assert.IsTrue(dataNode.ExchangeRate == 3.063M);
            Assert.IsTrue(string.Equals(dataNode.CountryName, "čína", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        [DataRow(TestData.DataNodeDataWrongNumberOfElements)]
        public void DataNode_ConstructingWrongDecimalFormat_FormatExceptionExchangeRate(string data)
        {
            //Arrange
            var node = Setup(data);
            try
            {
                //Act
                DataNode dataNode = new DataNode(node);
            }
            catch (Exception ex)
            {

                //Assert
                Assert.AreEqual("Data in wrong format", ex.Message);
            }
        }

        [TestMethod]
        [DataRow(TestData.DataNodeDataWrongDecimalFormat)]
        public void DataNode_ConstructingNullString_ExpectionDataInWrongFormat(string data)
        {
            //Arrange
            var node = Setup(data);
            try
            {
                //Act
                DataNode dataNode = new DataNode(node);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual("ExchangeRate parse was not succesful", ex.Message);
            }
        }


        [TestMethod]
        [DataRow(TestData.DataNodeDataNullString)]
        public void DataNode_ConstructingWrongNumberOfElements_ArgumentNullException(string data)
        {
            //Arrange
            var node = Setup(data);
            try
            {
                //Act
                DataNode dataNode = new DataNode(node);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual("Value cannot be null.", ex.Message);
            }
        }

        private HtmlNode Setup(string data)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(data);
            var dataNodes = htmlDoc.DocumentNode.SelectSingleNode("/tr");
            return dataNodes;
        }

    }
}

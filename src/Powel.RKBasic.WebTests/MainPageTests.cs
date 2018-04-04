using FluentAssert;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Powel.RKBasic.WebTests
{
    public class MainPageTests : IDisposable
    {
        private readonly IWebDriver _driver = new ChromeDriver();

        public MainPageTests()
        {
            _driver.Url = "http:\\tdgdarkbasic001\\#";
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void Dispose()
        {
            _driver.Dispose();
        }

        [Fact]
        public void AddUpRegulationBidButton_WhenClicked_BidsTableShouldHaveOneMoreElement()
        {
            MoveToParentComponent();

            var addUpBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[1]/div"));

            var bidsTable = WaitForMultipleElements(By.ClassName("react-grid-Canvas"));
            var bidRows = bidsTable.SelectMany(x => x.FindElements(By.ClassName("txtTruncate"))).ToList();
            var bidsNumber = bidRows.Count;

            addUpBidBtn.Click();
            Thread.Sleep(5000);

            bidsTable = WaitForMultipleElements(By.ClassName("react-grid-Canvas"));
            var bidRowsAfterAdding = bidsTable.SelectMany(x => x.FindElements(By.ClassName("txtTruncate"))).ToList();
            var bidsNumberAfterAdding = bidRowsAfterAdding.Count;

            bidsNumberAfterAdding.ShouldBeEqualTo(bidsNumber + 1);

            _driver.Close();
        }

        [Fact]
        public void AddDownRegulationBidButton_WhenClicked_BidsTableShouldHaveOneMoreElement()
        {
            MoveToParentComponent();

            var addDownBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[2]/div"));

            var bidsTable = WaitForMultipleElements(By.ClassName("react-grid-Canvas"));
            var bidRows = bidsTable.SelectMany(x => x.FindElements(By.ClassName("txtTruncate"))).ToList();
            var bidsNumber = bidRows.Count;

            addDownBidBtn.Click();

            Thread.Sleep(5000);

            var bidRowsAfterAdding = bidsTable.SelectMany(x => x.FindElements(By.ClassName("txtTruncate"))).ToList();
            var bidsNumberAfterAdding = bidRowsAfterAdding.Count;

            bidsNumberAfterAdding.ShouldBeEqualTo(bidsNumber + 1);

            _driver.Close();
        }
        
        [Fact]
        public void DeleteRegulationBidButton_WhenClicked_DeleteButtonShouldBeDisplayed()
        {
            MoveToParentComponent();
            
            var addDownBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[2]/div"));
            addDownBidBtn.Click();

            var bidCheckBox = WaitForElement(By.ClassName("react-grid-checkbox-container"));
            bidCheckBox.Click();

            var container = WaitForMultipleElements(By.ClassName("_1_TekdKySuQJ_-ny_YwMCR")).First();
            var elementsInContainerWithDeleteButton = container.FindElements(By.TagName("div"));
            var deleteBidBtn = elementsInContainerWithDeleteButton.Single(x => x.Text == "Delete selected bid(s)");

            deleteBidBtn.Displayed.ShouldBeTrue();

            _driver.Close();
        }

        [Fact]
        public void DeleteRegulationBidButton_WhenClicked_AllBidsAreDeleted()
        {
            MoveToParentComponent();

            var container = WaitForMultipleElements(By.ClassName("_1_TekdKySuQJ_-ny_YwMCR")).First();

            var addDownBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[2]/div"));
            addDownBidBtn.Click();

            Thread.Sleep(500);

            var bidCheckBox = WaitForMultipleElements(By.ClassName("react-grid-checkbox-label"));
            bidCheckBox.First().Click();

            var elementsInContainerWithDeleteButton = container.FindElements(By.TagName("div"));
            var deleteBidBtn = elementsInContainerWithDeleteButton.Single(x => x.Text == "Delete selected bid(s)");

            deleteBidBtn.Click();

            Thread.Sleep(1000);

            var bidTableDisplaySpace = container.FindElements(By.TagName("p"));
            var emptyBidTableDisplaySpace = bidTableDisplaySpace.First(x => x.Text.Contains("Nothing here"));
            emptyBidTableDisplaySpace.Text.ShouldBeEqualTo("Nothing here…");

            _driver.Close();
        }

        [Fact]
        public void AddBidInParentThenAddBidInChild_MoveToSubPage_BidsForParentAreNotDisplayed()
        {
            MoveToParentComponent();

            var container = WaitForMultipleElements(By.ClassName("_1_TekdKySuQJ_-ny_YwMCR")).First();

            var addDownBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[2]/div"));
            addDownBidBtn.Click();
            Thread.Sleep(1500);

            MoveToChildComponent();

            var bidTableDisplaySpace = container.FindElements(By.TagName("p"));
            Thread.Sleep(1000);

            var emptyBidTableDisplaySpace = bidTableDisplaySpace.First(x => x.Text == "Nothing here…");
            emptyBidTableDisplaySpace.Text.ShouldBeEqualTo("Nothing here…");
        }

        [Fact]
        public void AddBidButton_WhenClicked_AddPriceShouldBeVisibleInTable()
        {
            MoveToParentComponent();

            var addUpBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[1]/div"));
            addUpBidBtn.Click();

            var priceCell = WaitForMultipleElements(By.ClassName("_1r-QxVZVy0YQI36FO1tLAJ"));
            var firstBidPriceCell = priceCell.First(x => x.Text != "Price");

            firstBidPriceCell.Text.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public void AddTwoBids_WhenSelectedTogether_AggregateBidsButtonShouldBeDisplayed()
        {
            MoveToParentComponent();

            var addUpBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[1]/div"));
            addUpBidBtn.Click();
            Thread.Sleep(1500);
            addUpBidBtn.Click();

            var bidsTable = WaitForMultipleElements(By.ClassName("react-grid-Canvas"));
            var tableCheckBoxes = bidsTable.First().FindElements(By.ClassName("react-grid-checkbox-label"));
            tableCheckBoxes[1].Click();
            Thread.Sleep(5000);
            tableCheckBoxes[2].Click();

            var aggregateBidsBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/button[2]"));

            aggregateBidsBtn.Displayed.ShouldBeTrue();
        }

        [Fact]
        public void AddTwoBids_WhenAggregateButtonIsClicked_AggregatedBidPriceShouldBeTakenFromHigherBid()
        {
            MoveToParentComponent();

            var addUpBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[1]/div"));

            addUpBidBtn.Click();
            Thread.Sleep(1500);
            addUpBidBtn.Click();

            _driver.Navigate().Refresh();
            MoveToParentComponent();

            var bidsTable = WaitForMultipleElements(By.ClassName("react-grid-Canvas"));
            var tableCheckBoxes = bidsTable.First().FindElements(By.ClassName("react-grid-checkbox-label"));
            tableCheckBoxes[0].Click();
            Thread.Sleep(5000);
            tableCheckBoxes[1].Click();

            var firstCell = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[1]/div/div/div/div/div[2]/div/div/div/div[1]/div/div[17]"));
            DoubleClickAndFillInCells(firstCell);

            var secondCell = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[1]/div/div/div/div/div[2]/div/div/div/div[2]/div/div[17]"));
            DoubleClickAndFillInCells(secondCell);

            var priceCells = WaitForMultipleElements(By.ClassName("_1r-QxVZVy0YQI36FO1tLAJ"));
            var firstBidPrice = Convert.ToInt32(priceCells[1].Text);
            var secondBidPrice = Convert.ToInt32(priceCells[2].Text);

            var aggregateBidsBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/button[2]"));
            aggregateBidsBtn.Click();

            var expectedResult = firstBidPrice > secondBidPrice ? firstBidPrice : secondBidPrice;
            var aggregatedCells = WaitForMultipleElements(By.ClassName("_1r-QxVZVy0YQI36FO1tLAJ"));
            var aggregatedCellText = aggregatedCells[1].Text;
            var aggregatedCellValue = Convert.ToInt32(aggregatedCellText);

            aggregatedCellValue.ShouldBeEqualTo(expectedResult);
        }

        [Fact]
        public void AddUpAndDownBidButton_AggregateBidButton_ShouldNotBeAvailable()
        {
            MoveToParentComponent();

            var addUpBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[1]/div"));
            addUpBidBtn.Click();
            Thread.Sleep(2000);

            var addDownBidBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/div/button[2]/div"));
            addDownBidBtn.Click();
            Thread.Sleep(2000);

            var bidsTable = WaitForMultipleElements(By.ClassName("react-grid-Canvas"));
            var tableCheckBoxes = bidsTable.First().FindElements(By.ClassName("react-grid-checkbox-label"));
            tableCheckBoxes[tableCheckBoxes.Count - 1].Click();
            Thread.Sleep(5000);
            tableCheckBoxes[tableCheckBoxes.Count - 2].Click();
            Thread.Sleep(5000);

            var aggregateBidsBtn = WaitForElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[3]/div[2]/div[2]/button[2]"));

            aggregateBidsBtn.Enabled.ShouldBeFalse();
        }

        private void MoveToParentComponent()
        {
            Thread.Sleep(10000);

            var navButtons = WaitForMultipleElements(By.ClassName("LZSNH0VM10ahesuSp7Fes"));
            navButtons.First().Click();

            var regionNavButtons = WaitForMultipleElements(By.ClassName("LZSNH0VM10ahesuSp7Fes")).ToList();
            var no1Button = regionNavButtons.Find(x => x.Text == "NO1");
            no1Button.Click();

            var childComponentButtons = WaitForMultipleElements(By.ClassName("LZSNH0VM10ahesuSp7Fes")).ToList();
            childComponentButtons.First(x => x.FindElement(By.TagName("span")).Text == "Aqua").Click();
        }

        private void MoveToChildComponent()
        {
            var regionNavButtons = WaitForMultipleElements(By.ClassName("LZSNH0VM10ahesuSp7Fes")).ToList();
            var childButton = regionNavButtons.Find(x => x.Text == "Aqua1");
            childButton.Click();
        }

        private IWebElement WaitForElement(By by)
        {
            var wasElementFound = false;
            var element = _driver.FindElement(by);

            while (!wasElementFound)
            {
                if (element != null)
                {
                    wasElementFound = true;
                    break;
                }
                Thread.Sleep(500);
            }

            return element;
        }

        private List<IWebElement> WaitForMultipleElements(By by)
        {
            var wereElementsFound = false;
            var elements = _driver.FindElements(by);

            while (!wereElementsFound)
            {
                if (elements != null)
                {
                    wereElementsFound = true;
                }
                Thread.Sleep(500);
            }

            return elements.ToList();
        }

        private void DoubleClickAndFillInCells(IWebElement webElement)
        {
            new Actions(_driver).DoubleClick(webElement).Perform();
            var bidsTable = WaitForElement(By.ClassName("react-grid-Canvas"));
            var doubleClickedCell = bidsTable.FindElement(By.TagName("input"));
            doubleClickedCell.SendKeys("10");
            doubleClickedCell.SendKeys(Keys.Return);
        }
    }
}

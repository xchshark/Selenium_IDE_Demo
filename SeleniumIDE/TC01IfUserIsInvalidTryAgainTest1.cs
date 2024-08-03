using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

[TestFixture]
public class TC01IfUserIsInvalidTryAgainTest
{
    private IWebDriver driver;
    public IDictionary<string, object> vars { get; private set; }
    private IJavaScriptExecutor js;

    [SetUp]
    public void SetUp()
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArguments("headless"); 
        options.AddArguments("no-sandbox");
        options.AddArguments("disable-dev-shm-usage");
        options.AddArguments("disable-gpu");
        options.AddArguments("window-size=1920x1080");
        // Увери се, че пътят към ChromeDriver е правилен
        driver = new ChromeDriver(options);
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<string, object>();
    }

    [TearDown]
    protected void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void tC01IfUserIsInvalidTryAgain()
    {
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.Manage().Window.Size = new Size(1552, 832);
        driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).SendKeys("user123");
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"login-password\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("*[data-test=\"login-button\"]")).Click();

        vars["errorMessage"] = driver.FindElement(By.CssSelector("*[data-test=\"error\"]")).Text;

        if (vars["errorMessage"].ToString() == "Epic sadface: Username and password do not match any user in this service")
        {
            Console.WriteLine("Wrong username");
            driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).Clear();
            driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("*[data-test=\"login-button\"]")).Click();
            Assert.That(driver.FindElement(By.CssSelector("*[data-test=\"title\"]")).Text, Is.EqualTo("Products"));
            Console.WriteLine("Successful login");
        }
    }
}

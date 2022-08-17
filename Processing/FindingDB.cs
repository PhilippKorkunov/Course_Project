using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using System.Reflection;

namespace Course_Project.Processing
{
    internal class FindingDB
    {
        static string UsersDbPath { get; set; }
        static string AuctionsDbPath { get; set; }
        static string[] Connection { get; set; }
        internal static bool IsRealPath { get; set; }

        static FindingDB()
        {
            IsRealPath = false;
            Connection = new string[2];

            Connection[0] = ConfigurationManager.ConnectionStrings["UsersDB"].ToString();
            UsersDbPath = Connection[0].Split(new char[] { ';' })[1].Split(new char[] { '=' })[1];

            Connection[1] = ConfigurationManager.ConnectionStrings["AuctionsDB"].ToString();
            AuctionsDbPath = Connection[1].Split(new char[] { ';' })[1].Split(new char[] { '=' })[1];
        }

        internal static void FindRealDbPaths()
        {
            if (!File.Exists(UsersDbPath) || !File.Exists(AuctionsDbPath))
            {
                string[] directories =  Directory.GetCurrentDirectory().Split("\\");

                UsersDbPath = "";

                for (int i = 0; i < directories.Length; i++)
                {
                   UsersDbPath += $"{directories[i]}\\";
                    if (directories[i] == "Course_Project")
                    {
                        break;
                    }
                }

                AuctionsDbPath = UsersDbPath + "AuctionsDbDirectory\\AuctionsDB.mdf";
                UsersDbPath += "UsersDbDirectory\\UsersDataBase.mdf";

                UpdateAppConfig();
            }
            else
            {
                IsRealPath = true;
            }
            
        }

        static void UpdateAppConfig()
        {
            MessageBox.Show(Connection[0] + "\n" + Connection[1]);

            var parts = Connection[0].Split(new char[] { ';' });
            var parts2 = parts[1].Split(new char[] { '=' });

            string newConnectionStringUsers = $"{parts[0]};{parts2[0]}={UsersDbPath};{parts[2]}";

            parts = Connection[1].Split(new char[] { ';' });
            parts2 = parts[1].Split(new char[] { '=' });

            string newConnectionStringAuctions = $"{parts[0]};{parts2[0]}={AuctionsDbPath};{parts[2]}";

           
        }
    }
}

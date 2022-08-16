using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace Course_Project.Processing
{
    internal class FindingDB
    {
        internal static string UsersDbPath { get; set; }
        static string AuctionsDbPath { get; set; }

        static FindingDB()
        {
            string connection = ConfigurationManager.ConnectionStrings["UsersDB"].ToString();
            UsersDbPath = connection.Split(new char[] { ';' })[1].Split(new char[] { '=' })[1];

            connection = ConfigurationManager.ConnectionStrings["AuctionsDB"].ToString();
            AuctionsDbPath = connection.Split(new char[] { ';' })[1].Split(new char[] { '=' })[1];
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

                MessageBox.Show(File.Exists(AuctionsDbPath).ToString());
            }
            
        }
    }
}

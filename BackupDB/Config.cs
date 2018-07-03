namespace BackupDB
{
    using System;
    using System.Configuration;

    public class Config
    {
        private static string connString = ConfigurationManager.AppSettings["conStr"];

        public static string ConnString
        {
            get
            {
                if (string.IsNullOrEmpty(connString))
                {
                    throw new Exception("请配置连接字符串");
                }
                return connString;
            }
        }
    }
}


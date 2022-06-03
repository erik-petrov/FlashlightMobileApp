using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProstoDajteMne3
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "friends.db";
        public static DB database;
        public static DB Database
        {
            get
            {
                if (database == null)
                {
                    database = new DB(DATABASE_NAME);
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

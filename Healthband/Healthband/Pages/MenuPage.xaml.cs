using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Healthband.Pages;
using Healthband.MenuItems;

namespace Healthband
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : MasterDetailPage
    {

        int id_user;
        public MenuPage(int _id_user)
        {
            InitializeComponent();
            id_user = _id_user;

            MenuList = new List<MasterPageItems>();

            // Adding menu items to menu List and you can define title ,page and icon
            MenuList.Add(new MasterPageItems() { Title = "Home", Icon = "home.png", TargetType = typeof(HomePage) });
            MenuList.Add(new MasterPageItems() { Title = "Heart Rate", Icon = "heartrate.png", TargetType = typeof(HeartRatePage) });
            MenuList.Add(new MasterPageItems() { Title = "Settings", Icon = "settings.png", TargetType = typeof(ConnectionPage) });
            MenuList.Add(new MasterPageItems() { Title = "LogOut", Icon = "logout.png", TargetType = typeof(LogOutPage) });

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)));

        }

        internal List<MasterPageItems> MenuList { get; private set; }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            var item = (MasterPageItems)e.SelectedItem;
            Type page = item.TargetType;

            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }


    }
}
﻿using ClearSkyMaps.Xamarin.Forms.Pages;
using ClearSkyMaps.Xamarin.Forms.Pages.Home;
using ClearSkyMaps.Xamarin.Forms.Store;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ClearSkyMaps.Xamarin.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Bootstrapper.RegisterDependencies();
            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

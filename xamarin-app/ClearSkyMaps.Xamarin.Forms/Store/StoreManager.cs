using ClearSkyMaps.Xamarin.Forms.Store.Home;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearSkyMaps.Xamarin.Forms.Store
{
    public static class StoreManager
    {
        public static IStore<HomePageState> HomePageStore;
        public static void InitHomePageStore()
        {
            HomePageStore = new Store<HomePageState>(reducer: HomePageReducer.Execute, initialState: new HomePageState());
        }
    }
}

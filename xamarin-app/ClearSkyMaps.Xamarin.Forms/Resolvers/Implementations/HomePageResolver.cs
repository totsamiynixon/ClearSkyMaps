using ClearSkyMaps.Xamarin.Forms.Pages;
using ClearSkyMaps.Xamarin.Forms.Resolvers.Interfaces;
using ClearSkyMaps.Xamarin.Forms.Services.Interfaces;
using ClearSkyMaps.Xamarin.Forms.Store.Home;
using ClearSkyMaps.Xamarin.Forms.Store.Home.Actions;
using ClearSkyMaps.Xamarin.Forms.ViewModels.Home;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ClearSkyMaps.Xamarin.Forms.Resolvers.Implementations
{
    //public class HomePageResolver : IHomePageResolver
    //{
    //    private readonly IApiClientService _apiClientService;
    //    private readonly IStore<HomePageState> _homePageStore;
    //    private readonly INavigation _navigation;
    //    public HomePageResolver(IApiClientService apiClientService, IStore<HomePageState> store, INavigation navigation)
    //    {
    //        _apiClientService = apiClientService;
    //        _homePageStore = store;
    //        _navigation = navigation;
    //    }
    //    public async Task<HomePage> ResolveAsync(object resolveParams = null)
    //    {
    //        await Task.Factory.StartNew(async () =>
    //       {
    //           var result = await _apiClientService.GetSensorsAsync();
    //           _homePageStore.Dispatch(new SetSensorsAction(result));
    //       });
    //        var vm = new SensorsMapViewModel(_navigation);
    //        var page = new HomePage();
    //        page.BindingContext = vm;
    //        page.Content = new Map
    //        {
    //            VerticalOptions = LayoutOptions.FillAndExpand
    //        };
    //        _homePageStore.Subscribe(state =>
    //        {

    //            if (state.LastAction is SetSensorsAction)
    //            {
    //                Device.BeginInvokeOnMainThread(() =>
    //                {
    //                    vm.Pins = new List<Pin>();
    //                });
    //            }
    //        });

    //        return page;
    //    }
    //}
}

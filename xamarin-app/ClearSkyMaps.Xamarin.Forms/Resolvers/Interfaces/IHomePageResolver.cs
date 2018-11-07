using ClearSkyMaps.Xamarin.Forms.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClearSkyMaps.Xamarin.Forms.Resolvers.Interfaces
{
    public interface IHomePageResolver
    {
        Task<HomePage> ResolveAsync(object resolveParams = null);
    }
}

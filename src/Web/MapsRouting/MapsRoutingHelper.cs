using Itinero;
using Itinero.Osm.Vehicles;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Web.MapsRouting
{
    public static class MapsRoutingHelper
    {
        private const string _mapFileName = "latest-map.osm.pbf";
        public static void InitializeMap(IKernel kernel)
        {

        }

        private static void DownloadlMap()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://download.geofabrik.de/europe/belarus-latest.osm.pbf", HttpContext.Current.Server.MapPath($"~/{_mapFileName}"));
            }
        }

        private static void InitializeRouterDb()
        {
            var routerDb = new RouterDb();
            using (var stream = new FileInfo(@"/path/to/some/osmfile.osm.pbf").OpenRead())
            {
                // create the network for cars only.
                //routerDb.LoadOsmData(stream, Vehicle.Car);
            }
        }

        private static void DeleteMap()
        {

        }
    }
}
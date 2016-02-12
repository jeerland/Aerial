using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace ScreenSaver
{
    // Parses http://a1.phobos.apple.com/us/r1000/000/Features/atv/AutumnResources/videos/entries.json
    public class AerialContext
    {
        static List<Asset> cachedPlaylist;

        public List<Asset> GetMovies()
        {
            var settings = new RegSettings();
            
            var aerialUrl = "http://a1.phobos.apple.com/us/r1000/000/Features/atv/AutumnResources/videos/entries.json";
            WebClient webClient = new WebClient();
            string ee = webClient.DownloadString(aerialUrl);

            var urls = new JavaScriptSerializer().Deserialize<IdAsset[]>(ee);
            // fetch local files
            var downloadedFiles = new HashSet<string>(urls.SelectMany(s => s.assets).Select(a => a.url.Split('/').Last().ToLower()));
            var localAssets = new List<Asset>();
            foreach (var f in new DirectoryInfo(settings.CacheDir).EnumerateFiles())
            {
                var filename = Path.GetFileName(f.FullName).ToLower();
                if (!downloadedFiles.Contains(filename))
                {
                    localAssets.Add(new Asset() { accessibilityLabel = filename, filename = f.FullName, timeOfDay = "any", type = "video", id = filename });
                }
            }
            if (localAssets.Count > 0)
            {
                var newUrls = urls.ToList();
                newUrls.Add(new IdAsset() { id = "local", assets = localAssets.ToArray() });
                urls = newUrls.ToArray();
            }

            var time = (DateTime.Now.Hour < 6 || DateTime.Now.Hour > 19) ? "night" : "day";
            var ran = new Random();
            var links = urls.SelectMany(s => s.assets)
                .OrderBy(t => ran.Next())
//                .Where(t => t.filename != null) // uncomment to use only local files
                .OrderByDescending(t => settings.UseTimeOfDay && (t.timeOfDay == time || t.timeOfDay == "any"))
                .ToList();

            if (settings.DifferentMoviesOnDual)
                return links;
            if (cachedPlaylist == null)
                cachedPlaylist = links;

            return cachedPlaylist;
        }
    }

    public class IdAsset
    {
        public string id;
        public Asset[] assets;
    }

    public class Asset
    {
        public string url;//" : "http://a1.phobos.apple.com/us/r1000/000/Features/atv/AutumnResources/videos/b1-1.mov",
        public string accessibilityLabel;//" : "Hawaii",
        public string type;//" : "video",
        public string id;// : "b1-1",
        public string timeOfDay;//" : "day"
        public string filename;// local filename
    }
}

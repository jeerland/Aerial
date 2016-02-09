using Microsoft.Win32;
using System;
using System.IO;

namespace ScreenSaver
{
    public class RegSettings
    {
        string keyAddress = @"SOFTWARE\AerialScreenSaver";
        public bool DifferentMoviesOnDual = false;
        public bool UseTimeOfDay = true;
        public bool MultiscreenDisabled = true;
        public bool CacheVideos = false;
        public bool SharedCache = false;
        public string CacheDir = "";

        public RegSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyAddress);
            if (key != null)
            {
                DifferentMoviesOnDual = bool.Parse(key.GetValue("DifferentMoviesOnDual") as string ?? "True");
                UseTimeOfDay = bool.Parse(key.GetValue("UseTimeOfDay") as string ?? "True");
                MultiscreenDisabled = bool.Parse(key.GetValue("MultiscreenDisabled") as string ?? "True");
                CacheVideos = bool.Parse(key.GetValue("CacheVideos") as string ?? "True");
                SharedCache = CacheVideos && bool.Parse(key.GetValue("SharedCache") as string ?? "True");
                CacheDir = key.GetValue("CacheDir") as string ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Aerial");
            }
        }

        /// <summary>
        /// Save text into the Registry.
        /// </summary>
        public void SaveSettings()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(keyAddress);
            
            key.SetValue("DifferentMoviesOnDual", DifferentMoviesOnDual);
            key.SetValue("UseTimeOfDay", UseTimeOfDay);
            key.SetValue("MultiscreenDisabled", MultiscreenDisabled);
            key.SetValue("CacheVideos", CacheVideos);
            key.SetValue("SharedCache", SharedCache);
            key.SetValue("CacheDir", CacheDir);
        }
    }
}

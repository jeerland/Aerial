﻿using System;
using System.Windows.Forms;

namespace ScreenSaver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Load display text from the Registry
        /// </summary>
        private void LoadSettings()
        {
            var settings = new RegSettings();
            chkDifferentMonitorMovies.Checked = settings.DifferentMoviesOnDual;
            chkUseTimeOfDay.Checked = settings.UseTimeOfDay;
            chkMultiscreenDisabled.Checked = settings.MultiscreenDisabled;
            chkCacheVideos.Checked = settings.CacheVideos;
            chkSharedLocation.Checked = settings.SharedCache;
            tbSharedLocation.Text = settings.CacheDir;
        }

        /// <summary>
        /// Save text into the Registry.
        /// </summary>
        private void SaveSettings()
        {
            var settings = new RegSettings();
            settings.DifferentMoviesOnDual = chkDifferentMonitorMovies.Checked;
            settings.UseTimeOfDay = chkUseTimeOfDay.Checked;
            settings.MultiscreenDisabled = chkMultiscreenDisabled.Checked;
            settings.CacheVideos = chkCacheVideos.Checked;
            settings.SharedCache = chkSharedLocation.Checked;
            settings.CacheDir = tbSharedLocation.Text;

            settings.SaveSettings();
            
        }
        

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkCacheVideos_CheckedChanged(object sender, EventArgs e)
        {
            chkSharedLocation.Enabled = chkCacheVideos.Checked;
            tbSharedLocation.Enabled = chkCacheVideos.Checked;
        }
    }
}

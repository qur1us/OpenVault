using System;
using System.IO;
using System.Windows;
using System.Text.Json;
using System.Windows.Shell;
using System.Windows.Forms;
using System.Diagnostics;
using Application = System.Windows.Application;
using System.Collections.Generic;

namespace OpenVault
{
    public class AppConfig
    {
        public List<string>? VaultNames { get; set; }
        public bool Tray {  get; set; }
        public bool Taskbar { get; set; }

        public AppConfig(List<string>? vaultNames, bool tray, bool taskbar)
        {
            VaultNames = vaultNames;
            Tray = tray;
            Taskbar = taskbar;
        }
    }

    public partial class App : Application
    {
        private AppConfig? ReadConfig(string jsonFilePath)
        {
            try
            {
                string jsonString = File.ReadAllText(jsonFilePath);

                AppConfig? config = JsonSerializer.Deserialize<AppConfig>(jsonString);

                if (config == null)
                {
                    return null;
                }

                if (config.VaultNames == null)
                {
                    System.Windows.Forms.MessageBox.Show($"Error parsing the config file {jsonFilePath}\n\n{jsonString}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
                
                return config;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show($"Error reading config file {jsonFilePath}\n\n{e.Message}", "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return null;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppConfig? config = ReadConfig("..\\..\\..\\config.json");

            if (config == null)
            {
                Environment.Exit(1);
            }

            if (config.Taskbar == true)
            {
                // Add jump tasks to use if the app is pinned to the taskbar
                JumpList jl = new JumpList();

                foreach (string vaultName in config.VaultNames)
                {
                    JumpTask jt = new JumpTask();

                    jt.ApplicationPath = $"obsidian://open/?vault={vaultName}";
                    jt.Title = vaultName;

                    jl.JumpItems.Add(jt);
                }

                JumpList.SetJumpList(Current, jl);
            }
            
            if (config.Tray == true)
            {
                // Add tray icon to make OpenVault available every time from system tray
                NotifyIcon TrayIcon = new NotifyIcon();
                ContextMenuStrip menu = new ContextMenuStrip();

                TrayIcon.Icon = new System.Drawing.Icon("..\\..\\..\\Resources\\obsidian.ico");
                TrayIcon.ContextMenuStrip = menu;

                foreach (string vaultName in config.VaultNames)
                {
                    EventHandler handler = (sender, e) => StartVault(sender, e, vaultName);

                    ToolStripMenuItem item = new ToolStripMenuItem(vaultName);
                    item.Click += new EventHandler(handler);

                    menu.Items.Add(item);
                }

                ToolStripMenuItem exitMenu = new ToolStripMenuItem("Exit");
                exitMenu.Click += new EventHandler(Exit);
                menu.Items.Add(exitMenu);

                TrayIcon.Visible = true;
            }   
        }

        public void StartVault(object sender, EventArgs e, string vaultName)
        {
            string url = $"obsidian://open/?vault={vaultName}";

            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = true;
            info.FileName = url;

            Process.Start(info);
        }

        public void Exit(object sender, EventArgs e)
        {
            Current.Shutdown();
        }
    }
}

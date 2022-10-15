using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;
using System.Windows.Forms;
using System.Diagnostics;
using Application = System.Windows.Application;

namespace OpenVault
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Rightclick on the app added to the taskbar
            JumpList jl = new JumpList();

            // Personal
            JumpTask personalVault = new JumpTask();
            personalVault.ApplicationPath = "obsidian://open/?vault=Personal";
            personalVault.Title = "Personal";

            // EOSec
            JumpTask eosecVault = new JumpTask();
            eosecVault.ApplicationPath = "obsidian://open/?vault=EO Security";
            eosecVault.Title = "EO Security";

            // Infosec
            JumpTask infosecVault = new JumpTask();
            infosecVault.ApplicationPath = "obsidian://open/?vault=Information Security";
            infosecVault.Title = "Information Security";

            jl.JumpItems.Add(personalVault);
            jl.JumpItems.Add(eosecVault);
            jl.JumpItems.Add(infosecVault);
            
            JumpList.SetJumpList(App.Current, jl);


            // System tray
            NotifyIcon TrayIcon = new NotifyIcon();
            ContextMenuStrip menu = new ContextMenuStrip();

            TrayIcon.Icon = new System.Drawing.Icon("..\\..\\..\\Resources\\obsidian.ico");
            TrayIcon.ContextMenuStrip = menu;

            ToolStripMenuItem personal = new ToolStripMenuItem("Personal");
            personal.Click += new EventHandler(StartPersonalVault);
            
            ToolStripMenuItem infosec = new ToolStripMenuItem("EO Security");
            infosec.Click += new EventHandler(StartWorkVault);
            
            ToolStripMenuItem work = new ToolStripMenuItem("Information Security");
            work.Click += new EventHandler(StartInfosecVault);

            ToolStripMenuItem exitMenu = new ToolStripMenuItem("Exit");
            exitMenu.Click += new EventHandler(Exit);

            menu.Items.Add(personal);
            menu.Items.Add(work);
            menu.Items.Add(infosec);
            menu.Items.Add(exitMenu);  

            TrayIcon.Visible = true;
        }

        public void StartPersonalVault(object sender, EventArgs e)
        {
            string url = "obsidian://open/?vault=Personal";

            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = true;
            info.FileName = url;
            Process.Start(info);
        }

        public void StartWorkVault(object sender, EventArgs e)
        {
            string url = "obsidian://open/?vault=EO Security";

            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = true;
            info.FileName = url;
            Process.Start(info);
        }

        public void StartInfosecVault(object sender, EventArgs e)
        {
            string url = "obsidian://open/?vault=Information Security";

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

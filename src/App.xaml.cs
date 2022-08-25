using System.Windows;
using System.Windows.Shell;

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

            JumpList jl = new JumpList();

            // Personal
            JumpTask personalVault = new JumpTask();
            personalVault.ApplicationPath = "obsidian://open/?vault=Personal";
            personalVault.Title = "Personal";

            // School
            JumpTask schoolVault = new JumpTask();
            schoolVault.ApplicationPath = "obsidian://open/?vault=school";
            schoolVault.Title = "School";

            // Infosec
            JumpTask infosecVault = new JumpTask();
            infosecVault.ApplicationPath = "obsidian://open/?vault=Information Security";
            infosecVault.Title = "Information Security";

            // EOSec
            JumpTask eosecVault = new JumpTask();
            eosecVault.ApplicationPath = "obsidian://open/?vault=EO Security";
            eosecVault.Title = "EO Security";


            jl.JumpItems.Add(personalVault);
            jl.JumpItems.Add(schoolVault);
            jl.JumpItems.Add(infosecVault);
            jl.JumpItems.Add(eosecVault);

            JumpList.SetJumpList(App.Current, jl);

            App.Current.Shutdown();
        }
    }
}

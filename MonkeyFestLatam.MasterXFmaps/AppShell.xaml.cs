namespace MonkeyFestLatam.MasterXFmaps
{
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public partial class AppShell : Shell
    {
        public ICommand AgendaMonkeyFestCommand => new Command<string>((url) => Device.OpenUri(new Uri(url)));

        public ICommand HelpMeMonkeyFestCommand => new Command<string>((url) => Device.OpenUri(new Uri(url)));

        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}

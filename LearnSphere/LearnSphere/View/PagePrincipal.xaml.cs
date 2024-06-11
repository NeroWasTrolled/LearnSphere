using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearnSphere.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePrincipal : Shell
    {
        public PagePrincipal()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PageLogin), typeof(PageLogin));
            Routing.RegisterRoute(nameof(PageHome), typeof(PageHome));
            Routing.RegisterRoute(nameof(PagePublicar), typeof(PagePublicar));
            Routing.RegisterRoute(nameof(PagePerfil), typeof(PagePerfil));
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Target.Location.OriginalString == "//PagePublicar" ||
                args.Target.Location.OriginalString == "//PagePerfil")
            {
                if (!LoginManager.IsUserLoggedIn)
                {
                    args.Cancel();
                    await Shell.Current.GoToAsync($"{nameof(PageLogin)}");
                }
            }
        }
    } 
}
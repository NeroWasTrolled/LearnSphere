using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LearnSphere.View;
using LearnSphere.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnSphere
{
    public partial class App : Application
    {
        public static Usuarios UsuarioLogado { set; get; }

        public static String DbName;
        public static String DbPath;

        public App()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PageLogin), typeof(PageLogin));
            Routing.RegisterRoute(nameof(PageHome), typeof(PageHome));
            Routing.RegisterRoute(nameof(PagePublicar), typeof(PagePublicar));
            Routing.RegisterRoute(nameof(PagePerfil), typeof(PagePerfil));

            MainPage = new PagePrincipal();
        }

        public App(string dbPath, string dbName)
        {
            InitializeComponent();
            App.DbName = dbName;
            App.DbPath = dbPath;
            MainPage = new PagePrincipal();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static async Task NavigateToPage(Page page)
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage != null)
            {
                var allowedPagesWithoutLogin = new HashSet<Type>
            {
                typeof(PageHome),
                typeof(PagePrincipal),
                typeof(SearchPage),
                typeof(PageCursos)
            };

                if (allowedPagesWithoutLogin.Contains(page.GetType()))
                {
                    await navigationPage.PushAsync(page);
                }
                else if (LoginManager.IsUserLoggedIn)
                {
                    if (page is PagePublicar && !LoginManager.IsUserFornecedor())
                    {
                        await Application.Current.MainPage.DisplayAlert("Acesso Negado", "Você precisa ser um provedor para publicar um curso.", "OK");
                        await Shell.Current.GoToAsync("//PageHome");
                    }
                    else
                    {
                        await navigationPage.PushAsync(page);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Acesso Negado", "Você precisa estar logado para acessar essa página.", "OK");
                    await Shell.Current.GoToAsync("//PageLogin");
                }
            }
        }
    }
}

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

				// Verifica se a página atual não requer login e permite o acesso
				if (allowedPagesWithoutLogin.Contains(page.GetType()))
				{
					await navigationPage.PushAsync(page);
				}
				// Verifica se o usuário está logado antes de permitir o acesso
				else if (LoginManager.IsUserLoggedIn)
				{
					// Verifica se a página é a página de publicação e se o usuário é um provedor
					if (page.GetType() == typeof(PagePublicar) && LoginManager.GetLoggedInUser().fornecedor)
					{
						await navigationPage.PushAsync(page);
					}
					else
					{
						// Exibe mensagem de erro e redireciona para a página inicial
						await Application.Current.MainPage.DisplayAlert("Acesso Negado", "Você precisa ser um provedor para acessar essa página.", "OK");
						await navigationPage.PopToRootAsync(); // Redireciona para a página inicial
					}
				}
				// Caso contrário, redireciona para a página de login
				else
				{
					await Application.Current.MainPage.DisplayAlert("Acesso Negado", "Você precisa estar logado para acessar essa página.", "OK");
					await navigationPage.PushAsync(new PageLogin());
				}
			}
		}
	}
}

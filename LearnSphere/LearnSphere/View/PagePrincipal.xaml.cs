using Xamarin.Forms;

namespace LearnSphere.View
{
	public partial class PagePrincipal : Shell
	{
		public PagePrincipal()
		{
			InitializeComponent();
			RegisterRoutes();
		}

		private void RegisterRoutes()
		{
			Routing.RegisterRoute(nameof(PageLogin), typeof(PageLogin));
			Routing.RegisterRoute(nameof(PageHome), typeof(PageHome));
			Routing.RegisterRoute(nameof(PagePublicar), typeof(PagePublicar));
			Routing.RegisterRoute(nameof(PagePerfil), typeof(PagePerfil));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			HandleLoginStatus();
		}

		private async void HandleLoginStatus()
		{
			if (!LoginManager.IsUserLoggedIn)
			{
				await GoToAsync($"{nameof(PageLogin)}");
			}
		}
	}
}

using LearnSphere.Controller;
using LearnSphere.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageHome : ContentPage
	{
		public PageHome()
		{
			InitializeComponent();
			_ = CarregarCursos();  // Use _ = para ignorar o retorno da Task
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await CarregarCursos();
			await Shell.Current.Navigation.PopToRootAsync();
		}

		private async Task CarregarCursos()
		{
			await Device.InvokeOnMainThreadAsync(() =>
			{
				try
				{
					var cursos = MySQLCon.ListarCursos();
					if (cursos != null && cursos.Any())
					{
						foreach (var curso in cursos)
						{
							curso.CarregarImagem();
						}
						cursosListView.ItemsSource = cursos;
					}
				}
				catch (Exception ex)
				{
					DisplayAlert("Erro", $"Erro ao carregar cursos: {ex.Message}", "OK");
				}
			});
		}

		private async void cursosListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			var cursoSelecionado = (Cursos)e.SelectedItem;

			try
			{
				await Navigation.PushAsync(new PageCursos(cursoSelecionado));
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro", $"Erro ao navegar para o curso: {ex.Message}", "OK");
			}

			cursosListView.SelectedItem = null;
		}

		private async void OnSearchButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SearchPage());
		}

		private async void OnCartClicked(object sender, EventArgs e)
		{
			if (App.UsuarioLogado != null)
			{
				await Navigation.PushAsync(new PageCarrinho());
			}
			else
			{
				await DisplayAlert("Acesso Restrito", "Você precisa estar logado para acessar o carrinho.", "OK");
				await Navigation.PushAsync(new PageLogin());
			}
		}
	}
}

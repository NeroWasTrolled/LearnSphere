using LearnSphere.Controller;
using LearnSphere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageHome : ContentPage
	{
		public PageHome()
		{
			InitializeComponent();
			CarregarCursos();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			CarregarCursos();
		}

		private void CarregarCursos()
		{
			Device.BeginInvokeOnMainThread(() =>
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
	}
}

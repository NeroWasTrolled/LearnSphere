using LearnSphere.Controller;
using LearnSphere.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageCursosAdquiridos : ContentPage
	{
		public PageCursosAdquiridos()
		{
			InitializeComponent();
			CarregarCursosAdquiridos();
		}

		private void CarregarCursosAdquiridos()
		{
			int usuarioId = LoginManager.GetLoggedInUserId();
			if (usuarioId != -1)
			{
				try
				{
					List<Cursos> cursosAdquiridos = CCompras.ListarCursosAdquiridos(usuarioId);
					if (cursosAdquiridos.Count > 0)
					{
						cursosListView.ItemsSource = cursosAdquiridos;
						cursosListView.IsVisible = true;
						lblSemCursos.IsVisible = false;
					}
					else
					{
						cursosListView.IsVisible = false;
						lblSemCursos.IsVisible = true;
					}
				}
				catch (Exception ex)
				{
					DisplayAlert("Erro", $"Erro ao carregar cursos adquiridos: {ex.Message}", "OK");
				}
			}
			else
			{
				cursosListView.IsVisible = false;
				lblSemCursos.IsVisible = true;
			}
		}

		private void CursosListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				Cursos curso = e.SelectedItem as Cursos;
				((ListView)sender).SelectedItem = null;
			}
		}
	}
}

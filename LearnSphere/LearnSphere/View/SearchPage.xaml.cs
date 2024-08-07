﻿using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LearnSphere.Controller;
using LearnSphere.Models;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{
		private List<Cursos> todosCursos;

		public SearchPage()
		{
			InitializeComponent();
			CarregarCursos();
		}

		private void CarregarCursos()
		{
			todosCursos = MySQLCon.ListarCursos();
			cursosListView.ItemsSource = todosCursos;
		}

		private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
		{
			var textoPesquisa = searchBar.Text.ToLower();
			var cursosFiltrados = todosCursos.Where(c => c.titulo.ToLower().Contains(textoPesquisa)).ToList();
			cursosListView.ItemsSource = cursosFiltrados;
		}

		private async void cursosListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem is Cursos selectedCurso)
			{
				await Navigation.PushAsync(new PageCursos(selectedCurso));
				cursosListView.SelectedItem = null; 
			}
		}
	}
}

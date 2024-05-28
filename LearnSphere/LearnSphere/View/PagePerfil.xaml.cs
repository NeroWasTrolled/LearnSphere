using System;
using System.Collections.Generic;
using LearnSphere.Controller;
using LearnSphere.Models;
using Xamarin.Forms;

namespace LearnSphere.View
{
	public partial class PagePerfil : ContentPage
	{
		public PagePerfil()
		{
			InitializeComponent();
			AtualizarDadosUsuario();
		}

		private void AtualizarDadosUsuario()
		{
			if (App.UsuarioLogado != null)
			{
				lblNomeUsuario.Text = $"Olá, {App.UsuarioLogado.usuario}";
			}
			else
			{
				lblNomeUsuario.Text = "Olá, Usuário";
			}
		}

		private async void Perfil_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new EditPerfilPage());
		}

		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new PageCursosAdquiridos());

		}
	}
}

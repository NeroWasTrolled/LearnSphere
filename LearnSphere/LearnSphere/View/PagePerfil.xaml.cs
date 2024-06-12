using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
			try
			{
				await Shell.Current.GoToAsync($"{nameof(EditPerfilPage)}");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro", $"Erro ao navegar para EditPerfilPage: {ex.Message}", "OK");
			}
		}

		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			try
			{
				await Shell.Current.GoToAsync($"{nameof(PageCursosAdquiridos)}");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro", $"Erro ao navegar para PageCursosAdquiridos: {ex.Message}", "OK");
			}
		}

		private async Task VerificarLogin()
		{
			if (!LoginManager.IsUserLoggedIn)
			{
				await Shell.Current.GoToAsync($"{nameof(PageLogin)}");
			}
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await VerificarLogin();
		}

		private async void Carrinho_Clicked(object sender, EventArgs e)
		{
			try
			{
				await Shell.Current.GoToAsync($"{nameof(PageCarrinho)}");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro", $"Erro ao navegar para PageCarrinho: {ex.Message}", "OK");
			}
		}
	}
}

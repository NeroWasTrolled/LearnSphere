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

			if (App.UsuarioLogado != null && App.UsuarioLogado.fornecedor)
			{
				CarregarCursosFornecedor();
			}
			else
			{
				lblMensagemCursos.Text = "Bem-vindo ao seu perfil!";
				listViewCursos.IsVisible = false;
			}
		}

		private async void CarregarCursosFornecedor()
		{
			try
			{
				var cursos = await Task.Run(() => MySQLCon.ListarCursosPorFornecedor(App.UsuarioLogado.id));
				if (cursos.Count > 0)
				{
					listViewCursos.ItemsSource = cursos;
					listViewCursos.IsVisible = true;
					lblMensagemCursos.IsVisible = false;
				}
				else
				{
					lblMensagemCursos.Text = "Você ainda não publicou nenhum curso.";
					listViewCursos.IsVisible = false;
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro", $"Erro ao carregar cursos: {ex.Message}", "OK");
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

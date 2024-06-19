using LearnSphere.Controller;
using LearnSphere.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageCarrinho : ContentPage
	{
		public PageCarrinho()
		{
			InitializeComponent();
			CarregarCursosNoCarrinho();
		}

		private void CarregarCursosNoCarrinho()
		{
			int usuarioId = LoginManager.GetLoggedInUserId();
			if (usuarioId != -1)
			{
				try
				{
					List<Cursos> cursosNoCarrinho = CCarrinho.ListarCursosNoCarrinho(usuarioId);
					if (cursosNoCarrinho.Count > 0)
					{
						carrinhoListView.ItemsSource = cursosNoCarrinho;
						carrinhoListView.IsVisible = true;
						lblSemCursos.IsVisible = false;
						btnComprarTudo.IsVisible = true;
					}
					else
					{
						carrinhoListView.IsVisible = false;
						lblSemCursos.IsVisible = true;
						btnComprarTudo.IsVisible = false;
					}
				}
				catch (Exception ex)
				{
					DisplayAlert("Erro", $"Erro ao carregar cursos no carrinho: {ex.Message}", "OK");
				}
			}
			else
			{
				DisplayAlert("Erro", "Usuário não está logado. Faça o login para ver seu carrinho.", "OK");
			}
		}

		private void OnComprarTudoClicked(object sender, EventArgs e)
		{
			modalPagamento.IsVisible = true;
		}

		private async void OnPixClicked(object sender, EventArgs e)
		{
			int usuarioId = LoginManager.GetLoggedInUserId();
			if (usuarioId != -1)
			{
				try
				{
					CCarrinho.TransferirDoCarrinhoParaCompras(usuarioId);
					modalPagamento.IsVisible = false;
					await DisplayAlert("Sucesso", "Pagamento via Pix realizado com sucesso!", "OK");
					CarregarCursosNoCarrinho();
				}
				catch (Exception ex)
				{
					await DisplayAlert("Erro", $"Erro ao realizar pagamento: {ex.Message}", "OK");
				}
			}
			else
			{
				await DisplayAlert("Erro", "Usuário não está logado. Faça o login para comprar.", "OK");
			}

			modalPagamento.IsVisible = false;
		}

		private async void OnCartaoClicked(object sender, EventArgs e)
		{
			int usuarioId = LoginManager.GetLoggedInUserId();
			if (usuarioId != -1)
			{
				try
				{
					CCarrinho.TransferirDoCarrinhoParaCompras(usuarioId);
					modalPagamento.IsVisible = false;
					await DisplayAlert("Sucesso", "Pagamento via Cartão realizado com sucesso!", "OK");
					CarregarCursosNoCarrinho();
				}
				catch (Exception ex)
				{
					await DisplayAlert("Erro", $"Erro ao realizar pagamento: {ex.Message}", "OK");
				}
			}
			else
			{
				await DisplayAlert("Erro", "Usuário não está logado. Faça o login para comprar.", "OK");
			}

			modalPagamento.IsVisible = false;
		}

		private void OnCancelarClicked(object sender, EventArgs e)
		{
			modalPagamento.IsVisible = false;
		}
	}
}

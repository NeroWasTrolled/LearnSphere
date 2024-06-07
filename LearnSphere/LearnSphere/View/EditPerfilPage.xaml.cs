using System;
using Xamarin.Forms;
using LearnSphere.Controller;
using LearnSphere.Models;

namespace LearnSphere.View
{
	public partial class EditPerfilPage : ContentPage
	{
		private Usuarios usuarioLogado;

		public EditPerfilPage()
		{
			InitializeComponent();
			CarregarDadosUsuario();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			usuarioLogado = LoginManager.GetLoggedInUser();
			CarregarDadosUsuario();
		}

		private void CarregarDadosUsuario()
		{
			if (App.UsuarioLogado != null)
			{
				entryUsuario.Text = App.UsuarioLogado.usuario;
				entryEmail.Text = App.UsuarioLogado.email;
				entryTelefone.Text = App.UsuarioLogado.celular;
				entrySenha.Text = App.UsuarioLogado.senha;
				entryCPF.Text = App.UsuarioLogado.cpf;

				// Verifica se o usuário é fornecedor e define o estado do switch
				if (App.UsuarioLogado.fornecedor)
				{
					FornecedorSwitch.IsToggled = true; // Define como ativado
				}
				else
				{
					FornecedorSwitch.IsToggled = false; // Define como desativado
				}
				Console.WriteLine($"CarregarDadosUsuario: Fornecedor = {App.UsuarioLogado.fornecedor}");
			}
		}


		private void Salvar_Clicked(object sender, EventArgs e)
		{
			if (usuarioLogado != null)
			{
				usuarioLogado.usuario = entryUsuario.Text;
				usuarioLogado.email = entryEmail.Text;
				usuarioLogado.celular = entryTelefone.Text;
				usuarioLogado.senha = entrySenha.Text;
				usuarioLogado.cpf = entryCPF.Text;
				usuarioLogado.fornecedor = FornecedorSwitch.IsToggled;

				Users.AtualizarUser(usuarioLogado);

				DisplayAlert("Sucesso", Users.StatusMessage, "OK");
			}
		}

		private async void Desconectar_Clicked(object sender, EventArgs e)
		{
			LoginManager.Logout();
			Application.Current.MainPage = new PagePrincipal();
		}
	}
}

using Xamarin.Forms;
using LearnSphere.Models;
using LearnSphere.Controller;
using System;

namespace LearnSphere.View
{
	public partial class EditPerfilPage : ContentPage
	{
		private Usuarios usuarioLogado;

		public EditPerfilPage()
		{
			InitializeComponent();
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

				FornecedorSwitch.IsToggled = App.UsuarioLogado.fornecedor;
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

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
			usuarioLogado = LoginManager.GetLoggedInUser();
			CarregarDadosUsuario();
		}

		private void CarregarDadosUsuario()
		{
			if (usuarioLogado != null)
			{
				entryUsuario.Text = usuarioLogado.usuario;
				entryEmail.Text = usuarioLogado.email;
				entryTelefone.Text = usuarioLogado.celular;
				entrySenha.Text = usuarioLogado.senha;
				switchTipoUsuario.IsToggled = usuarioLogado.fornecedor;
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
				usuarioLogado.fornecedor = switchTipoUsuario.IsToggled;

				MySQLCon.AtualizarUser(usuarioLogado);

				DisplayAlert("Sucesso", MySQLCon.StatusMessage, "OK");
			}
		}

		private async void Desconectar_Clicked(object sender, EventArgs e)
		{
			LoginManager.Logout();
			Application.Current.MainPage = new PagePrincipal();
		}

	}
}

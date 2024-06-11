using System;
using LearnSphere.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageLogin : ContentPage
	{
		public PageLogin()
		{
			InitializeComponent();
			NavigationPage.SetHasBackButton(this, false); // Desabilita o botão de volta
			NavigationPage.SetHasNavigationBar(this, false); // Desabilita a barra de navegação
		}

		private async void BtnEntrar_Clicked(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtLogin.Text) ||
				string.IsNullOrWhiteSpace(txtSenhaLogin.Text))
			{
				await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
				return;
			}

			if (!IsValidEmail(txtLogin.Text))
			{
				await DisplayAlert("Erro", "Por favor, insira um email válido.", "OK");
				return;
			}

			bool loginSucesso = await LoginManager.LocalizarUser(this, txtLogin.Text, null, null, txtSenhaLogin.Text);

			if (loginSucesso)
			{
				Usuarios usuarioLogado = LoginManager.GetLoggedInUser();
				App.UsuarioLogado = usuarioLogado;
				await DisplayAlert("Sucesso", "Login bem-sucedido.", "OK");

				Application.Current.MainPage = new PagePrincipal();
			}
			else
			{
				await DisplayAlert("Erro de Login", "Usuário não encontrado ou senha incorreta.", "OK");
			}

			LimparCampos();
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		private void LimparCampos()
		{
			txtLogin.Text = string.Empty;
			txtSenhaLogin.Text = string.Empty;
		}

		private void CriarConta_Tapped(object sender, EventArgs e)
		{
			Navigation.PushAsync(new PageCadastrar());
		}
	}
}

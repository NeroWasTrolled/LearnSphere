using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnSphere.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LearnSphere.Controller;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageCadastrar : ContentPage
	{
		public PageCadastrar()
		{
			InitializeComponent();
		}

		private async void BtnCadastrar_Clicked(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtusuario.Text) ||
				string.IsNullOrWhiteSpace(txtemail.Text) ||
				string.IsNullOrWhiteSpace(txtcelular.Text) ||
				string.IsNullOrWhiteSpace(txtsenha.Text))
			{
				await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
				return;
			}

			if (!IsValidEmail(txtemail.Text))
			{
				await DisplayAlert("Erro", "Por favor, insira um email válido.", "OK");
				return;
			}

			if (!IsValidPhoneNumber(txtcelular.Text))
			{
				await DisplayAlert("Erro", "Por favor, insira um número de celular válido.", "OK");
				return;
			}

			if (txtsenha.Text.Length < 7)
			{
				await DisplayAlert("Erro", "A senha deve ter no mínimo 7 caracteres.", "OK");
				return;
			}

			Usuarios novoUsuario = new Usuarios
			{
				usuario = txtusuario.Text,
				email = txtemail.Text,
				celular = FormatPhoneNumber(txtcelular.Text),
				senha = txtsenha.Text
			};

			MySQLCon.InserirUser(novoUsuario);

			App.UsuarioLogado = novoUsuario;

			await DisplayAlert("Sucesso", MySQLCon.StatusMessage, "OK");

			await Navigation.PushAsync(new PageHome());
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

		private bool IsValidPhoneNumber(string phoneNumber)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{11}$");
		}

		private string FormatPhoneNumber(string phoneNumber)
		{
			return System.Text.RegularExpressions.Regex.Replace(phoneNumber, @"(\d{2})(\d{5})(\d{4})", "($1) $2-$3");
		}

		private void LimparCampos()
		{
			txtusuario.Text = string.Empty;
			txtemail.Text = string.Empty;
			txtcelular.Text = string.Empty;
			txtsenha.Text = string.Empty;
		}

		private async void Login_Tapped(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new PageLogin());
		}
	}
}
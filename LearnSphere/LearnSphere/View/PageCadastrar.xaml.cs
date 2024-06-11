using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LearnSphere.Models;
using LearnSphere.Controller;
using System.Linq;

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
            if (!IsValidCPF(txtcpf.Text))
            {
                await DisplayAlert("Erro", "Por favor, insira um CPF válido.", "OK");
                return;
            }

            if (Users.VerificarCPFCadastrado(txtcpf.Text))
            {
                await DisplayAlert("Erro", "CPF já cadastrado.", "OK");
                return;
            }

            Usuarios novoUsuario = new Usuarios
            {
                usuario = txtusuario.Text,
                email = txtemail.Text,
                celular = FormatPhoneNumber(txtcelular.Text),
                senha = txtsenha.Text,
                cpf = txtcpf.Text
            };

            Users.InserirUser(novoUsuario);

            App.UsuarioLogado = novoUsuario;

            await DisplayAlert("Sucesso", Users.StatusMessage, "OK");

            await Shell.Current.GoToAsync($"//PageHome");
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

        private bool IsValidCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digitoVerificador1)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[10].ToString()) != digitoVerificador2)
                return false;

            return true;
        }

        private void OnTogglePasswordVisibility(object sender, EventArgs e)
        {
            txtsenha.IsPassword = !txtsenha.IsPassword;
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

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

                if (App.UsuarioLogado.admin)
                {
                    adminStackLayout.IsVisible = true;
                }
            }
            else
            {
                lblNomeUsuario.Text = "Olá, Usuário";
            }
        }

        private async void Perfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPerfilPage());
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageCursosAdquiridos());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!LoginManager.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new PageLogin());
            }

            AtualizarDadosUsuario();
        }

        private async void Carrinho_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageCarrinho());
        }

        private async void Admin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageValidarCurso());
        }
    }
}

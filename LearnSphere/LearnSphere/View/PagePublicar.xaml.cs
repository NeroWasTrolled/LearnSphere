using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using LearnSphere.Models;
using LearnSphere.Controller;
using System.Collections.Generic;

namespace LearnSphere.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePublicar : ContentPage
    {
        private Cursos novoCurso;
        private List<Cursos> listaCursos;

        public PagePublicar()
        {
            InitializeComponent();
            VerificarLoginEPermissao();
        }

        public PagePublicar(Cursos curso)
        {
            InitializeComponent();
            listaCursos = new List<Cursos>();
            novoCurso = new Cursos();
            VerificarLoginEPermissao();
        }

        private async void VerificarLoginEPermissao()
        {
            if (!LoginManager.IsUserLoggedIn)
            {
                await Shell.Current.GoToAsync($"{nameof(PageLogin)}");
                return;
            }

            if (!LoginManager.IsUserFornecedor())
            {
                await DisplayAlert("Acesso Negado", "Você precisa ser um provedor para publicar um curso.", "OK");
                await Shell.Current.GoToAsync($"//{nameof(PageHome)}");
            }
        }

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.Photos>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Erro", "Permissão para acessar fotos não concedida.", "OK");
                    return;
                }

                var foto = await MediaPicker.PickPhotoAsync();
                if (foto != null)
                {
                    imagemFoto.Source = ImageSource.FromFile(foto.FullPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao selecionar foto: {ex.Message}", "OK");
            }
        }

        private async void btnPublicar_Clicked(object sender, EventArgs e)
        {
            if (imagemFoto.Source == null)
            {
                await DisplayAlert("Erro", "Por favor, selecione uma imagem.", "OK");
                return;
            }

            byte[] fotoBytes = GetImageBytes(imagemFoto.Source);

            Cursos novoCurso = new Cursos
            {
                titulo = txtTitulo.Text,
                subtitulo = txtSubtitulo.Text,
                foto = fotoBytes,
                desc_principal = txtDesc.Text,
                desc_secundaria = txtDescSecundaria.Text,
                criador = txtAutor.Text,
                duracao = txtDuracao.Text,
                atualizacao = dtAtualizacao.Date,
                preco = decimal.Parse(txtPreco.Text),
                conteudo = txtConteudo.Text,
                publicado = false, // Define como não publicado inicialmente
                fornecedorid = App.UsuarioLogado.id // Certifique-se de que App.UsuarioLogado.id é do tipo int
            };

            MySQLCon.Inserir(novoCurso);
            await DisplayAlert("Status", MySQLCon.StatusMessage, "OK");
        }

        private byte[] GetImageBytes(ImageSource imageSource)
        {
            if (imageSource is FileImageSource fileImageSource)
            {
                string filePath = fileImageSource.File;
                if (!string.IsNullOrEmpty(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
            }
            return null;
        }

        private void dtAtualizacao_DateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime dataSelecionada = e.NewDate;

            novoCurso.atualizacao = dataSelecionada;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            VerificarLoginEPermissao();
        }
    }
}

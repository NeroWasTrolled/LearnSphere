using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LearnSphere.Controller;
using LearnSphere.Models;

namespace LearnSphere.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCursosValidar : ContentPage
    {
        private Cursos curso;
        private bool descricaoCompleta = false;
        private const int MAX_DESC_LENGTH = 200;

        public PageCursosValidar(Cursos curso)
        {
            InitializeComponent();
            this.curso = curso;

            cursoImage.Source = ImageSource.FromStream(() => new MemoryStream(curso.foto));
            cursoNome.Text = curso.titulo;
            cursoSubtitulo.Text = curso.subtitulo;
            cursoAutor.Text = curso.criador;
            cursoData.Text = curso.atualizacao.ToString("dd/MM/yyyy");
            cursoDescSecundaria.Text = curso.desc_secundaria;
            cursoDuracao.Text = curso.duracao;

            LimitarDescricao();

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += MostrarMaisLabel_Tapped;
            mostrarMaisLabel.GestureRecognizers.Add(tapGesture);
        }

        private void LimitarDescricao()
        {
            if (!string.IsNullOrEmpty(curso.desc_principal) && curso.desc_principal.Length > MAX_DESC_LENGTH)
            {
                cursoDescricao.Text = curso.desc_principal.Substring(0, MAX_DESC_LENGTH) + "...";
                mostrarMaisLabel.IsVisible = true;
            }
            else
            {
                cursoDescricao.Text = curso.desc_principal;
                mostrarMaisLabel.IsVisible = false;
            }
        }

        private void MostrarMaisLabel_Tapped(object sender, EventArgs e)
        {
            if (descricaoCompleta)
            {
                cursoDescricao.MaxLines = 3;
                cursoDescricao.Text = curso.desc_principal.Substring(0, MAX_DESC_LENGTH) + "...";
                mostrarMaisLabel.Text = "Mostrar mais";
            }
            else
            {
                cursoDescricao.MaxLines = int.MaxValue;
                cursoDescricao.Text = curso.desc_principal;
                mostrarMaisLabel.Text = "Mostrar menos";
            }

            descricaoCompleta = !descricaoCompleta;
        }

        private async void OnPublicarClicked(object sender, EventArgs e)
        {
            try
            {
                curso.publicado = true;
                MySQLCon.Atualizar(curso);
                await DisplayAlert("Sucesso", "Curso publicado com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao publicar o curso: {ex.Message}", "OK");
            }
        }

        private async void OnRemoverClicked(object sender, EventArgs e)
        {
            try
            {
                MySQLCon.RemoverCurso(curso.id);
                await DisplayAlert("Removido", "Curso removido da plataforma.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao remover o curso: {ex.Message}", "OK");
            }
        }
    }
}

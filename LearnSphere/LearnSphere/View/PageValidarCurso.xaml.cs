using LearnSphere.Controller;
using LearnSphere.Models;
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearnSphere.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageValidarCurso : ContentPage
    {
        public PageValidarCurso()
        {
            InitializeComponent();
            CarregarCursosNaoPublicados();
        }

        private void CarregarCursosNaoPublicados()
        {
            try
            {
                List<Cursos> cursosNaoPublicados = MySQLCon.ListarCursosNaoPublicados();
                listViewCursos.ItemsSource = cursosNaoPublicados;
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", $"Erro ao carregar cursos não publicados: {ex.Message}", "OK");
            }
        }

        private async void ListViewCursos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Cursos curso)
            {
                await Navigation.PushAsync(new PageCursosValidar(curso));
            }
        }
    }
}

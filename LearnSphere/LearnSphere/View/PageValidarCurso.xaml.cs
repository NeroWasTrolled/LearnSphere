using LearnSphere.Models;
using LearnSphere.Controller;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LearnSphere.View
{
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

        private async void OnCursoTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Cursos curso)
            {
                bool publicar = await DisplayAlert("Publicar Curso", $"Deseja publicar o curso {curso.titulo}?", "Sim", "Não");
                if (publicar)
                {
                    try
                    {
                        MySQLCon.AtualizarPublicacaoCurso(curso.id, true);
                        CarregarCursosNaoPublicados();
                        await DisplayAlert("Sucesso", "Curso publicado com sucesso.", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", $"Erro ao publicar curso: {ex.Message}", "OK");
                    }
                }
            }
        }

        private void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            if (sender is Switch toggleSwitch && toggleSwitch.BindingContext is Cursos curso)
            {
                MySQLCon.AtualizarPublicacaoCurso(curso.id, toggleSwitch.IsToggled);
                CarregarCursosNaoPublicados();
            }
        }
    }
}

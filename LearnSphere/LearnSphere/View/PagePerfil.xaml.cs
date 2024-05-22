using LearnSphere.Controller;
using LearnSphere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearnSphere.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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

                // Obter cursos adquiridos pelo usuário
                List<Cursos> cursosAdquiridos = MySQLCon.ListarCursosPorUsuario(App.UsuarioLogado.id);

                // Exibir a quantidade de cursos adquiridos
                lblQuantidadeCursos.Text = cursosAdquiridos.Count.ToString();

                // Exibir detalhes dos cursos adquiridos, se houver
                foreach (var curso in cursosAdquiridos)
                {
                    // Exibir detalhes do curso, por exemplo, título do curso
                    // lblDetalhesCursos.Text += curso.titulo + ", ";
                }
            }
            else
            {
                lblNomeUsuario.Text = "Olá, Usuário";
                lblQuantidadeCursos.Text = "0";
            }
        }

        private async void Perfil_Clicked(object sender, EventArgs e)
        {
			await Navigation.PushModalAsync(new EditPerfilPage());
		}
    }
}
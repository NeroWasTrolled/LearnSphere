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
	public partial class PageCursosAdquiridos : ContentPage
	{
		public PageCursosAdquiridos()
		{
			InitializeComponent();
			CarregarCursosAdquiridos();
		}

		private void CarregarCursosAdquiridos()
		{
			// Verificar se há um usuário logado
			if (App.UsuarioLogado != null)
			{
				// Obter os cursos adquiridos pelo usuário logado
				List<Cursos> cursosAdquiridos = MySQLCon.ListarCursosAdquiridosPorUsuario(App.UsuarioLogado.id);

				// Verificar se há cursos adquiridos
				if (cursosAdquiridos.Count > 0)
				{
					// Exibir os cursos adquiridos na ListView
					cursosListView.ItemsSource = cursosAdquiridos;
				}
				else
				{
					// Caso não haja cursos adquiridos, exibir uma mensagem
					lblSemCursos.Text = "Você ainda não adquiriu nenhum curso.";
					lblSemCursos.IsVisible = true;
					cursosListView.IsVisible = false;
				}
			}
			else
			{
				// Caso não haja usuário logado, exibir uma mensagem
				lblSemCursos.Text = "Faça login para ver seus cursos adquiridos.";
				lblSemCursos.IsVisible = true;
				cursosListView.IsVisible = false;
			}
		}

		//private async void cursosListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		//{
		//	if (e.SelectedItem == null)
		//		return;

		//	// Deselecionar o item
		//	cursosListView.SelectedItem = null;

		//	// Obter o curso selecionado
		//	Cursos cursoSelecionado = (Cursos)e.SelectedItem;

		//	// Navegar para a página de detalhes do curso
		//	await Navigation.PushAsync(new PageCursos(cursoSelecionado));
		//}
	}
}

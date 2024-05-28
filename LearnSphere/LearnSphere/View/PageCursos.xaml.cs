using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnSphere.Controller;
using LearnSphere.Models;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageCursos : ContentPage
	{
		private Cursos curso;
		private bool descricaoCompleta = false;
		private const int MAX_DESC_LENGTH = 200;

		public PageCursos(Cursos curso)
		{
			InitializeComponent();
			this.curso = curso;

			cursoImage.Source = ImageSource.FromStream(() => new MemoryStream(curso.foto));
			cursoNome.Text = curso.titulo;
			cursoAutor.Text = curso.criador;

			LimitarDescricao();

			FillStarRating(curso.estrelas);
		}

		private void LimitarDescricao()
		{
			if (!string.IsNullOrEmpty(curso.desc_principal) && curso.desc_principal.Length > MAX_DESC_LENGTH)
			{
				cursoDescricao.Text = curso.desc_principal.Substring(0, MAX_DESC_LENGTH);
				mostrarMaisButton.IsVisible = true;
			}
			else
			{
				cursoDescricao.Text = curso.desc_principal;
				mostrarMaisButton.IsVisible = false;
			}
		}

		private void MostrarMaisButton_Clicked(object sender, EventArgs e)
		{
			if (descricaoCompleta)
			{
				cursoDescricao.MaxLines = 3;
				cursoDescricao.Text = curso.desc_principal.Substring(0, MAX_DESC_LENGTH);
				mostrarMaisButton.Text = "Mostrar mais";
			}
			else
			{
				cursoDescricao.MaxLines = int.MaxValue;
				cursoDescricao.Text = curso.desc_principal;
				mostrarMaisButton.Text = "Mostrar menos";
			}

			descricaoCompleta = !descricaoCompleta;
		}

		private void FillStarRating(int rating)
		{
			avaliacaoStack.Children.Clear();
			for (int i = 0; i < rating; i++)
			{
				Label starLabel = new Label
				{
					Text = "★",
					FontSize = 20,
					TextColor = Color.Yellow
				};
				avaliacaoStack.Children.Add(starLabel);
			}
		}

		private async void OnComprarClicked(object sender, EventArgs e)
		{
			try
			{
				if (App.UsuarioLogado != null)
				{
					Compras compra = new Compras
					{
						IdCurso = curso.id,
						IdUsuario = App.UsuarioLogado.id
					};

					CCompras.InserirCompra(compra);

					await DisplayAlert("Sucesso", "Compra efetuada com sucesso!", "OK");
				}
				else
				{
					await DisplayAlert("Erro", "Usuário não está logado. Faça o login para efetuar a compra.", "OK");
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro", $"Erro ao efetuar a compra: {ex.Message}", "OK");
			}
		}
	}
}
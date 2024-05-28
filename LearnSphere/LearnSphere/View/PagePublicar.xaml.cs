using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using LearnSphere.Models;
using LearnSphere.Controller;
using System.Collections.Generic;
using LearnSphere.Controller;
using LearnSphere.Models;

namespace LearnSphere.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PagePublicar : ContentPage
	{
		private int star;
		private Cursos novoCurso;
		private List<Cursos> listaCursos;
		public PagePublicar()
		{
			InitializeComponent();
		}

		public PagePublicar(Cursos curso)
		{
			InitializeComponent();
			listaCursos = new List<Cursos>();
			novoCurso = new Cursos();

		}

		private void Reset()
		{
			ChangeTextColor(5, Color.Gray);
		}

		private void ChangeTextColor(int starcount, Color color)
		{
			for (int i = 1; i <= starcount; i++)
			{
				(FindByName($"star{i}") as Label).TextColor = color;
				star = i;
			}
		}

		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			Reset();
			Label clicked = sender as Label;
			ChangeTextColor(Convert.ToInt32(clicked.StyleId.Substring(4, 1)), Color.Yellow);
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
				estrelas = star,
				criador = txtAutor.Text,
				duracao = txtDuracao.Text,
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
	}
}

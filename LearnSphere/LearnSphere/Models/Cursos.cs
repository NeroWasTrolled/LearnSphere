using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace LearnSphere.Models
{
	public class Cursos
	{
		public int id { get; set; }
		public string titulo { get; set; }
		public string subtitulo { get; set; }
		public byte[] foto { get; set; }
		public string desc_principal { get; set; }
		public string desc_secundaria { get; set; }
		public DateTime atualizacao { get; set; }
		public int estrelas { get; set; }
		public string criador { get; set; }
		public string duracao { get; set; }

		public ImageSource Imagem { get; set; }

		public void CarregarImagem()
		{
			if (foto != null && foto.Length > 0)
			{
				Imagem = ImageSource.FromStream(() => new MemoryStream(foto));
			}
			else
			{
				Imagem = ImageSource.FromFile("imagem_padrao.png");
			}
		}
	}
}

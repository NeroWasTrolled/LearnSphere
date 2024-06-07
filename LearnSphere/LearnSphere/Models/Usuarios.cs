using LearnSphere.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Models
{
	public class Usuarios
	{
		public int id { get; set; }
		public string usuario { get; set; }
		public string email { get; set; }
		public string cpf { get; set; }
		public string celular { get; set; }
		public string senha { get; set; }
		public bool fornecedor { get; set; }

		public List<Cursos> Cursos { get; set; }
		public List<Compras> CursosAdquiridos { get; set; }

		public Usuarios()
		{
			Cursos = new List<Cursos>();
			CursosAdquiridos = new List<Compras>();
		}
	}
}

using System;

namespace LearnSphere.Models
{
	public class CursosAdquiridos
	{
		public int Id { get; set; }
		public int IdUsuario { get; set; }
		public int IdCurso { get; set; }
		public DateTime DataCompra { get; set; }

		public CursosAdquiridos()
		{
			DataCompra = DateTime.Now;
		}
	}
}

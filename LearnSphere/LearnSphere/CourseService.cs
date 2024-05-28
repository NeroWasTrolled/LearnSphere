using System;
using System.Collections.Generic;
using LearnSphere.Models;
using LearnSphere.Controller;
using System.Threading.Tasks;
using static Xamarin.Essentials.Permissions;
using LearnSphere.Controller;
using LearnSphere.Models;
using LearnSphere;

namespace LearnSphere
{
	public class CourseService
	{
		public List<Cursos> GetCourses()
		{
			return MySQLCon.ListarCursos();
		}

		public void AddCourse(Cursos curso)
		{
			MySQLCon.Inserir(curso);
		}

		public void UpdateCourse(Cursos curso)
		{
			MySQLCon.Atualizar(curso);
		}

		public async Task BuyCourse(Usuarios user, Cursos curso)
		{
			bool confirmarCompra = false;

			confirmarCompra = await App.Current.MainPage.DisplayAlert("Confirmação", "Deseja comprar este item?", "Sim", "Não");

			if (confirmarCompra)
			{
				user.Cursos.Add(curso);

				Users.AtualizarUser(user);

				await App.Current.MainPage.DisplayAlert("Compra efetuada", "Curso comprado com sucesso!", "OK");
			}
		}

		public async Task<bool> EfetuarCompra(Usuarios usuario, Cursos curso)
		{
			try
			{
				// Verifica se o usuário e o curso são válidos
				if (usuario != null && curso != null)
				{
					// Cria uma nova compra com o ID do curso e do usuário
					var compra = new Compras
					{
						IdCurso = curso.id,
						IdUsuario = usuario.id
					};

					// Insere a compra no banco de dados
					CCompras.InserirCompra(compra);

					// Retorna true indicando que a compra foi efetuada com sucesso
					return true;
				}
				else
				{
					// Retorna false se o usuário ou o curso forem nulos
					return false;
				}
			}
			catch (Exception ex)
			{
				// Em caso de erro, lança uma exceção e retorna false
				Console.WriteLine($"Erro ao efetuar a compra: {ex.Message}");
				return false;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySqlConnector;
using LearnSphere.Models;
using System.IO;
using System.Linq;

namespace LearnSphere.Controller
{
	public class MySQLCon
	{
		static string conn = @"server=sql10.freesqldatabase.com;port=3306;database=sql10714026;user=sql10714026;password=pf5lL1idAD";
		public static string StatusMessage { get; set; }

		public static List<Cursos> ListarCursos()
		{
			List<Cursos> listacursos = new List<Cursos>();
			try
			{
				string sql = "SELECT * FROM cursos";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						using (MySqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								Cursos cursos = new Cursos()
								{
									id = reader.GetInt32("id"),
									titulo = reader.GetString("titulo"),
									subtitulo = reader.GetString("subtitulo"),
									foto = reader.IsDBNull(reader.GetOrdinal("foto")) ? null : (byte[])reader["foto"],
									desc_principal = reader.GetString("desc_principal"),
									desc_secundaria = reader.GetString("desc_secundaria"),
									atualizacao = reader.GetDateTime("atualizacao"),
									estrelas = reader.GetInt32("estrelas"),
									criador = reader.GetString("criador"),
									duracao = reader.GetString("duracao")
								};
								listacursos.Add(cursos);
							}
						}
					}
				}
				if (listacursos.Count > 0)
				{
					StatusMessage = "Cursos encontrados.";
				}
				else
				{
					StatusMessage = "Nenhum Curso encontrado.";
				}
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao listar Cursos: {ex.Message}";
			}
			return listacursos;
		}

		public static void SalvarFoto(Cursos imagem, byte[] fotoBytes)
		{
			if (fotoBytes == null || fotoBytes.Length == 0)
			{
				throw new ArgumentException("Os bytes da foto não podem ser nulos ou vazios.", nameof(fotoBytes));
			}

			try
			{
				string caminhoFotos = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "foto");

				if (!Directory.Exists(caminhoFotos))
				{
					Directory.CreateDirectory(caminhoFotos);
				}

				string nomeArquivo = $"{imagem.id}.png";
				string caminhoArquivo = Path.Combine(caminhoFotos, nomeArquivo);

				File.WriteAllBytes(caminhoArquivo, fotoBytes);

				imagem.foto = File.ReadAllBytes(caminhoArquivo);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao salvar a foto do Curso.", ex);
			}
		}

		public static void Inserir(Cursos cursos)
		{
			try
			{
				string sql = "INSERT INTO cursos(titulo, subtitulo, foto, desc_principal, desc_secundaria, atualizacao, estrelas, criador, duracao) " +
							 "VALUES (@titulo, @subtitulo, @foto, @descPri, @descSec, @atualizacao, @estrelas, @criador, @duracao)";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@titulo", cursos.titulo);
						cmd.Parameters.AddWithValue("@subtitulo", cursos.subtitulo);
						if (cursos.foto != null)
						{
							cmd.Parameters.AddWithValue("@foto", cursos.foto);
						}
						else
						{
							cmd.Parameters.AddWithValue("@foto", new byte[0]);
						}
						cmd.Parameters.AddWithValue("@descPri", cursos.desc_principal);
						cmd.Parameters.AddWithValue("@descSec", cursos.desc_secundaria);
						cmd.Parameters.AddWithValue("@atualizacao", cursos.atualizacao.Date);
						cmd.Parameters.AddWithValue("@estrelas", cursos.estrelas);
						cmd.Parameters.AddWithValue("@criador", cursos.criador);
						cmd.Parameters.AddWithValue("@duracao", cursos.duracao);
						cmd.ExecuteNonQuery();
					}
					con.Close();
				}

				SalvarFoto(cursos, cursos.foto);
				StatusMessage = "Curso enviado para análise!!!";
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro no cadastro de Curso: {ex.Message}";
				throw;
			}
		}

		public static void Atualizar(Cursos cursos)
		{
			try
			{
				string sql = "UPDATE cursos SET titulo = @titulo, subtitulo = @subtitulo, " +
							 "desc_principal = @descPri, desc_secundaria = @descSec, " +
							 "atualizacao = @atualizacao, estrelas = @estrelas, " +
							 "criador = @criador, duracao = @duracao WHERE id = @id";

				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@titulo", cursos.titulo);
						cmd.Parameters.AddWithValue("@subtitulo", cursos.subtitulo);
						cmd.Parameters.AddWithValue("@descPri", cursos.desc_principal);
						cmd.Parameters.AddWithValue("@descSec", cursos.desc_secundaria);
						cmd.Parameters.AddWithValue("@atualizacao", cursos.atualizacao);
						cmd.Parameters.AddWithValue("@estrelas", cursos.estrelas);
						cmd.Parameters.AddWithValue("@criador", cursos.criador);
						cmd.Parameters.AddWithValue("@duracao", cursos.duracao);
						cmd.Parameters.AddWithValue("@id", cursos.id);
						cmd.ExecuteNonQuery();
					}
				}
				StatusMessage = "Curso atualizado com sucesso.";
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao atualizar curso: {ex.Message}";
			}
		}

		public static List<Cursos> Localizar(string nome)
		{
			List<Cursos> cursosEncontrados = new List<Cursos>();

			try
			{
				string sql = "SELECT * FROM cursos WHERE titulo LIKE @nome";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");
						using (MySqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								Cursos curso = new Cursos()
								{
									id = reader.GetInt32("id"),
									titulo = reader.GetString("titulo"),
									subtitulo = reader.GetString("subtitulo"),
									foto = reader.IsDBNull(reader.GetOrdinal("foto")) ? null : (byte[])reader["foto"],
									desc_principal = reader.GetString("desc_principal"),
									desc_secundaria = reader.GetString("desc_secundaria"),
									atualizacao = reader.GetDateTime("atualizacao"),
									estrelas = reader.GetInt32("estrelas"),
									criador = reader.GetString("criador"),
									duracao = reader.GetString("duracao")
								};
								cursosEncontrados.Add(curso);
							}
						}
					}
				}
				StatusMessage = "Cursos encontrados com sucesso.";
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao localizar cursos: {ex.Message}";
			}

			return cursosEncontrados;
		}		

		public static Cursos ObterCursoPorId(int idCurso)
		{
			Cursos curso = null;

			try
			{
				string sql = "SELECT * FROM cursos WHERE id = @idCurso";

				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@idCurso", idCurso);
						using (MySqlDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								curso = new Cursos()
								{
									id = reader.GetInt32("id"),
									titulo = reader.GetString("titulo"),
									subtitulo = reader.GetString("subtitulo"),
									foto = reader.IsDBNull(reader.GetOrdinal("foto")) ? null : (byte[])reader["foto"],
									desc_principal = reader.GetString("desc_principal"),
									desc_secundaria = reader.GetString("desc_secundaria"),
									atualizacao = reader.GetDateTime("atualizacao"),
									estrelas = reader.GetInt32("estrelas"),
									criador = reader.GetString("criador"),
									duracao = reader.GetString("duracao")
								};
							}
						}
					}
				}
				StatusMessage = "Curso encontrado com sucesso.";
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao obter curso por ID: {ex.Message}";
			}

			return curso;
		}

		public static List<Cursos> ListarCursosPorFornecedor(int fornecedorId)
		{
			List<Cursos> listacursos = new List<Cursos>();
			try
			{
				string sql = "SELECT * FROM cursos WHERE fornecedor_id = @fornecedorId";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@fornecedorId", fornecedorId);
						using (MySqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								Cursos cursos = new Cursos()
								{
									id = reader.GetInt32("id"),
									titulo = reader.GetString("titulo"),
									subtitulo = reader.GetString("subtitulo"),
									foto = reader.IsDBNull(reader.GetOrdinal("foto")) ? null : (byte[])reader["foto"],
									desc_principal = reader.GetString("desc_principal"),
									desc_secundaria = reader.GetString("desc_secundaria"),
									atualizacao = reader.GetDateTime("atualizacao"),
									estrelas = reader.GetInt32("estrelas"),
									criador = reader.GetString("criador"),
									duracao = reader.GetString("duracao")
								};
								listacursos.Add(cursos);
							}
						}
					}
				}
				if (listacursos.Count > 0)
				{
					StatusMessage = "Cursos encontrados.";
				}
				else
				{
					StatusMessage = "Nenhum Curso encontrado.";
				}
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao listar Cursos: {ex.Message}";
			}
			return listacursos;
		}

	}
}

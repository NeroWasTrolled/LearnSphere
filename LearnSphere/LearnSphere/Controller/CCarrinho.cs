using LearnSphere.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Controller
{
	public class CCarrinho
	{
		static string conn = @"server=sql10.freesqldatabase.com;port=3306;database=sql10714026;user=sql10714026;password=pf5lL1idAD";

		public static void InserirNoCarrinho(Carrinho carrinho)
		{
			try
			{
				string sql = "INSERT INTO carrinho (idcurso, iduser) VALUES (@idCurso, @idUsuario)";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@idCurso", carrinho.IdCurso);
						cmd.Parameters.AddWithValue("@idUsuario", carrinho.IdUsuario);
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao inserir no carrinho: " + ex.Message);
			}
		}

		public static bool VerificarCompra(int idUsuario, int idCurso)
		{
			try
			{
				string sql = "SELECT COUNT(*) FROM compra WHERE iduser = @idUsuario AND idcurso = @idCurso";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
						cmd.Parameters.AddWithValue("@idCurso", idCurso);
						int count = Convert.ToInt32(cmd.ExecuteScalar());
						return count > 0;
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao verificar compra: " + ex.Message);
			}
		}

		public static List<Cursos> ListarCursosNoCarrinho(int usuarioId)
		{
			List<Cursos> cursosNoCarrinho = new List<Cursos>();

			try
			{
				string sql = @"SELECT c.* FROM cursos c " +
							 "INNER JOIN carrinho ca ON c.id = ca.idcurso " +
							 "WHERE ca.iduser = @idUsuario";

				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@idUsuario", usuarioId);
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
								cursosNoCarrinho.Add(curso);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Erro ao listar cursos no carrinho: {ex.Message}");
			}

			return cursosNoCarrinho;
		}

		public static bool VerificarNoCarrinho(int idUsuario, int idCurso)
		{
			try
			{
				string sql = "SELECT COUNT(*) FROM carrinho WHERE iduser = @idUsuario AND idcurso = @idCurso";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
						cmd.Parameters.AddWithValue("@idCurso", idCurso);
						int count = Convert.ToInt32(cmd.ExecuteScalar());
						return count > 0;
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao verificar no carrinho: " + ex.Message);
			}
		}

		public static void RemoverDoCarrinho(int idUsuario, int idCurso)
		{
			try
			{
				string sql = "DELETE FROM carrinho WHERE iduser = @idUsuario AND idcurso = @idCurso";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
						cmd.Parameters.AddWithValue("@idCurso", idCurso);
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao remover do carrinho: " + ex.Message);
			}
		}

		public static void TransferirDoCarrinhoParaCompras(int usuarioId)
		{
			try
			{
				List<int> idsCurso = new List<int>();

				string selectSql = "SELECT idcurso FROM carrinho WHERE iduser = @idUsuario";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(selectSql, con))
					{
						cmd.Parameters.AddWithValue("@idUsuario", usuarioId);
						using (MySqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								idsCurso.Add(reader.GetInt32("idcurso"));
							}
						}
					}

					string insertSql = "INSERT INTO compra (idcurso, iduser) VALUES (@idCurso, @idUsuario)";
					foreach (int idCurso in idsCurso)
					{
						using (MySqlCommand cmd = new MySqlCommand(insertSql, con))
						{
							cmd.Parameters.AddWithValue("@idCurso", idCurso);
							cmd.Parameters.AddWithValue("@idUsuario", usuarioId);
							cmd.ExecuteNonQuery();
						}
					}

					string deleteSql = "DELETE FROM carrinho WHERE iduser = @idUsuario";
					using (MySqlCommand cmd = new MySqlCommand(deleteSql, con))
					{
						cmd.Parameters.AddWithValue("@idUsuario", usuarioId);
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Erro ao transferir do carrinho para compras: {ex.Message}");
			}
		}
	}
}

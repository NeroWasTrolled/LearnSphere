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
		static string conn = @"server=sql.freedb.tech;port=3306;database=freedb_cursos;user=freedb_adminv2;password=K3X59F@xY&@pYB?";
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

		public static void Excluir(int cursoId)
		{
			try
			{
				string sql = "DELETE FROM cursos WHERE id = @id";

				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@id", cursoId);
						cmd.ExecuteNonQuery();
					}
				}
				StatusMessage = "Curso excluído com sucesso.";
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao excluir curso: {ex.Message}";
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

		//Neymar

		public static List<Usuarios> ListarUser()
		{
			List<Usuarios> listarusers = new List<Usuarios>();
			try
			{
				string sql = "SELECT * FROM users";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						using (MySqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								Usuarios user = new Usuarios()
								{
									id = reader.GetInt32("id"),
									usuario = reader.GetString("usuario"),
									email = reader.GetString("email"),
									celular = reader.GetString("celular"),
									senha = reader.GetString("senha"),
								};
								listarusers.Add(user);
							}
						}
					}
				}
				if (listarusers.Count > 0)
				{
					StatusMessage = "Úsuario encontrado.";
				}
				else
				{
					StatusMessage = "Nenhum Úsuario encontrado.";
				}
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao listar Úsuario: {ex.Message}";
			}
			return listarusers;
		}

        public static void InserirUser(Usuarios users)
        {
            try
            {
                if (!IsValidEmail(users.email))
                {
                    StatusMessage = $"Erro: O email '{users.email}' não está no formato correto.";
                    return;
                }

                string telefoneFormatado = FormatPhoneNumber(users.celular);

                string sql = "INSERT INTO users(usuario, email, celular, senha, fornecedor) " +
                             "VALUES (@usuario, @email, @celular, @senha, @fornecedor)";

                // Conexão com o banco de dados
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        // Adiciona os parâmetros
                        cmd.Parameters.AddWithValue("@usuario", users.usuario);
                        cmd.Parameters.AddWithValue("@email", users.email);
                        cmd.Parameters.AddWithValue("@celular", telefoneFormatado);
                        cmd.Parameters.AddWithValue("@senha", users.senha);
                        cmd.Parameters.AddWithValue("@fornecedor", users.fornecedor);

                        // Executa o comando SQL
                        cmd.ExecuteNonQuery();
                    }
                }

                StatusMessage = $"Cadastro concluído com sucesso para o usuário {users.usuario}!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao efetuar cadastro: {ex.Message}";
            }
        }

        public static void AlterarUser(Usuarios users)
        {
            try
            {
                string sql = "UPDATE users SET usuario = @usuario, email = @email, " +
                             "celular = @celular, senha = @senha, fornecedor = @fornecedor WHERE id = @id";

                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@usuario", users.usuario);
                        cmd.Parameters.AddWithValue("@email", users.email);
                        cmd.Parameters.AddWithValue("@celular", users.celular);
                        cmd.Parameters.AddWithValue("@senha", users.senha);
                        cmd.Parameters.AddWithValue("@fornecedor", users.fornecedor);
                        cmd.Parameters.AddWithValue("@id", users.id);
                        cmd.ExecuteNonQuery();
                    }
                }

                StatusMessage = "Usuário atualizado com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao atualizar usuário: {ex.Message}";
            }
        }

        public static void ExcluirUser(int id)
		{
			try
			{
				string sql = "DELETE FROM users WHERE id=@id";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.ExecuteNonQuery();
					}
					StatusMessage = $"Conta Excluída com Sucesso!";
				}
			}
			catch (Exception)
			{
				StatusMessage = $"Erro ao efetuar exclusão de conta.";
			}
		}

		public static Usuarios LocalizarUser(string email, string user, string senha)
		{
			Usuarios usuario = null;
			try
			{
				string sql = "SELECT * FROM users WHERE (email = @email OR usuario = @user) AND senha = @senha";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@email", email);
						cmd.Parameters.AddWithValue("@user", user);
						cmd.Parameters.AddWithValue("@senha", senha);
						using (MySqlDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								usuario = new Usuarios()
								{
									id = reader.GetInt32("id"),
									usuario = reader.GetString("usuario"),
									email = reader.GetString("email"),
									celular = reader.GetString("celular"),
									senha = reader.GetString("senha"),
                            };
							}
						}
					}
				}

				if (usuario != null)
				{
					StatusMessage = "Usuário encontrado. Login bem-sucedido.";
				}
				else
				{
					StatusMessage = "Usuário não encontrado ou senha incorreta.";
				}
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao efetuar login: {ex.Message}";
			}

			return usuario;
		}

        public static void AtualizarUser(Usuarios usuario)
        {
            try
            {
                string sql = "UPDATE users SET usuario = @usuario, email = @email, " +
                             "celular = @celular, senha = @senha, fornecedor = @fornecedor WHERE id = @id";

                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario.usuario);
                        cmd.Parameters.AddWithValue("@email", usuario.email);
                        cmd.Parameters.AddWithValue("@celular", usuario.celular);
                        cmd.Parameters.AddWithValue("@senha", usuario.senha);
                        cmd.Parameters.AddWithValue("@fornecedor", usuario.fornecedor); // Certifique-se de que 'fornecedor' é um bool
                        cmd.Parameters.AddWithValue("@id", usuario.id);
                        cmd.ExecuteNonQuery();
                    }
                }

                StatusMessage = "Usuário atualizado com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao atualizar usuário: {ex.Message}";
            }
        }


        public static bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		public static string FormatPhoneNumber(string phoneNumber)
		{
			string digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());

			if (digitsOnly.Length == 11)
			{
				return string.Format("({0}) {1}-{2}",
					digitsOnly.Substring(0, 2),
					digitsOnly.Substring(2, 5),
					digitsOnly.Substring(7, 4));
			}
			else
			{
				return phoneNumber;
			}
		}

        public static List<Compras> ListarComprasPorUsuario(int idUsuario)
        {
            List<Compras> compras = new List<Compras>();

            try
            {
                string sql = "SELECT * FROM compras WHERE idusuario = @idUsuario";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Compras compra = new Compras()
                                {
                                    Id = reader.GetInt32("id"),
                                    IdCurso = reader.GetInt32("idcurso"),
                                    IdUsuario = reader.GetInt32("idusuario")
                                };
                                compras.Add(compra);
                            }
                        }
                    }
                }
                StatusMessage = "Compras encontradas com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar compras: {ex.Message}";
            }

            return compras;
        }

        public static List<Cursos> ListarCursosPorUsuario(int idUsuario)
        {
            List<Cursos> cursos = new List<Cursos>();

            try
            {
                string sql = "SELECT c.* FROM cursos c " +
                             "INNER JOIN compras co ON c.id = co.idcurso " +
                             "WHERE co.idusuario = @idUsuario";

                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
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
                                cursos.Add(curso);
                            }
                        }
                    }
                }
                StatusMessage = "Cursos encontrados com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar cursos: {ex.Message}";
            }

            return cursos;
        }
        public static void AtualizarFornecedor(int idUsuario, bool? fornecedor)
        {
            try
            {
                string sql = "UPDATE users SET fornecedor = @fornecedor WHERE id = @idUsuario";

                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@fornecedor", fornecedor ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmd.ExecuteNonQuery();
                    }
                }

                StatusMessage = "Fornecedor atualizado com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao atualizar fornecedor: {ex.Message}";
            }
        }


        public static Usuarios ObterUsuarioPorId(int idUsuario)
        {
            Usuarios usuario = null;

            try
            {
                string sql = "SELECT * FROM users WHERE id = @idUsuario";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuarios()
                                {
                                    id = reader.GetInt32("id"),
                                    usuario = reader.GetString("usuario"),
                                    email = reader.GetString("email"),
                                    celular = reader.GetString("celular"),
                                    senha = reader.GetString("senha")
                                };
                            }
                        }
                    }
                }
                StatusMessage = "Usuário encontrado com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao obter usuário: {ex.Message}";
            }

            return usuario;
        }

    }
}

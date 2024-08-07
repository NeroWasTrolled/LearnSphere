﻿using LearnSphere.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LearnSphere.Controller
{
	public class Users
	{
		static string conn = @"server=sql.freedb.tech;port=3306;database=freedb_tcccursos;user=freedb_GabrielF;password=WJxZ6@6frNm2WQb";
		public static string StatusMessage { get; set; }

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
									cpf = reader.GetString("cpf"),
									fornecedor = reader.GetBoolean("fornecedor")
								};
								listarusers.Add(user);
							}
						}
					}
				}
				if (listarusers.Count > 0)
				{
					StatusMessage = "Usuário encontrado.";
				}
				else
				{
					StatusMessage = "Nenhum usuário encontrado.";
				}
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao listar usuários: {ex.Message}";
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

				if (!IsStrongPassword(users.senha))
				{
					StatusMessage = "Erro: A senha deve conter pelo menos uma letra e um número.";
					return;
				}

				if (!IsValidCPF(users.cpf))
				{
					StatusMessage = $"Erro: O CPF '{users.cpf}' não está no formato correto.";
					return;
				}

				string cpfFormatado = users.cpf.Replace(".", "").Replace("-", "");
				string celularFormatado = users.celular.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

				string sql = "INSERT INTO users(usuario, email, celular, senha, cpf, fornecedor) " +
							 "VALUES (@usuario, @email, @celular, @senha, @cpf, @fornecedor)";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@usuario", users.usuario);
						cmd.Parameters.AddWithValue("@email", users.email);
						cmd.Parameters.AddWithValue("@celular", celularFormatado);
						cmd.Parameters.AddWithValue("@senha", users.senha);
						cmd.Parameters.AddWithValue("@cpf", cpfFormatado);
						cmd.Parameters.AddWithValue("@fornecedor", users.fornecedor);
						cmd.ExecuteNonQuery();
					}
				}
				StatusMessage = "Usuário cadastrado com sucesso!";
			}
			catch (MySqlException ex)
			{
				StatusMessage = $"Erro no cadastro de Usuário: {ex.Message}";
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro no cadastro de Usuário: {ex.Message}";
			}
		}

        public static Usuarios LocalizarUser(string email, string user, string cpf, string senha)
        {
            Usuarios usuario = null;
            string sql = "SELECT * FROM users WHERE (email = @Email OR usuario = @User OR cpf = @CPF) AND senha = @Senha";
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@User", user);
                    cmd.Parameters.AddWithValue("@CPF", cpf);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuarios()
                            {
                                id = reader.GetInt32("id"),
                                usuario = reader.GetString("usuario"),
                                email = reader.GetString("email"),
                                senha = reader.GetString("senha"),
                                cpf = reader.GetString("cpf"),
                                celular = reader.GetString("celular"),
                                fornecedor = reader.GetBoolean("fornecedor"),
                                admin = reader.GetBoolean("admin") 
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public static async Task<Usuarios> RealizarLogin(string email, string senha)
        {
            Usuarios usuario = null;
            string sql = "SELECT * FROM users WHERE email = @Email AND senha = @Senha";
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuarios()
                            {
                                id = reader.GetInt32("id"),
                                usuario = reader.GetString("usuario"),
                                email = reader.GetString("email"),
                                senha = reader.GetString("senha"),
                                cpf = reader.GetString("cpf"),
                                celular = reader.GetString("celular"),
                                fornecedor = reader.GetBoolean("fornecedor"),
                                admin = reader.GetBoolean("admin") 
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public static void AtualizarUser(Usuarios users)
		{
			try
			{
				if (!IsValidCPF(users.cpf))
				{
					StatusMessage = $"Erro: O CPF '{users.cpf}' não está no formato correto.";
					return;
				}

				string sql = "UPDATE users SET usuario = @usuario, email = @email, " +
							 "celular = @celular, senha = @senha, cpf = @cpf, fornecedor = @fornecedor WHERE id = @id";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@usuario", users.usuario);
						cmd.Parameters.AddWithValue("@email", users.email);
						cmd.Parameters.AddWithValue("@celular", users.celular);
						cmd.Parameters.AddWithValue("@senha", users.senha);
						cmd.Parameters.AddWithValue("@cpf", users.cpf);
						cmd.Parameters.AddWithValue("@fornecedor", users.fornecedor);
						cmd.Parameters.AddWithValue("@id", users.id);
						cmd.ExecuteNonQuery();
					}
				}
				StatusMessage = "Usuário atualizado com sucesso!";
			}
			catch (Exception ex)
			{
				StatusMessage = $"Erro ao atualizar usuário: {ex.Message}";
			}
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
									cpf = reader.GetString("cpf"),
									celular = reader.GetString("celular"),
									senha = reader.GetString("senha"),
									fornecedor = reader.GetBoolean("fornecedor")
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

		public static bool VerificarCPFCadastrado(string cpf)
		{
			try
			{
				string sql = "SELECT COUNT(*) FROM users WHERE cpf = @cpf";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@cpf", cpf);
						int count = Convert.ToInt32(cmd.ExecuteScalar());
						return count > 0;
					}
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool VerificarEmailCadastrado(string email)
		{
			try
			{
				string sql = "SELECT COUNT(*) FROM users WHERE email = @email";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@email", email);
						int count = Convert.ToInt32(cmd.ExecuteScalar());
						return count > 0;
					}
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool VerificarUsuarioCadastrado(string usuario)
		{
			try
			{
				string sql = "SELECT COUNT(*) FROM users WHERE usuario = @usuario";
				using (MySqlConnection con = new MySqlConnection(conn))
				{
					con.Open();
					using (MySqlCommand cmd = new MySqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@usuario", usuario);
						int count = Convert.ToInt32(cmd.ExecuteScalar());
						return count > 0;
					}
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		private static string FormatPhoneNumber(string phoneNumber)
		{
			if (phoneNumber.Length != 11)
			{
				throw new ArgumentException("O número de celular deve conter exatamente 11 dígitos (incluindo o código de área).");
			}

			return string.Format("{0:(##) # ####-####}", long.Parse(phoneNumber));
		}

		private static bool IsStrongPassword(string password)
		{
			return Regex.IsMatch(password, @"\d") && Regex.IsMatch(password, "[a-zA-Z]");
		}

		private static bool IsValidEmail(string email)
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

		private static bool IsValidCPF(string cpf)
		{
			cpf = cpf.Replace(".", "").Replace("-", "");

			if (cpf.Length != 11)
				return false;

			if (cpf.Distinct().Count() == 1)
				return false;

			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			string tempCpf = cpf.Substring(0, 9);
			int soma = 0;

			for (int i = 0; i < 9; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

			int resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			string digito = resto.ToString();
			tempCpf = tempCpf + digito;
			soma = 0;

			for (int i = 0; i < 10; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = digito + resto.ToString();

			return cpf.EndsWith(digito);
		}
	}
}



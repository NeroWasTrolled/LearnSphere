using LearnSphere.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace LearnSphere.Controller
{
    public class CCompras
    {
        static string conn = @"server=sql.freedb.tech;port=3306;database=freedb_tcccursos;user=freedb_GabrielF;password=WJxZ6@6frNm2WQb";
        public static string StatusMessage { get; set; }

        public static void InserirCompra(Compras compra)
        {
            try
            {
                if (UsuarioComprouCurso(compra.IdUsuario, compra.IdCurso))
                {
                    StatusMessage = "Você já comprou este curso.";
                    throw new InvalidOperationException("Você já comprou este curso.");
                }

                string sql = "INSERT INTO compra (idcurso, iduser) VALUES (@idCurso, @idUsuario)";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@idCurso", compra.IdCurso);
                        cmd.Parameters.AddWithValue("@idUsuario", compra.IdUsuario);
                        cmd.ExecuteNonQuery();
                    }
                }
                StatusMessage = "Compra registrada com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao registrar compra: {ex.Message}";
                throw;
            }
        }

        private static bool UsuarioComprouCurso(int idUsuario, int idCurso)
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
                throw new Exception($"Erro ao verificar se o usuário já comprou o curso: {ex.Message}");
            }
        }

        public static List<Cursos> ListarCursosAdquiridos(int usuarioId)
        {
            List<Cursos> cursosAdquiridos = new List<Cursos>();

            try
            {
                string sql = @"SELECT c.* FROM cursos c 
                               INNER JOIN compra co ON c.id = co.idcurso 
                               WHERE co.iduser = @idUsuario";

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
                                    duracao = reader.GetString("duracao"),
                                    preco = reader.GetDecimal("preco"), // Adicionei o campo preco
                                    conteudo = reader.GetString("conteudo") // Adicionei o campo conteudo
                                };
                                cursosAdquiridos.Add(curso);
                            }
                        }
                    }
                }
                StatusMessage = "Cursos adquiridos encontrados com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar cursos adquiridos: {ex.Message}";
            }

            return cursosAdquiridos;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using MySqlConnector;
using LearnSphere.Models;

namespace LearnSphere.Controller
{
    public class MySQLCon
    {
        static string conn = @"server=sql.freedb.tech;port=3306;database=freedb_tcccursos;user=freedb_GabrielF;password=WJxZ6@6frNm2WQb";
        public static string StatusMessage { get; set; }

        public static List<Cursos> ListarCursos()
        {
            List<Cursos> listacursos = new List<Cursos>();
            try
            {
                string sql = "SELECT * FROM cursos WHERE publicado = true";
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
                                    duracao = reader.GetString("duracao"),
                                    preco = reader.GetDecimal("preco"),
                                    conteudo = reader.GetString("conteudo"),
                                    publicado = reader.GetBoolean("publicado"),
                                    fornecedorid = reader.GetInt32("fornecedorid")
                                };
                                listacursos.Add(cursos);
                            }
                        }
                    }
                }
                StatusMessage = listacursos.Count > 0 ? "Cursos encontrados." : "Nenhum curso encontrado.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar cursos: {ex.Message}";
            }
            return listacursos;
        }

        public static List<Cursos> ListarCursosNaoPublicados()
        {
            List<Cursos> listacursos = new List<Cursos>();
            try
            {
                string sql = "SELECT * FROM cursos WHERE publicado = false";
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
                                    duracao = reader.GetString("duracao"),
                                    preco = reader.GetDecimal("preco"),
                                    conteudo = reader.GetString("conteudo"),
                                    publicado = reader.GetBoolean("publicado"),
                                    fornecedorid = reader.GetInt32("fornecedorid")
                                };
                                listacursos.Add(cursos);
                            }
                        }
                    }
                }
                StatusMessage = listacursos.Count > 0 ? "Cursos não publicados encontrados." : "Nenhum curso não publicado encontrado.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar cursos não publicados: {ex.Message}";
            }
            return listacursos;
        }

        public static void AtualizarPublicacaoCurso(int idCurso, bool publicar)
        {
            try
            {
                string sql = "UPDATE cursos SET publicado = @publicado WHERE id = @idCurso";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@publicado", publicar);
                        cmd.Parameters.AddWithValue("@idCurso", idCurso);
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
                throw new Exception("Erro ao salvar a foto do curso.", ex);
            }
        }

        public static void Inserir(Cursos cursos)
        {
            try
            {
                string sql = "INSERT INTO cursos(titulo, subtitulo, foto, desc_principal, desc_secundaria, atualizacao, estrelas, criador, duracao, preco, conteudo, publicado, fornecedorid) " +
                             "VALUES (@titulo, @subtitulo, @foto, @descPri, @descSec, @atualizacao, @estrelas, @criador, @duracao, @preco, @conteudo, @publicado, @fornecedorid)";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@titulo", cursos.titulo);
                        cmd.Parameters.AddWithValue("@subtitulo", cursos.subtitulo);
                        cmd.Parameters.AddWithValue("@foto", cursos.foto ?? new byte[0]);
                        cmd.Parameters.AddWithValue("@descPri", cursos.desc_principal);
                        cmd.Parameters.AddWithValue("@descSec", cursos.desc_secundaria);
                        cmd.Parameters.AddWithValue("@atualizacao", cursos.atualizacao);
                        cmd.Parameters.AddWithValue("@estrelas", cursos.estrelas);
                        cmd.Parameters.AddWithValue("@criador", cursos.criador);
                        cmd.Parameters.AddWithValue("@duracao", cursos.duracao);
                        cmd.Parameters.AddWithValue("@preco", cursos.preco);
                        cmd.Parameters.AddWithValue("@conteudo", cursos.conteudo);
                        cmd.Parameters.AddWithValue("@publicado", cursos.publicado);
                        cmd.Parameters.AddWithValue("@fornecedorid", cursos.fornecedorid);
                        cmd.ExecuteNonQuery();
                    }
                }

                SalvarFoto(cursos, cursos.foto);
                StatusMessage = "Curso enviado para análise!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro no cadastro de curso: {ex.Message}";
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
                             "criador = @criador, duracao = @duracao, preco = @preco, " +
                             "conteudo = @conteudo, publicado = @publicado, fornecedorid = @fornecedorid WHERE id = @id";

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
                        cmd.Parameters.AddWithValue("@preco", cursos.preco);
                        cmd.Parameters.AddWithValue("@conteudo", cursos.conteudo);
                        cmd.Parameters.AddWithValue("@publicado", cursos.publicado);
                        cmd.Parameters.AddWithValue("@fornecedorid", cursos.fornecedorid);
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

        public static void RemoverCurso(int idCurso)
        {
            try
            {
                string sql = "DELETE FROM cursos WHERE id = @idCurso";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@idCurso", idCurso);
                        cmd.ExecuteNonQuery();
                    }
                }
                StatusMessage = "Curso removido com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao remover curso: {ex.Message}";
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
                                    duracao = reader.GetString("duracao"),
                                    preco = reader.GetDecimal("preco"),
                                    conteudo = reader.GetString("conteudo"),
                                    publicado = reader.GetBoolean("publicado"),
                                    fornecedorid = reader.GetInt32("fornecedorid")
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
                                    duracao = reader.GetString("duracao"),
                                    preco = reader.GetDecimal("preco"),
                                    conteudo = reader.GetString("conteudo"),
                                    publicado = reader.GetBoolean("publicado"),
                                    fornecedorid = reader.GetInt32("fornecedorid")
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
                string sql = "SELECT * FROM cursos WHERE fornecedorid = @fornecedorId";
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
                                    duracao = reader.GetString("duracao"),
                                    preco = reader.GetDecimal("preco"),
                                    conteudo = reader.GetString("conteudo"),
                                    publicado = reader.GetBoolean("publicado"),
                                    fornecedorid = reader.GetInt32("fornecedorid")
                                };
                                listacursos.Add(cursos);
                            }
                        }
                    }
                }
                StatusMessage = listacursos.Count > 0 ? "Cursos encontrados." : "Nenhum curso encontrado.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar cursos: {ex.Message}";
            }
            return listacursos;
        }

        public static List<Cursos> ListarCursosDisponiveis()
        {
            List<Cursos> listacursos = new List<Cursos>();
            try
            {
                string sql = "SELECT * FROM cursos WHERE publicado = true";
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
                                    duracao = reader.GetString("duracao"),
                                    preco = reader.GetDecimal("preco"),
                                    conteudo = reader.GetString("conteudo"),
                                    publicado = reader.GetBoolean("publicado"),
                                    fornecedorid = reader.GetInt32("fornecedorid")
                                };
                                listacursos.Add(cursos);
                            }
                        }
                    }
                }
                StatusMessage = listacursos.Count > 0 ? "Cursos disponíveis encontrados." : "Nenhum curso disponível encontrado.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar cursos disponíveis: {ex.Message}";
            }
            return listacursos;
        }

        public static List<Cursos> ListarCursosNoCarrinho(int usuarioId)
        {
            List<Cursos> listacursos = new List<Cursos>();
            try
            {
                string sql = "SELECT c.*, cr.* FROM cursos c INNER JOIN carrinho cr ON c.id = cr.IdCurso WHERE cr.IdUsuario = @usuarioId";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cursos curso = new Cursos
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
                                    preco = reader.GetDecimal("preco"), 
                                    conteudo = reader.GetString("conteudo"),
                                    publicado = reader.GetBoolean("publicado"),
                                    fornecedorid = reader.GetInt32("fornecedorid")
                                };
                                listacursos.Add(curso);
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
                    StatusMessage = "Nenhum curso encontrado.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar cursos no carrinho: {ex.Message}";
            }
            return listacursos;
        }
    }
}

using LearnSphere.Controller;
using LearnSphere.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LearnSphere
{
    public static class LoginManager
    {
        private static Usuarios loggedInUser;

        public static bool IsUserLoggedIn => loggedInUser != null;

        public static void Login(Usuarios usuario)
        {
            loggedInUser = usuario;
            App.UsuarioLogado = usuario;
            Console.WriteLine($"Login: UsuarioLogado = {App.UsuarioLogado.usuario}, Fornecedor = {App.UsuarioLogado.fornecedor}, Admin = {App.UsuarioLogado.admin}");
        }

        public static void Logout()
        {
            loggedInUser = null;
            App.UsuarioLogado = null;
        }

        public static Usuarios GetLoggedInUser()
        {
            return loggedInUser;
        }

        public static int GetLoggedInUserId()
        {
            return loggedInUser != null ? loggedInUser.id : -1;
        }

        public static bool IsUserFornecedor()
        {
            var user = GetLoggedInUser();
            return user != null && user.fornecedor;
        }

        public static bool IsUserAdmin()
        {
            var user = GetLoggedInUser();
            return user != null && user.admin;
        }

        public static async Task<bool> LocalizarUser(Page page, string email, string cpf, string user, string senha)
        {
            try
            {
                if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(user) && string.IsNullOrEmpty(senha))
                {
                    await page.DisplayAlert("Erro", "Por favor, forneça o e-mail, nome de usuário ou número de celular.", "OK");
                    return false;
                }

                Usuarios usuario = Users.LocalizarUser(email, user, cpf, senha);

                if (usuario != null)
                {
                    Login(usuario);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                await page.DisplayAlert("Erro", $"Erro ao efetuar login: {ex.Message}", "OK");
                return false;
            }
        }

        public static async Task<bool> LocalizarUser(Page page, string email, string senha)
        {
            try
            {
                if (!IsValidEmail(email))
                {
                    await page.DisplayAlert("Erro", "Formato de email incorreto.", "OK");
                    return false;
                }

                Usuarios usuario = await Users.RealizarLogin(email, senha);
                if (usuario != null)
                {
                    Login(usuario);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await page.DisplayAlert("Erro", $"Erro ao efetuar login: {ex.Message}", "OK");
                return false;
            }
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

        public static List<Cursos> ObterCursosAdquiridos()
        {
            if (loggedInUser == null)
            {
                throw new InvalidOperationException("Nenhum usuário está logado.");
            }

            return CCompras.ListarCursosAdquiridos(loggedInUser.id);
        }

        public static void RegistrarCompra(int cursoId)
        {
            if (loggedInUser == null)
            {
                throw new InvalidOperationException("Nenhum usuário está logado.");
            }

            var compra = new Compras
            {
                IdCurso = cursoId,
                IdUsuario = loggedInUser.id
            };

            CCompras.InserirCompra(compra);
        }
    }
}

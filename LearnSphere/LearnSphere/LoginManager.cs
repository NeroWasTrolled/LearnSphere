using LearnSphere.Controller;
using LearnSphere.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LearnSphere
{
    public static class LoginManager
    {
        private static Usuarios loggedInUser;

        public static bool IsUserLoggedIn => loggedInUser != null;

        public static async Task InitializeAsync()
        {
            var userId = await SecureStorage.GetAsync("user_id");
            var userEmail = await SecureStorage.GetAsync("user_email");
            var userFornecedor = await SecureStorage.GetAsync("user_fornecedor");
            var userAdmin = await SecureStorage.GetAsync("user_admin");

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userEmail))
            {
                loggedInUser = new Usuarios
                {
                    id = int.Parse(userId),
                    email = userEmail,
                    fornecedor = bool.Parse(userFornecedor),
                    admin = bool.Parse(userAdmin)
                };

                App.UsuarioLogado = loggedInUser;
            }
        }

        public static async void Login(Usuarios usuario)
        {
            loggedInUser = usuario;
            App.UsuarioLogado = usuario;

            await SecureStorage.SetAsync("user_id", usuario.id.ToString());
            await SecureStorage.SetAsync("user_email", usuario.email);
            await SecureStorage.SetAsync("user_fornecedor", usuario.fornecedor.ToString());
            await SecureStorage.SetAsync("user_admin", usuario.admin.ToString());

            Console.WriteLine($"Login: UsuarioLogado = {App.UsuarioLogado.usuario}, Fornecedor = {App.UsuarioLogado.fornecedor}, Admin = {App.UsuarioLogado.admin}");
        }

        public static async void Logout()
        {
            loggedInUser = null;
            App.UsuarioLogado = null;

            SecureStorage.Remove("user_id");
            SecureStorage.Remove("user_email");
            SecureStorage.Remove("user_fornecedor");
            SecureStorage.Remove("user_admin");
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

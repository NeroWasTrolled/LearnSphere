using LearnSphere.Controller;
using LearnSphere.Models;
using System;
using System.Threading.Tasks;
using LearnSphere.Controller;
using LearnSphere.Models;
using Xamarin.Forms;
using LearnSphere.View;

namespace LearnSphere
{
	public static class LoginManager
	{
		private static Usuarios loggedInUser;
		public static string StatusMessage { get; set; }

		public static bool IsUserLoggedIn => loggedInUser != null;

		public static async Task<bool> LocalizarUser(Page page, string email, string user, string senha)
		{
			try
			{
				if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(user) && string.IsNullOrEmpty(senha))
				{
					await page.DisplayAlert("Erro", "Por favor, forneça o e-mail, nome de usuário ou número de celular.", "OK");
					return false;
				}

				Usuarios usuario = MySQLCon.LocalizarUser(email, user, senha);

				if (usuario != null)
				{
					Login(usuario);
					StatusMessage = "Usuário encontrado. Login bem-sucedido.";
					return true;
				}
				else
				{
					await page.DisplayAlert("Erro", "Usuário não encontrado ou senha incorreta.", "OK");
					return false;
				}
			}
			catch (Exception ex)
			{
				await page.DisplayAlert("Erro", $"Erro ao efetuar login: {ex.Message}", "OK");
				return false;
			}
		}


		public static void Login(Usuarios usuario)
		{
			loggedInUser = usuario;
		}

        // Em LoginManager.cs
        public static void Logout()
        {
            loggedInUser = null;
        }


        public static Usuarios GetLoggedInUser()
		{
			return loggedInUser;
		}
	}
}

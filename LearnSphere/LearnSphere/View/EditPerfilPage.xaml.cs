using System;
using Xamarin.Forms;
using LearnSphere.Controller;
using LearnSphere.Models;

namespace LearnSphere.View
{
    public partial class EditPerfilPage : ContentPage
    {
        private int idUsuario; // Adicione esta variável para armazenar o ID do usuário logado
        public EditPerfilPage()
        {
            InitializeComponent();
        }
        public EditPerfilPage(int idUsuario) // Adicione um parâmetro para receber o ID do usuário
        {
            InitializeComponent();
            this.idUsuario = idUsuario; // Armazene o ID do usuário recebido
            CarregarDadosUsuario(); // Carregue os dados do usuário ao abrir a página
        }

        private void CarregarDadosUsuario()
        {
            // Obter dados do usuário pelo ID
            Usuarios usuario = MySQLCon.ObterUsuarioPorId(idUsuario);

            // Preencher os campos com os dados do usuário
            entryUsuario.Text = usuario.usuario;
            entryEmail.Text = usuario.email;
            entryTelefone.Text = usuario.celular;

            // Configurar o switch com o tipo de usuário atual
            switchTipoUsuario.IsToggled = usuario.fornecedor;
        }

        private void Salvar_Clicked(object sender, EventArgs e)
        {
            // Obter o valor do switch e convertê-lo para um tipo bool
            bool fornecedor = switchTipoUsuario.IsToggled;

            // Atualizar os dados do usuário com os valores dos campos
            Usuarios usuario = new Usuarios()
            {
                id = idUsuario,
                usuario = entryUsuario.Text,
                email = entryEmail.Text,
                celular = entryTelefone.Text,
                fornecedor = fornecedor // Atribuir o valor bool diretamente ao campo fornecedor
            };

            MySQLCon.AtualizarUser(usuario); // Atualizar os dados do usuário no banco de dados

            // Mostrar uma mensagem de sucesso ou erro
            DisplayAlert("Sucesso", MySQLCon.StatusMessage, "OK");
        }


        private async void Desconectar_Clicked(object sender, EventArgs e)
        {
            // Implemente a lógica para desconectar o usuário (logout)
            // Por exemplo, você pode limpar os dados de login armazenados localmente
            // E redirecionar o usuário para a página de login
            await Navigation.PushAsync(new PageLogin()); // Redirecionar para a página de login
        }
    }
}

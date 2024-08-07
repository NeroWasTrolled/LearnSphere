﻿using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LearnSphere.Controller;
using LearnSphere.Models;
using Xamarin.Essentials;

namespace LearnSphere.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCursos : ContentPage
    {
        private Cursos curso;
        private bool descricaoCompleta = false;
        private const int MAX_DESC_LENGTH = 200;

        public PageCursos(Cursos curso)
        {
            InitializeComponent();
            this.curso = curso;

            cursoImage.Source = ImageSource.FromStream(() => new MemoryStream(curso.foto));
            cursoNome.Text = curso.titulo;
            cursoSubtitulo.Text = curso.subtitulo;
            cursoAutor.Text = curso.criador;
            cursoData.Text = curso.atualizacao.ToString("dd/MM/yyyy");
            cursoDescSecundaria.Text = curso.desc_secundaria;
            cursoDuracao.Text = curso.duracao;
            cursoPreco.Text = $"R$ {curso.preco:F2}";

            LimitarDescricao();
            VerificarCompra();

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += MostrarMaisLabel_Tapped;
            mostrarMaisLabel.GestureRecognizers.Add(tapGesture);
        }

        private async void VerificarCompra()
        {
            if (App.UsuarioLogado != null)
            {
                try
                {
                    bool jaComprado = CCarrinho.VerificarCompra(App.UsuarioLogado.id, curso.id);
                    if (jaComprado)
                    {
                        ButtonComprar.IsVisible = false;
                        ButtonAdicionarCarrinho.IsVisible = false;
                        ButtonRemoverCarrinho.IsVisible = false;
                        cursoPreco.IsVisible = false;

                        cursoConteudoLayout.IsVisible = true;
                        cursoConteudo.Text = curso.conteudo;
                    }
                    else
                    {
                        bool noCarrinho = CCarrinho.VerificarNoCarrinho(App.UsuarioLogado.id, curso.id);
                        ButtonComprar.IsVisible = true;
                        ButtonAdicionarCarrinho.IsVisible = !noCarrinho;
                        ButtonRemoverCarrinho.IsVisible = noCarrinho;
                        cursoConteudoLayout.IsVisible = false;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Erro ao verificar compra: {ex.Message}", "OK");
                }
            }
        }

        private void LimitarDescricao()
        {
            if (!string.IsNullOrEmpty(curso.desc_principal) && curso.desc_principal.Length > MAX_DESC_LENGTH)
            {
                cursoDescricao.Text = curso.desc_principal.Substring(0, MAX_DESC_LENGTH) + "...";
                mostrarMaisLabel.IsVisible = true;
            }
            else
            {
                cursoDescricao.Text = curso.desc_principal;
                mostrarMaisLabel.IsVisible = false;
            }
        }

        private void MostrarMaisLabel_Tapped(object sender, EventArgs e)
        {
            if (descricaoCompleta)
            {
                cursoDescricao.MaxLines = 3;
                cursoDescricao.Text = curso.desc_principal.Substring(0, MAX_DESC_LENGTH) + "...";
                mostrarMaisLabel.Text = "Mostrar mais";
            }
            else
            {
                cursoDescricao.MaxLines = int.MaxValue;
                cursoDescricao.Text = curso.desc_principal;
                mostrarMaisLabel.Text = "Mostrar menos";
            }

            descricaoCompleta = !descricaoCompleta;
        }

        private async void OnAdicionarCarrinhoClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.UsuarioLogado != null)
                {
                    Carrinho carrinho = new Carrinho
                    {
                        IdCurso = curso.id,
                        IdUsuario = App.UsuarioLogado.id
                    };

                    try
                    {
                        CCarrinho.InserirNoCarrinho(carrinho);
                        await DisplayAlert("Sucesso", "Curso adicionado ao carrinho!", "OK");
                        VerificarCompra();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", $"Erro ao adicionar ao carrinho: {ex.Message}", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Erro", "Usuário não está logado. Faça o login para adicionar ao carrinho.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao adicionar ao carrinho: {ex.Message}", "OK");
            }
        }

        private async void OnRemoverCarrinhoClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.UsuarioLogado != null)
                {
                    try
                    {
                        CCarrinho.RemoverDoCarrinho(App.UsuarioLogado.id, curso.id);
                        await DisplayAlert("Sucesso", "Curso removido do carrinho!", "OK");
                        VerificarCompra();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", $"Erro ao remover do carrinho: {ex.Message}", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Erro", "Usuário não está logado. Faça o login para remover do carrinho.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao remover do carrinho: {ex.Message}", "OK");
            }
        }

        private async void OnComprarClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.UsuarioLogado != null)
                {
                    Compras compra = new Compras
                    {
                        IdCurso = curso.id,
                        IdUsuario = App.UsuarioLogado.id
                    };

                    try
                    {
                        CCompras.InserirCompra(compra);
                        CCarrinho.RemoverDoCarrinho(App.UsuarioLogado.id, curso.id);
                        await DisplayAlert("Sucesso", "Compra realizada com sucesso!", "OK");
                        await Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", $"Erro ao realizar a compra: {ex.Message}", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Erro", "Usuário não está logado. Faça o login para comprar.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao realizar a compra: {ex.Message}", "OK");
            }
        }

        private async void OnConteudoTapped(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(curso.conteudo))
            {
                await Launcher.OpenAsync(new Uri(curso.conteudo));
            }
        }
    }
}

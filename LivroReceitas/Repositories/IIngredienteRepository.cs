using System.Collections.Generic;
using LivroReceitas.Models;

namespace LivroReceitas.Repositories
{
    public interface IIngredienteRepository
    {
        void Salvar(Ingrediente ingrediente);
        void AtualizarQuantidade(string nome, int quantidade);
        Ingrediente? BuscarPorNome(string nome);
        List<Ingrediente> Listar();
    }
}

using System.Collections.Generic;
using LivroReceitas.Models;

namespace LivroReceitas.Repositories
{
    public interface IReceitaRepository
    {
        void Adicionar(Receita receita);
        List<Receita> Listar();
        Receita? BuscarPorNome(string nome);
    }
}

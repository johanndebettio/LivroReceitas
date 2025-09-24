using System;
using System.Collections.Generic;
using System.Linq;
using LivroReceitas.Models;

namespace LivroReceitas.Repositories
{
    public class ReceitaRepository : IReceitaRepository
    {
        private readonly List<Receita> _receitas = [];

        public void Adicionar(Receita receita) => _receitas.Add(receita);

        public List<Receita> Listar() => _receitas;

        public Receita? BuscarPorNome(string nome) =>
            _receitas.FirstOrDefault(r => r.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
    }
}

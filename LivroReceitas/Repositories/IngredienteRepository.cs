using System;
using System.Collections.Generic;
using System.Linq;
using LivroReceitas.Models;

namespace LivroReceitas.Repositories
{
    public class IngredienteRepository : IIngredienteRepository
    {
        private readonly List<Ingrediente> _ingredientes = [];

        public void Salvar(Ingrediente ingrediente)
        {
            var existente = _ingredientes.FirstOrDefault(i => i.Nome.Equals(ingrediente.Nome, StringComparison.OrdinalIgnoreCase));
            if (existente == null)
                _ingredientes.Add(new Ingrediente(ingrediente.Nome, ingrediente.Quantidade));
            else
                existente.Quantidade = ingrediente.Quantidade;
        }

        public void AtualizarQuantidade(string nome, int quantidade)
        {
            var existente = BuscarPorNome(nome);
            if (existente == null)
                _ingredientes.Add(new Ingrediente(nome, quantidade));
            else
                existente.Quantidade = quantidade;
        }

        public Ingrediente? BuscarPorNome(string nome) =>
            _ingredientes.FirstOrDefault(i => i.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

        public List<Ingrediente> Listar() => _ingredientes;
    }
}

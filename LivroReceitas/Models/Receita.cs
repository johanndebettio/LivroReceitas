using System;
using System.Collections.Generic;
using System.Linq;

namespace LivroReceitas.Models
{
    public class Receita(string nome)
    {
        public string Nome { get; set; } = nome;
        public List<Ingrediente> Ingredientes { get; set; } = [];

        public void AdicionarOuAtualizarIngrediente(string nome, int quantidade)
        {
            var ing = Ingredientes.FirstOrDefault(i => i.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (ing == null)
                Ingredientes.Add(new Ingrediente(nome, quantidade));
            else
                ing.Quantidade = quantidade;
        }

        public int ObterQuantidade(string nome) =>
            Ingredientes.FirstOrDefault(i => i.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))?.Quantidade ?? 0;
    }
}

namespace LivroReceitas.Models
{
    public class Ingrediente(string nome, int quantidade)
    {
        public string Nome { get; set; } = nome;
        public int Quantidade { get; set; } = quantidade;
    }
}

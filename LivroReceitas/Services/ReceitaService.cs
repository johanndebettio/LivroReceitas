using System.Collections.Generic;
using System.Linq;
using LivroReceitas.Models;
using LivroReceitas.Repositories;

namespace LivroReceitas.Services
{
    public class ReceitaService
    {
        private readonly IReceitaRepository _receitaRepo;
        private readonly IIngredienteRepository _ingredienteRepo;

        public ReceitaService(IReceitaRepository receitaRepo, IIngredienteRepository ingredienteRepo)
        {
            _receitaRepo = receitaRepo;
            _ingredienteRepo = ingredienteRepo;
        }

        public List<Receita> Receitas100Compatíveis() =>
            _receitaRepo.Listar()
                .Where(r => r.Ingredientes.All(i =>
                    (_ingredienteRepo.BuscarPorNome(i.Nome)?.Quantidade ?? 0) >= i.Quantidade))
                .ToList();

        public List<Receita> ReceitasParciais() =>
            _receitaRepo.Listar()
                .Where(r => r.Ingredientes.Any(i =>
                    (_ingredienteRepo.BuscarPorNome(i.Nome)?.Quantidade ?? 0) >= i.Quantidade)
                    && !Receitas100Compatíveis().Contains(r))
                .ToList();

        public List<string> GetMensagensReceitas100()
        {
            var mensagens = new List<string>();

            foreach (var r in Receitas100Compatíveis())
            {
                var linhas = $"Nome: {r.Nome}\n";

                foreach (var i in r.Ingredientes)
                {
                    int estoque = _ingredienteRepo.BuscarPorNome(i.Nome)?.Quantidade ?? 0;
                    int sobra = estoque - i.Quantidade;
                    string msgSobra = sobra == 0
                        ? $"Não está sobrando nenhum {i.Nome.ToLower()} para essa receita."
                        : $"Está sobrando {sobra} {i.Nome.ToLower()}(s).";
                    linhas += $"{i.Nome}: necessário {i.Quantidade}, em estoque {estoque} - {msgSobra}\n";
                }
                linhas += "-----------------------------";
                mensagens.Add(linhas);
            }
            return mensagens;
        }

        public List<string> GetMensagensReceitasParciais()
        {
            var mensagens = new List<string>();

            foreach (var r in ReceitasParciais())
            {
                var linhas = $"Nome: {r.Nome}\n";

                foreach (var i in r.Ingredientes)
                {
                    int estoque = _ingredienteRepo.BuscarPorNome(i.Nome)?.Quantidade ?? 0;
                    if (i.Quantidade > estoque)
                        linhas += $"{i.Nome}: disponível {estoque}, necessário {i.Quantidade} - Adicionar {i.Quantidade - estoque}\n";
                    else
                        linhas += $"{i.Nome}: não é necessário adicionar.\n";
                }
                linhas += "-----------------------------";
                mensagens.Add(linhas);
            }
            return mensagens;
        }
    }
}

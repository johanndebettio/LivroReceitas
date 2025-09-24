using System;
using System.Linq;
using LivroReceitas.Models;
using LivroReceitas.Repositories;
using LivroReceitas.Services;

namespace LivroReceitas.UI
{
    class Program
    {
        static void Main()
        {
            IReceitaRepository receitaRepo = new ReceitaRepository();
            IIngredienteRepository ingredienteRepo = new IngredienteRepository();
            var receitaService = new ReceitaService(receitaRepo, ingredienteRepo);

            ExibirMenu(receitaRepo, ingredienteRepo, receitaService);
        }

        static void ExibirMenu(IReceitaRepository receitaRepo, IIngredienteRepository ingredienteRepo, ReceitaService receitaService)
        {
            while (true)
            {
                Console.WriteLine("\n------- LIVRO DE RECEITAS -------");
                Console.WriteLine("1 - Cadastrar receita");
                Console.WriteLine("2 - Editar receita");
                Console.WriteLine("3 - Listar receitas");
                Console.WriteLine("4 - Cadastrar ingredientes");
                Console.WriteLine("5 - Editar ingredientes");
                Console.WriteLine("6 - Listar ingredientes");
                Console.WriteLine("7 - Listar receitas com todos os ingredientes");
                Console.WriteLine("8 - Listar receitas com ingredientes parciais");
                Console.WriteLine("9 - Sair");
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine()?.Trim();

                switch (opcao)
                {
                    case "1": 
                        CadastrarReceita(receitaRepo, ingredienteRepo); 
                        break;
                    case "2": 
                        EditarReceita(receitaRepo, ingredienteRepo); 
                        break;
                    case "3": 
                        ListarReceitas(receitaRepo); 
                        break;
                    case "4": 
                        CadastrarIngredientes(ingredienteRepo); 
                        break;
                    case "5": 
                        EditarIngredientes(ingredienteRepo); 
                        break;
                    case "6": 
                        ListarIngredientes(ingredienteRepo); 
                        break;
                    case "7": 
                        ListarReceitasComTodosOsIngredientes(receitaService); 
                        break;
                    case "8": 
                        ListarReceitasComIngredientesParciais(receitaService); 
                        break;
                    case "9": 
                        return;
                    default: 
                        Console.WriteLine("Opção inválida!"); 
                        break;
                }
            }
        }

        static int LerInteiroPositivo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int valor) && valor >= 0) 
                    return valor;

                Console.WriteLine("Informe um valor inteiro >= 0.");
            }
        }

        static void CadastrarReceita(IReceitaRepository receitaRepo, IIngredienteRepository ingredienteRepo)
        {
            Console.Write("Nome da receita: ");
            var receita = new Receita(Console.ReadLine()!.Trim());

            while (true)
            {
                Console.Write("Nome do ingrediente (ou ENTER para terminar): ");
                var nomeIng = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(nomeIng)) 
                    break;

                int quantidade = LerInteiroPositivo("Quantidade: ");
                receita.AdicionarOuAtualizarIngrediente(nomeIng, quantidade);

                if (ingredienteRepo.BuscarPorNome(nomeIng) == null)
                    ingredienteRepo.Salvar(new Ingrediente(nomeIng, 0));
            }

            receitaRepo.Adicionar(receita);
            Console.WriteLine("Receita cadastrada!");
        }

        static void EditarReceita(IReceitaRepository receitaRepo, IIngredienteRepository ingredienteRepo)
        {
            if (!receitaRepo.Listar().Any())
            { 
                Console.WriteLine("Nenhuma receita cadastrada."); 
                return; 
            }

            Console.Write("Nome da receita a editar: ");
            var receita = receitaRepo.BuscarPorNome(Console.ReadLine()!.Trim());

            if (receita == null) 
            { 
                Console.WriteLine("Receita não encontrada."); 
                return; 
            }

            Console.Write("Novo nome (ou ENTER para manter): ");
            var novoNome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoNome)) 
                receita.Nome = novoNome.Trim();

            while (true)
            {
                Console.Write("Nome do ingrediente (ou ENTER para terminar): ");
                var nomeIng = Console.ReadLine()!.Trim();
                if (string.IsNullOrWhiteSpace(nomeIng)) 
                    break;

                int quantidade = LerInteiroPositivo("Quantidade: ");
                receita.AdicionarOuAtualizarIngrediente(nomeIng, quantidade);

                if (ingredienteRepo.BuscarPorNome(nomeIng) == null)
                    ingredienteRepo.Salvar(new Ingrediente(nomeIng, 0));
            }

            Console.WriteLine("Receita editada!");
        }

        static void ListarReceitas(IReceitaRepository receitaRepo)
        {
            if (!receitaRepo.Listar().Any()) 
            { 
                Console.WriteLine("Nenhuma receita."); 
                return; 
            }

            foreach (var r in receitaRepo.Listar())
            {
                Console.WriteLine($"Nome: {r.Nome}");
                foreach (var i in r.Ingredientes)
                    Console.WriteLine($"  {i.Nome}: {i.Quantidade}");
            }
        }

        static void CadastrarIngredientes(IIngredienteRepository ingredienteRepo)
        {
            Console.Write("Nome do ingrediente: ");
            var nome = Console.ReadLine()!.Trim();

            var quantidade = LerInteiroPositivo("Quantidade: ");
            ingredienteRepo.Salvar(new Ingrediente(nome, quantidade));

            Console.WriteLine("Ingrediente cadastrado!");
        }

        static void EditarIngredientes(IIngredienteRepository ingredienteRepo)
        {
            if (!ingredienteRepo.Listar().Any()) 
            { 
                Console.WriteLine("Nenhum ingrediente."); 
                return; 
            }

            Console.Write("Nome do ingrediente a editar: ");
            var ing = ingredienteRepo.BuscarPorNome(Console.ReadLine()!.Trim());

            if (ing == null) 
            { 
                Console.WriteLine("Ingrediente não encontrado."); 
                return; 
            }

            ing.Quantidade = LerInteiroPositivo("Nova quantidade: ");
            Console.WriteLine("Ingrediente editado!");
        }

        static void ListarIngredientes(IIngredienteRepository ingredienteRepo)
        {
            if (!ingredienteRepo.Listar().Any()) 
            { 
                Console.WriteLine("Nenhum ingrediente."); 
                return; 
            }

            foreach (var i in ingredienteRepo.Listar())
                Console.WriteLine($"{i.Nome}: {i.Quantidade}");
        }

        static void ListarReceitasComTodosOsIngredientes(ReceitaService service)
        {
            var mensagens = service.GetMensagensReceitas100();
            if (!mensagens.Any()) 
                Console.WriteLine("Nenhuma receita 100% compatível.");

            foreach (var msg in mensagens) 
                Console.WriteLine(msg);
        }

        static void ListarReceitasComIngredientesParciais(ReceitaService service)
        {
            var mensagens = service.GetMensagensReceitasParciais();
            if (!mensagens.Any()) 
                Console.WriteLine("Nenhuma receita parcialmente compatível.");

            foreach (var msg in mensagens) 
                Console.WriteLine(msg);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using ConsoleTables;

namespace SistemaSupermercado
{
    class Program
    {
        public int MostrarMenu()
        {
            Console.WriteLine("O que você deseja fazer?");
            Console.WriteLine();
            Console.WriteLine("1 - Incluir cliente na fila:");
            Console.WriteLine("2 - Exibir fila");
            Console.WriteLine("3 - Atender primeiro da fila");
            Console.WriteLine("4 - Mostrar o total arrecadado no dia");
            Console.WriteLine("5 - Cancelar compra do último cliente");
            Console.WriteLine("6 - Listar produtos por ordem alfabética");
            Console.WriteLine("7 - Listar produtos por menor preço");
            Console.WriteLine("8 - Encerrar programa");
            Console.WriteLine();

            int opcao;
            while (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine("Opção inválida. Digite um número válido:");
            }
            return opcao;
        }

        // Change the declaration of `ultimaCompra` to be static since it is being accessed in a static context.
        static Stack<(string cliente, string produto, double preco, int quantidade)> ultimaCompra = new Stack<(string, string, double, int)>();


        static void Main(string[] args)
        {
            Program program = new Program();
            Queue<string> fila = new Queue<string>();
            string[] clientes = { "Vinicias", "Rafinha Caixeta", "Arthura", "Filipa", "Estebana Peruana", "Bruna", "Henrica" };
            int indiceCliente = 0;

            Random random = new Random();
            int resetaFila = 0;
            indiceCliente = random.Next(0, clientes.Length);

            string[,] matrizProdutos = new string[7, 3]
            {
                { "Arroz", "25.99", "10" },
                { "Feijão", "14.49", "20" },
                { "Batata", "2.99", "15" },
                { "Carne(Kg)", "55.99", "20" },
                { "Leite", "3.49", "30" },
                { "Ovos(Uni.)", "1.99", "50" },
                { "Chocolate", "4.79", "25" }
            };
            double totalDoDia = 0.0;


            static string CentralizarTexto(string texto, int largura)
            {
                if (string.IsNullOrEmpty(texto))
                    return new string(' ', largura);

                int espaços = largura - texto.Length;
                int padLeft = espaços / 2 + texto.Length;
                return texto.PadLeft(padLeft).PadRight(largura);
            }


            void ExibirFila()
            {
                Console.WriteLine("Fila de clientes:");
                var tabelaFila = new ConsoleTable("Posição", "Cliente");
                if (fila.Count == 0)
                {
                    Console.WriteLine("Nenhum cliente na fila.");
                    return;
                }
                int posicao = 1;
                foreach (var cliente in fila)
                {
                    tabelaFila.AddRow(posicao, cliente);
                    posicao++;
                }
                tabelaFila.Configure(o => o.EnableCount = false);
                tabelaFila.Write();
                Console.WriteLine("Total de clientes na fila: " + fila.Count);
            }

            static void MatrizParaArray(string[,] matrizProdutos, out string[] produtosMercado, out double[] preco, out int[] quantidadesDeProdutos)
            {
                int tamanho = matrizProdutos.GetLength(0);
                produtosMercado = new string[tamanho];
                preco = new double[tamanho];
                quantidadesDeProdutos = new int[tamanho];

                for (int i = 0; i < tamanho; i++)
                {
                    produtosMercado[i] = matrizProdutos[i, 0];
                    preco[i] = double.Parse(matrizProdutos[i, 1], CultureInfo.InvariantCulture);
                    quantidadesDeProdutos[i] = int.Parse(matrizProdutos[i, 2]);
                }
            }

            int opcao = program.MostrarMenu();

            do
            {
                string[] produtosMercado;
                double[] preco;
                int[] quantidadeDeProdutos;
                MatrizParaArray(matrizProdutos, out produtosMercado, out preco, out quantidadeDeProdutos);

                switch (opcao)
                {
                    case 1:
                        Console.Clear();
                        if (indiceCliente < clientes.Length)
                        {
                            string proximoCliente = clientes[indiceCliente];
                            fila.Enqueue(proximoCliente);
                            Console.WriteLine("Cliente adicionado à fila: " + proximoCliente);
                            Console.WriteLine();
                            indiceCliente++;
                            if (indiceCliente >= clientes.Length)
                            {
                                indiceCliente = resetaFila;
                            }
                        }
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;

                    case 2:
                        Console.Clear();
                        ExibirFila();
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;

                    case 3:
                        Console.Clear();
                        if (fila.Count > 0)
                        {
                            string clienteAtendido = fila.Dequeue();
                            Console.WriteLine($"Cliente atendido: {clienteAtendido}");

                            int quantidadeItens = random.Next(1, 4);
                            Console.WriteLine($"Itens comprados: ");
                            var tabela = new ConsoleTable("Produto", "Preço (R$)", "Quantidade");
                            double somaCompras = 0.0;
                            HashSet<string> produtosEsgotados = new HashSet<string>();
                            HashSet<int> indicesSelecionados = new HashSet<int>();

                            for (int i = 0; i < quantidadeItens; i++)
                            {
                                int itemIndex;
                                int tentativas = 0;
                                int maxTentativas = 10; 

                                do
                                {
                                    itemIndex = random.Next(0, produtosMercado.Length);
                                    tentativas++;
                                }
                                while (
                                    (indicesSelecionados.Contains(itemIndex) || 
                                     produtosEsgotados.Contains(produtosMercado[itemIndex])) && 
                                    tentativas < maxTentativas
                                );

                                if (tentativas >= maxTentativas)
                                {
                                    Console.WriteLine("Não foi possível encontrar um produto disponível para compra.");
                                    break;
                                }

                                indicesSelecionados.Add(itemIndex); 

                                int quantidade = random.Next(1, 5);
                                int estoqueAtual = int.Parse(matrizProdutos[itemIndex, 2]);

                                if (estoqueAtual >= quantidade)
                                {
                                    tabela.AddRow(
                                        produtosMercado[itemIndex],
                                        preco[itemIndex].ToString("C2", new CultureInfo("pt-BR")),
                                        quantidade
                                    );

                                    somaCompras += preco[itemIndex] * quantidade;
                                    quantidadeDeProdutos[itemIndex] -= quantidade;
                                    matrizProdutos[itemIndex, 2] = (estoqueAtual - quantidade).ToString();
                                    ultimaCompra.Push((clienteAtendido, produtosMercado[itemIndex], preco[itemIndex], quantidade));
                                }
                                else
                                {
                                    if (!produtosEsgotados.Contains(produtosMercado[itemIndex]))
                                    {
                                        Console.WriteLine($"Produto {produtosMercado[itemIndex]} esgotado. Não foi possível adicionar à compra.");
                                        produtosEsgotados.Add(produtosMercado[itemIndex]);
                                    }
                                    i--; 
                                }
                            }

                            totalDoDia += somaCompras;
                            tabela.AddRow("", "TOTAL", somaCompras.ToString("C2", new CultureInfo("pt-BR")));
                            tabela.Configure(o => o.EnableCount = false);
                            tabela.Write();
                            Console.WriteLine("Atendimento finalizado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Não existe cliente na fila.");
                        }
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine($"Total arrecadado no dia: {totalDoDia.ToString("C2", new CultureInfo("pt-BR"))}");
                        Console.WriteLine();
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;

                    // Rafael Caixeta
                    case 5:
                        Console.Clear();
                        if (ultimaCompra.Count > 0)
                        {
                            Console.WriteLine("Deseja cancelar a compra do último cliente atendido? (S/N)");
                            string resposta = Console.ReadLine().ToUpper();

                            if (resposta == "S")
                            {
                                Console.WriteLine("Cancelando a compra do último cliente atendido...");
                                var tabelaCancelamento = new ConsoleTable("Produto", "Preço (R$)", "Quantidade");

                                var ultimoItem = ultimaCompra.Peek();
                                string clienteCancelado = ultimoItem.cliente;
                                double totalDevolvido = 0.0;

                                while (ultimaCompra.Count > 0 && ultimaCompra.Peek().cliente == clienteCancelado)
                                {
                                    var item = ultimaCompra.Pop(); 
                                    tabelaCancelamento.AddRow(
                                        item.produto,
                                        item.preco.ToString("C2", new CultureInfo("pt-BR")),
                                        item.quantidade
                                    );

                                    int itemIndex = Array.IndexOf(produtosMercado, item.produto);
                                    if (itemIndex >= 0)
                                    {
                                        quantidadeDeProdutos[itemIndex] += item.quantidade;
                                        matrizProdutos[itemIndex, 2] = quantidadeDeProdutos[itemIndex].ToString();
                                    }

                                    totalDevolvido += item.preco * item.quantidade;
                                }

                                tabelaCancelamento.AddRow("", "TOTAL", totalDevolvido.ToString("C2", new CultureInfo("pt-BR")));
                                tabelaCancelamento.Configure(o => o.EnableCount = false);
                                tabelaCancelamento.Write();

                                Console.WriteLine($"Compra cancelada. Valor devolvido: {totalDevolvido.ToString("C2")}");
                                totalDoDia -= totalDevolvido;
                            }
                            else
                            {
                                Console.WriteLine("Operação cancelada pelo usuário.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Não há compras para cancelar.");
                        }
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.Clear();
                        (string nome, double preco, int quantidade)[] produtosCompletos = new (string, double, int)[produtosMercado.Length];

                        for (int i = 0; i < produtosMercado.Length; i++)
                        {
                            produtosCompletos[i] = (produtosMercado[i], preco[i], quantidadeDeProdutos[i]);
                        }
                        Array.Sort(produtosCompletos, (x, y) => string.Compare(x.nome, y.nome, StringComparison.OrdinalIgnoreCase));

                        Console.WriteLine("Produtos ordenados por ordem alfabética:");
                        var tabelaOrdAlfabetica = new ConsoleTable("Produto", "Preço (R$)", "Quantidade");
                        foreach (var produto in produtosCompletos)
                        {
                            tabelaOrdAlfabetica.AddRow(produto.nome, produto.preco.ToString("F2", new CultureInfo("pt-BR")), produto.quantidade);
                        }
                        
                        tabelaOrdAlfabetica.Configure(o => o.EnableCount = false);
                        tabelaOrdAlfabetica.Write();
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;
                    
                    case 7:
                        Console.Clear();
                        Array.Sort(preco, produtosMercado);

                        Console.WriteLine("Produtos ordenados por menor preço:");
                        int[] quantidadeDeProdutosOrdenado = new int[quantidadeDeProdutos.Length];
                        for (int i = 0; i < preco.Length; i++)
                        {
                            int index = Array.IndexOf(produtosMercado, matrizProdutos[i, 0]);
                            quantidadeDeProdutosOrdenado[i] = quantidadeDeProdutos[index];
                        }
                        quantidadeDeProdutos = quantidadeDeProdutosOrdenado;
                        var tabelaMenorPreco = new ConsoleTable("Produto", "Preço (R$)", "Quantidade");
                        for (int i = 0; i < preco.Length; i++)
                        {
                            tabelaMenorPreco.AddRow(produtosMercado[i], preco[i].ToString("F2", new CultureInfo("pt-BR")), quantidadeDeProdutos[i]);
                        }
                        tabelaMenorPreco.Configure(o => o.EnableCount = false);
                        tabelaMenorPreco.Write();
                       

                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;
                      
                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida. \nTente novamente.");
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;
                }

                if (opcao != 8)
                {
                    Console.Clear();
                    opcao = program.MostrarMenu();
                }
            } while (opcao != 8);

            Console.Clear();
            Console.WriteLine("Encerrando programa...");
        }
    }
}

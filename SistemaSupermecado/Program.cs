using System;
using System.Collections.Generic;
using System.Globalization;
using ConsoleTables;

namespace SistemaSupermercado
{
    class Program
    {
        public int MostrarMenu()
        {
            Console.WriteLine("O que você deseja fazer?");
            Console.WriteLine();
            Console.WriteLine("1 - Verificar produtos disponíveis no mercado:");
            Console.WriteLine("2 - Incluir cliente na fila");
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

            // Lista para armazenar as compras do último cliente atendido
            List<(string produto, double preco, int quantidade)> ultimaCompra = new List<(string, double, int)>();

            static string CentralizarTexto(string texto, int largura)
            {
                if (string.IsNullOrEmpty(texto))
                    return new string(' ', largura);

                int espaços = largura - texto.Length;
                int padLeft = espaços / 2 + texto.Length;
                return texto.PadLeft(padLeft).PadRight(largura);
            }

            void ExibirProdutos(string[,] produtos)
            {
                Console.WriteLine("Produtos disponíveis no mercado:");
                Console.WriteLine();
                var tabela = new ConsoleTable("Produto", "Preço (R$)", "Unidades");
                int larguraProduto = 12;
                int larguraPreco = 12;
                int larguraUnidades = 10;
                for (int i = 0; i < produtos.GetLength(0); i++)
                {
                    tabela.AddRow(
                        CentralizarTexto(produtos[i, 0], larguraProduto),
                        CentralizarTexto(double.Parse(produtos[i, 1], CultureInfo.InvariantCulture).ToString("C2", new CultureInfo("pt-BR")), larguraPreco),
                        CentralizarTexto(produtos[i, 2], larguraUnidades)
                    );
                }
                tabela.Configure(o => o.EnableCount = false);
                tabela.Write();
            }

            void ExibirFila()
            {
                int posicao = 1;
                foreach (var cliente in fila)
                {
                    Console.WriteLine($"{posicao++}. {cliente}");
                }
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
                        ExibirProdutos(matrizProdutos);
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;

                    case 2:
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

                            for (int i = 0; i < quantidadeItens; i++)
                            {
                                int itemIndex = random.Next(0, produtosMercado.Length);
                                int quantidade = random.Next(1, 5);

                                int estoqueAtual = int.Parse(matrizProdutos[itemIndex, 2]);

                                if (estoqueAtual >= quantidade)
                                {
                                    tabela.AddRow(produtosMercado[itemIndex],
                                                 preco[itemIndex].ToString("C2", new CultureInfo("pt-BR")),
                                                 quantidade);

                                    somaCompras += preco[itemIndex] * quantidade;

                                    // Armazenar a compra do cliente
                                    ultimaCompra.Add((produtosMercado[itemIndex], preco[itemIndex], quantidade));

                                    quantidadeDeProdutos[itemIndex] -= quantidade;
                                    matrizProdutos[itemIndex, 2] = (estoqueAtual - quantidade).ToString();
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
                            Console.WriteLine("Cancelando a compra do último cliente atendido...");
                            foreach (var item in ultimaCompra)
                            {
                                int itemIndex = Array.IndexOf(produtosMercado, item.produto);
                                quantidadeDeProdutos[itemIndex] += item.quantidade;
                                matrizProdutos[itemIndex, 2] = quantidadeDeProdutos[itemIndex].ToString();
                                Console.WriteLine($"Produto: {item.produto}, Quantidade: {item.quantidade} devolvida ao estoque.");
                            }
                            ultimaCompra.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Não há compras para cancelar.");
                        }
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

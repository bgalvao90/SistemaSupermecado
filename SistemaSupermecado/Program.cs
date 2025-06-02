using System;
using System.Collections.Generic;
using System.Globalization;
using ConsoleTables;

namespace SistemaSupermecado
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
            Console.WriteLine("5 - Cancelar compra do ultimo cliente");
            Console.WriteLine("6 - Listar produtos por ordem alfabetica");
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

            string[] produtosMercado = { "Arroz", "Feijão", "Batata", "Carne", "Leite", "Ovos" };
            double[] precos = { 25.99, 14.49, 2.99, 55.99, 3.49, 1.99 };
            int[] quantidadeDeProdutos = { 10, 20, 15, 5, 30, 50 };

            static string CentralizarTexto(string texto, int largura)
            {
                if (string.IsNullOrEmpty(texto))
                    return new string(' ', largura);

                int espaços = largura - texto.Length;
                int padLeft = espaços / 2 + texto.Length;
                return texto.PadLeft(padLeft).PadRight(largura);
            }

            int opcao = program.MostrarMenu();

            do
            {
                switch (opcao)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Produtos disponíveis no mercado:");

                        var tabela = new ConsoleTable("Produto", "Preço (R$)", "Unidades");

                        int larguraProduto = 12;
                        int larguraPreco = 12;
                        int larguraUnidades = 10;

                        for (int i = 0; i < produtosMercado.Length; i++)
                        {
                            tabela.AddRow(
                                CentralizarTexto(produtosMercado[i], larguraProduto),
                                CentralizarTexto(precos[i].ToString("C2"), larguraPreco),
                                CentralizarTexto(quantidadeDeProdutos[i].ToString(), larguraUnidades)
                            );
                        }
                        tabela.Configure(o => o.EnableCount = false);
                        tabela.Write();
                       
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
                            tabela = new ConsoleTable("Produto", "Preço (R$)", "Quantidade");
                            double somaCompras = 0.0;
                            for (int i = 0; i < quantidadeItens; i++)
                            {
                                int itemIndex = random.Next(0, produtosMercado.Length);
                                int quantidade = random.Next(1, 5);


                                tabela.AddRow(produtosMercado[itemIndex], precos[itemIndex].ToString("C2"), quantidade);

                                somaCompras += precos[itemIndex] * quantidade;
                                
                                quantidadeDeProdutos[itemIndex] -= quantidade;
                            }
                            tabela.AddRow("","TOTAL", somaCompras.ToString("C2"));

                            tabela.AddRow("", "TOTAL", somaCompras.ToString("C2", new CultureInfo("pt-BR")));
                            tabela.Configure(o => o.EnableCount = false);
                            tabela.Write();
                            Console.WriteLine();
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
                        Console.WriteLine($"Total arrecadado no dia: {totalDoDia:C2}");
                        Console.WriteLine();
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida. \nTente novamente.");
                        Console.WriteLine("Pressione enter para continuar...");
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine($"Total arrecadado no dia: {totalDoDia:C2}");
                        Console.WriteLine();
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
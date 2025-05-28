using System;
using System.Collections.Generic;
using System.Linq;
namespace SistemaSupermecado
{
    class Program
    {
        public void Menu()
        {
            Console.WriteLine("O que você deseja fazer?");
            Console.WriteLine();
            Console.WriteLine("1 - Verificar produtos disponíveis no mercado:");
            Console.WriteLine("2 - Atender primeiro da fila");
            Console.WriteLine("3 - Cancelar compra do ultimo cliente");
            Console.WriteLine("4 - Listar produtos por ordem alfabetica");
            Console.WriteLine("5 - Listar produtos por menor preço");
            Console.WriteLine("6 - Encerrar programa");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            Queue<string> fila = new Queue<string>();
            string[] clientes = { "Vinicias", "Rafinha Caixeta", "Arthura", "Filipa", "Estebana Peruana", "Bruna", "Henrica" };
            int indiceCliente = 0;

            string[] produtosMercado = { "Arroz", "Feijão", "Batata", "Carne", "Leite", "Ovos" };
            double[] precos = { 25.99, 14.49, 2.99, 55.99, 3.49, 1.99 };
            int[] quantidadeDeProdutos = { 10, 20, 15, 5, 30, 50 };

            Random random = new Random();
            indiceCliente = random.Next(0, clientes.Length);

            int unidadeCompradaPorCiente = random.Next(1, 5);

            if (quantidadeDeProdutos[indiceCliente] < unidadeCompradaPorCiente)
            {
                Console.WriteLine("Quantidade de produtos insuficiente para a compra.");
                return;
            }

            double somaProdutos = 0;

            for (int i = 0; i < precos.Length; i++)
            {
                somaProdutos += precos[i] * unidadeCompradaPorCiente;
            }

            program.Menu();

            int opcao = int.Parse(Console.ReadLine());
            do
            {
                switch (opcao)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Produtos disponíveis no mercado:");

                        for (int i = 0; i < produtosMercado.Length; i++)
                        {
                            Console.WriteLine($"- {produtosMercado[i]}: {precos[i]:C2} : {quantidadeDeProdutos[i]} unidades");
                        }
                        Console.WriteLine();

                        program.Menu();
                        opcao = int.Parse(Console.ReadLine());
                        break;

                    case 2:
                        if (indiceCliente < clientes.Length)
                        {
                            Console.Clear();
                            string proximoCliente = clientes[indiceCliente];
                            fila.Enqueue(proximoCliente);
                            Console.WriteLine($"Cliente adicionado à fila: {proximoCliente}");
                            indiceCliente++;
                            Console.WriteLine($"Quais itens o(a) {proximoCliente} comprou");

                            int itemIndex = random.Next(0, produtosMercado.Length);
                            int quantidade = random.Next(1, unidadeCompradaPorCiente);

                            string itensComprados = $"{produtosMercado[itemIndex]} - Preço: {precos[itemIndex]:C2} - Quantidade: {quantidade} unidades";

                            Console.WriteLine($"Itens comprados: {itensComprados}");

                            quantidadeDeProdutos[itemIndex] -= quantidade;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Não existe mais cliente na fila.");
                        }
                        Console.WriteLine();

                        program.Menu();
                        opcao = int.Parse(Console.ReadLine());
                        break;

                    case 3:
                        if (fila.Count > 0)
                        {
                            Console.Clear();
                            string clienteAtendido = fila.Dequeue();
                            Console.WriteLine($"Cliente atendido: {clienteAtendido}");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Não existe cliente na fila.");
                        }
                        Console.WriteLine();
                        program.Menu();
                        opcao = int.Parse(Console.ReadLine());
                        break;
                }
            } while (opcao != 6);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
namespace SistemaSupermecado
{
    class Program
    {
        public void Menu()
        {
            Console.WriteLine("Escolha uma opção abaixo para continuar:");
            Console.WriteLine("1 - Incluir cliente na fila");
            Console.WriteLine("2 - Atender primeiro da fila");
            Console.WriteLine("3 - Cancelar compra do ultimo cliente");
            Console.WriteLine("4 - Listar produtos por ordem alfabetica");
            Console.WriteLine("5 - Listar produtos por menor preço");
            Console.WriteLine("6 - Encerrar programa");
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            Queue<string> fila = new Queue<string>();
            string[] clientes = { "Vinicias", "Rafinha Caixeta", "Arthura", "Filipa", "Estebana Peruana", "Bruna" };
            int indiceCliente = 0;

            program.Menu();  

            int opcao = int.Parse(Console.ReadLine());
            do
            {
                switch (opcao)
                {
                    case 1:
                        if (indiceCliente < clientes.Length)
                        {
                            Console.Clear();
                            string proximoCliente = clientes[indiceCliente];
                            fila.Enqueue(proximoCliente);
                            Console.WriteLine($"Cliente adicionado à fila: {proximoCliente}");
                            indiceCliente++;
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

                    case 2:
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





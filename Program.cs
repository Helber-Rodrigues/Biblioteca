using System;

class Node
{
    public int Value;
    public Node Left;
    public Node Right;

    public Node(int value)
    {
        Value = value;
        Left = null;
        Right = null;
    }
}

class BinarySearchTree
{
    private Node root;

    public BinarySearchTree()
    {
        root = null;
    }

    public void Insert(int value)
    {
        root = InsertRec(root, value);
    }

    private Node InsertRec(Node root, int value)
    {
        if (root == null)
        {
            root = new Node(value);
            return root;
        }

        if (value < root.Value)
            root.Left = InsertRec(root.Left, value);
        else if (value > root.Value)
            root.Right = InsertRec(root.Right, value);

        return root;
    }

    public void InOrderTraversal()
    {
        InOrderRec(root);
        Console.WriteLine();
    }

    private void InOrderRec(Node root)
    {
        if (root != null)
        {
            InOrderRec(root.Left);
            Console.Write(root.Value + " ");
            InOrderRec(root.Right);
        }
    }

    public void PrintTree()
    {
        if (root == null)
        {
            Console.WriteLine("A árvore está vazia");
            return;
        }

        PrintTreeRec(root, 0);
    }

    private void PrintTreeRec(Node root, int space)
    {
        int COUNT = 10; // Define a distância entre os níveis
        if (root == null)
            return;

        space += COUNT;

        PrintTreeRec(root.Right, space);

        Console.WriteLine();
        for (int i = COUNT; i < space; i++)
            Console.Write(" ");
        Console.WriteLine(root.Value);

        PrintTreeRec(root.Left, space);
    }

    public bool Find(int value)
    {
        Node current = root;
        while (current != null)
        {
            if (value < current.Value)
            {
                current = current.Left;
            }
            else if (value > current.Value)
            {
                current = current.Right;
            }
            else
            {
                return true; // Valor encontrado
            }
        }
        return false; // Valor não encontrado
    }


    public void Delete(int value)
    {
        root = DeleteRec(root, value);
    }

    private Node DeleteRec(Node root, int value)
    {
        if (root == null) return root;

        // Procurando o valor
        if (value < root.Value)
            root.Left = DeleteRec(root.Left, value);
        else if (value > root.Value)
            root.Right = DeleteRec(root.Right, value);
        else
        {
            // Nó com apenas um filho ou sem filhos
            if (root.Left == null)
                return root.Right;
            else if (root.Right == null)
                return root.Left;

            // Nó com dois filhos: Pegue o sucessor em ordem (menor na subárvore direita)
            root.Value = MinValue(root.Right);

            // Delete o sucessor
            root.Right = DeleteRec(root.Right, root.Value);
        }

        return root;
    }

    private int MinValue(Node root)
    {
        int minValue = root.Value;
        while (root.Left != null)
        {
            minValue = root.Left.Value;
            root = root.Left;
        }
        return minValue;
    }

    private int Height(Node root)
    {
        if (root == null) return -1; // Base para nó nulo
        return 1 + Math.Max(Height(root.Left), Height(root.Right));
    }

    public int BalanceFactor()
    {
        return BalanceFactor(root);
    }

    private int BalanceFactor(Node root)
    {
        if (root == null) return 0; // Um nó nulo é considerado balanceado
        return Height(root.Left) - Height(root.Right);
    }

}

class Program
{

    public static int menu()
    {
        Console.Clear();
        Console.WriteLine("0 - sair");
        Console.WriteLine("1 - add");
        Console.WriteLine("2 - Encontrar elemento");
        Console.WriteLine("3 - exclusao");
        Console.WriteLine("4 - mostrar em ordem");
        Console.WriteLine("5 - desenhar a arvore");
        Console.WriteLine("6 - FB do nó raiz");



        Console.Write("selecione a opção: ");
        return int.Parse(Console.ReadLine());
    }

    static void Main(string[] args)
    {
        BinarySearchTree bst = new BinarySearchTree();
        int opc = -1;

        while (opc != 0)
        {
            opc = menu();

            switch (opc)
            {
                case 0:
                    //sair

                    break;
                case 1:
                    //adicionar
                    Console.Write("Infome o primeiro elemento da arvore: ");
                    bst.Insert(int.Parse(Console.ReadLine()));
                    break;
                case 2:
                    //encontrar elemento
                    Console.Write("Infome o elemento que deseja encontrar: ");
                    int valueToFind = int.Parse(Console.ReadLine());
                    Console.WriteLine($"Procurando o valor {valueToFind} na árvore: " + (bst.Find(valueToFind) ? "Encontrado." : "Não encontrado."));
                    Console.ReadLine();
                    break;
                case 3:
                    //exclusão de nó
                    Console.Write("Infome o elemento que deseja excluir: ");
                    bst.Delete(int.Parse(Console.ReadLine()));
                    break;
                case 4:
                    //mostrar em ordem
                    Console.Write("A travessia em ordem da árvore construída é: ");
                    bst.InOrderTraversal();
                    Console.ReadLine();
                    break;
                case 5:
                    bst.PrintTree();
                    Console.ReadLine();
                    break;
                case 6:
                    int balanceFactor = bst.BalanceFactor();
                    Console.WriteLine($"Fator de balanceamento da raiz: {balanceFactor}");
                    Console.ReadLine();
                    break;

            }
        }

    }

}
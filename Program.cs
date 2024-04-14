using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class Livro
{
    public int ID { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int AnoPublicacao { get; set; }

    public Livro(int id, string titulo, string autor, int anoPublicacao)
    {
        ID = id;
        Titulo = titulo;
        Autor = autor;
        AnoPublicacao = anoPublicacao;
    }
}

public class NoAVL
{
    public Livro Livro { get; set; }
    public int Altura { get; set; }
    public NoAVL Esquerda { get; set; }
    public NoAVL Direita { get; set; }


    public NoAVL(Livro livro)
    {
        Livro = livro;
        Altura = 1;
    }
}

public class ArvoreAVL
{
    public NoAVL root { get; private set; }

    int Altura(NoAVL N)
    {
        if (N == null)
            return 0;
        return N.Altura;
    }
    int max(int a, int b)
    {
        return (a > b) ? a : b;
    }

    NoAVL rightRotate(NoAVL y)
    {
        NoAVL x = y.Esquerda;
        NoAVL T2 = x.Direita;

        x.Direita = y;
        y.Esquerda = T2;

        y.Altura = max(Altura(y.Esquerda), Altura(y.Direita)) + 1;
        x.Altura = max(Altura(x.Esquerda), Altura(x.Direita)) + 1;
        return x;
    }

    NoAVL leftRotate(NoAVL x)
    {
        NoAVL y = x.Direita;
        NoAVL T2 = y.Esquerda;

        y.Esquerda = x;
        x.Direita = T2;

        x.Altura = max(Altura(x.Esquerda), Altura(x.Direita)) + 1;
        y.Altura = max(Altura(y.Esquerda), Altura(y.Direita)) + 1;

        return y;
    }
    int getBalance(NoAVL N) //Verificar balanceamento do nó
    {
        if (N == null)
            return 0;
        return Altura(N.Esquerda) - Altura(N.Direita);
    }
    public NoAVL insert(NoAVL node, Livro data) //Inserir novo nó
    {

        if (node == null)
            return new NoAVL(data);

        if (data.ID < node.Livro.ID)
            node.Esquerda = insert(node.Esquerda, data);
        else if (data.ID > node.Livro.ID)
            node.Direita = insert(node.Direita, data);



        node.Altura = 1 + max(Altura(node.Esquerda),
                              Altura(node.Direita));


        var balance = getBalance(node); //Função pra verificar o balanceamento do nó e saber pra qual lado rotacionar
        if (balance > 1 && data.ID < node.Esquerda.Livro.ID)
            return rightRotate(node);
        if (balance < -1 && data.ID > node.Direita.Livro.ID)
            return leftRotate(node);
        if (balance > 1 && data.ID > node.Esquerda.Livro.ID)
        {
            node.Esquerda = leftRotate(node.Esquerda);
            return rightRotate(node);
        }
        if (balance < -1 && data.ID < node.Direita.Livro.ID)
        {
            node.Direita = rightRotate(node.Direita);
            return leftRotate(node);
        }
        return node;
    }

    // Função travessia em ordem
    public void inorder(NoAVL node)
    {
        if (node != null)
        {
            inorder(node.Esquerda);
            Console.Write(node.Livro.ID + " ");
            inorder(node.Direita);
        }
    }

    public NoAVL balance(NoAVL node)
    {
        int balanceFactor = getBalance(node);
        if (balanceFactor > 1)
        {
            if (getBalance(node.Esquerda) >= 0)
            {
                return rightRotate(node);
            }
            else
            {
                node.Esquerda = leftRotate(node.Esquerda);
                return rightRotate(node);
            }
        }
        else if (balanceFactor < -1)
        {
            if (getBalance(node.Direita) <= 0)
            {
                return leftRotate(node);
            }
            else
            {
                node.Direita = rightRotate(node.Direita);
                return leftRotate(node);
            }
        }
        return node;
    }

    public NoAVL deleteNode(NoAVL node, int key)
    {
        if (node == null)
            return node;

        // Se a chave a ser deletada é menor que a raiz, então está na subárvore esquerda
        if (key < node.Livro.ID)
            node.Esquerda = deleteNode(node.Esquerda, key);

        // Se a chave a ser deletada é maior que a raiz, então está na subárvore direita
        else if (key > node.Livro.ID)
            node.Direita = deleteNode(node.Direita, key);

        // Se a chave é igual à raiz, então este é o nó a ser deletado
        else
        {
            // Nó com apenas um filho ou sem filho
            if ((node.Esquerda == null) || (node.Direita == null))
            {
                NoAVL temp = null;
                if (temp == node.Esquerda)
                    temp = node.Direita;
                else
                    temp = node.Esquerda;

                // Sem filho
                if (temp == null)
                {
                    temp = node;
                    node = null;
                }
                else // Um filho
                    node = temp; // Copia o conteúdo do filho não vazio
            }
            else
            {
                // Nó com dois filhos: Pegue o sucessor em ordem (menor na subárvore direita)
                NoAVL temp = minValueNode(node.Direita);

                // Copia os dados do sucessor em ordem para este nó
                node.Livro.ID = temp.Livro.ID;

                // Deleta o sucessor em ordem
                node.Direita = deleteNode(node.Direita, temp.Livro.ID);
            }
        }

        // Se a árvore tinha apenas um nó, então retorna
        if (node == null)
            return node;

        // ATUALIZAÇÃO DA ALTURA DO NÓ ATUAL
        node.Altura = max(Altura(node.Esquerda), Altura(node.Direita)) + 1;

        // VERIFICAÇÃO DO BALANCEAMENTO
        int balance = getBalance(node);

        // Se o nó estiver desbalanceado, então há 4 casos

        // Caso Esquerda-Esquerda
        if (balance > 1 && getBalance(node.Esquerda) >= 0)
            return rightRotate(node);

        // Caso Esquerda-Direita
        if (balance > 1 && getBalance(node.Esquerda) < 0)
        {
            node.Esquerda = leftRotate(node.Esquerda);
            return rightRotate(node);
        }

        // Caso Direita-Direita
        if (balance < -1 && getBalance(node.Direita) <= 0)
            return leftRotate(node);

        // Caso Direita-Esquerda
        if (balance < -1 && getBalance(node.Direita) > 0)
        {
            node.Direita = rightRotate(node.Direita);
            return leftRotate(node);
        }

        return node;
    }


    NoAVL minValueNode(NoAVL node)
    {
        NoAVL current = node;
        while (current.Esquerda != null)
            current = current.Esquerda;
        return current;
    }

    public void printTree(NoAVL root, string indent, bool last)
    {
        if (root != null)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "| ";
            }
            Console.WriteLine(root.Livro.ID);

            printTree(root.Esquerda, indent, false);
            printTree(root.Direita, indent, true);
        }
    }

    public int getRootBalanceFactor()
    {
        if (root == null)
        { return 0; }
        else
        { return getBalance(root); }
    }

    public NoAVL Search(NoAVL root, int key)  //Função pra procurar informação na árvore
    {
        if (root == null || root.Livro.ID == key)
            return root;
        if (root.Livro.ID < key)
            return Search(root.Direita, key);
        return Search(root.Esquerda, key);
    }



    // Métodos para inserir, buscar, listar e balancear a árvore AVL
    public List<Livro> BuscarPorAnoPublicacao(NoAVL raiz, int ano)
    {
        var livros = new List<Livro>();
        // Implementar busca em ordem para encontrar livros pelo ano de publicação
        return livros;
    }

    public List<Livro> BuscarPorAutor(NoAVL raiz, string autor)
    {
        var livros = new List<Livro>();
        // Implementar busca em ordem para encontrar livros pelo autor
        return livros;
    }
}




public class SistemaGerenciamentoBiblioteca
{
    public ArvoreAVL arvore;

    public SistemaGerenciamentoBiblioteca()
    {
        ArvoreAVL arvore = new ArvoreAVL();
    }

    public void Menu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("0 - sair");
            Console.WriteLine("1 - add");
            Console.WriteLine("2 - balancear");
            Console.WriteLine("3 - exclusao");
            Console.WriteLine("4 - mostrar em ordem");
            Console.WriteLine("5 - desenhar a arvore");
            Console.WriteLine("6 - FB do nó raiz");
            Console.WriteLine("7 - Encontrar elemento");


            Console.Write("selecione a opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    // Implementar inserção de livro
                    Console.Write("Infome o ID do livro: ");
                    int cod = int.Parse(Console.ReadLine());
                    Console.Write("Infome título do livro: ");
                    string nome = Console.ReadLine();
                    Console.Write("Infome autor do livro: ");
                    string autor = Console.ReadLine();
                    Console.Write("Infome o ano de publicação do livro: ");
                    int ano = int.Parse(Console.ReadLine());

                    Livro novolivro = new Livro(cod, nome, autor, ano);
                    //arvore.root = arvore.insert(arvore.root, novolivro);

                    arvore.insert(arvore.root, new Livro(cod, nome, autor, ano));
                    break;
                case "2":
                    // Implementar Balanceamento

                    break;
                case "3":
                    // Implementar Exclusão
                    arvore.deleteNode(arvore.root, int.Parse(Console.ReadLine()));
                    break;
                case "4":
                    //Mostrar em ordem
                    Console.Write("A travessia em ordem da árvore construída é: ");
                    arvore.inorder(arvore.root);
                    Console.ReadLine();
                    break;
                case "5":
                    //Desenhar árvore
                    arvore.printTree(arvore.root, "", true);
                    Console.ReadLine();
                    break;
                case "6":
                    //Fator de balanceamento raiz
                    int balanceFactor = arvore.getRootBalanceFactor();
                    Console.WriteLine("Fator de balanceamento do nó raiz: " + balanceFactor);
                    Console.ReadLine();
                    break;
                case "7":
                    //Encontrar elemento
                    Console.Write("informe o elemento que deseja encontrar: ");
                    int valueToFind = int.Parse(Console.ReadLine());
                    NoAVL foundNode = arvore.Search(arvore.root, valueToFind);
                    if (foundNode != null)
                        Console.WriteLine("Nó encontrado: " + foundNode.Livro);
                    else
                        Console.WriteLine("Nó não encontrado.");

                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var sistema = new SistemaGerenciamentoBiblioteca();
        sistema.Menu();
    }
}

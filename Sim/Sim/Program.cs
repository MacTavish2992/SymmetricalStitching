using System;

class Node
{
    public int Data;
    public Node Left;
    public Node Right;

    // Добавляем два поля для "прошивки"
    public bool IsThreaded;
    public Node NextInOrder;

    public Node(int data)
    {
        Data = data;
        Left = null;
        Right = null;
        IsThreaded = false;
        NextInOrder = null;
    }
}

class BinaryTree
{
    public Node Root;

    public BinaryTree()
    {
        Root = null;
    }

    public void Insert(int data)
    {
        Root = InsertRec(Root, data);
    }

    private Node InsertRec(Node root, int data)
    {
        if (root == null)
        {
            root = new Node(data);
            return root;
        }

        if (data < root.Data)
            root.Left = InsertRec(root.Left, data);
        else if (data > root.Data)
            root.Right = InsertRec(root.Right, data);

        return root;
    }

    public void InOrderTraversal(Node node)
    {
        if (node != null)
        {
            InOrderTraversal(node.Left);
            Console.Write(node.Data + " ");
            InOrderTraversal(node.Right);
        }
    }

    public void InOrderTraversal()
    {
        InOrderTraversal(Root);
        Console.WriteLine();
    }

    // Прошивка дерева
    public void ThreadTree()
    {
        Node current = Root;
        Node prev = null;

        ThreadTreeRec(current, ref prev);
    }

    private void ThreadTreeRec(Node current, ref Node prev)
    {
        if (current == null)
            return;

        ThreadTreeRec(current.Left, ref prev);

        if (prev != null && prev.Data < current.Data)
        {
            prev.NextInOrder = current;
            prev.IsThreaded = true;
        }

        prev = current;

        ThreadTreeRec(current.Right, ref prev);
    }

    // Симметричный обход после прошивки
    public void ThreadedInOrderTraversal()
    {
        Node current = LeftMostNode();
        while (current != null)
        {
            Console.Write(current.Data + " ");

            if (current.IsThreaded)
                current = current.NextInOrder;
            else
                current = LeftMostNode(current.Right);
        }
        Console.WriteLine();
    }


    // Вывод связей между узлами после прошивки
    public void PrintThreadedLinks()
    {
        PrintThreadedLinksRec(Root);
    }

    private void PrintThreadedLinksRec(Node node)
    {
        if (node == null)
            return;

        if (node.IsThreaded && node.NextInOrder != null)
        {
            Console.WriteLine($"{node.Data} -> {node.NextInOrder.Data}");
        }

        PrintThreadedLinksRec(node.Left);
        PrintThreadedLinksRec(node.Right);
    }

    private Node LeftMostNode()
    {
        Node current = Root;
        while (current != null && current.Left != null)
        {
            current = current.Left;
        }
        return current;
    }

    private Node LeftMostNode(Node node)
    {
        while (node != null && node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }
    public void InsertThreaded(int data)
    {
        if (Root == null)
        {
            Root = new Node(data);
        }
        else
        {
            InsertThreadedRec(Root, null, data);
        }
    }

    private Node InsertThreadedRec(Node current, Node parent, int data)
    {
        if (current == null)
        {
            Node newNode = new Node(data);

            if (data < parent.Data)
            {
                newNode.NextInOrder = parent;
                newNode.IsThreaded = true;
                parent.Left = newNode;
            }
            else
            {
                newNode.NextInOrder = parent.NextInOrder;
                parent.NextInOrder = newNode;
                newNode.IsThreaded = true;
                parent.Right = newNode;
            }

            return newNode;
        }

        if (data < current.Data)
        {
            return InsertThreadedRec(current.Left, current, data);
        }
        else if (data > current.Data || (data == current.Data && current.IsThreaded))
        {
            current.NextInOrder = InsertThreadedRec(current.NextInOrder, current, data);
        }

        return current;
    }





    public Node Find(int data)
    {
        return FindRec(Root, data);
    }

    private Node FindRec(Node node, int data)
    {
        if (node == null)
            return null;

        if (data == node.Data)
            return node;
        else if (data < node.Data)
            return FindRec(node.Left, data);
        else
        {
            if (node.IsThreaded)
                return FindRec(node.NextInOrder, data);
            else
                return FindRec(node.Right, data);
        }
    }

}

class Program
{
    static void Main()
    {
        BinaryTree tree = new BinaryTree();

        int[] values = { 10, 15, 7, 45, 2, 8, 1, 9, 5, 6 };

        foreach (int value in values)
        {
            tree.Insert(value);
        }

        Console.WriteLine("Симметричный обход до прошивки:");

        // Вывод значений перед прошивкой
        tree.InOrderTraversal();

        Console.WriteLine("Симметричный обход после прошивки:");

        // Прошивка дерева
        tree.ThreadTree();

        // Вывод значений после прошивки
        tree.ThreadedInOrderTraversal();

        Console.WriteLine("Связи между узлами после прошивки:");

        // Вывод связей между узлами после прошивки
        tree.PrintThreadedLinks();

        tree.InsertThreaded(16);

        Console.WriteLine("Симметричный обход прошитого дерева после вставки:");

        // Вывод прошитого дерева
        tree.ThreadedInOrderTraversal();
        Console.WriteLine("Связи между узлами после прошивки:");
        tree.PrintThreadedLinks();
        // Поиск элемента в прошитом дереве
        Node foundNode = tree.Find(46);
        if (foundNode != null)
        {
            Console.WriteLine($"Элемент {foundNode.Data} найден.");
        }
        else
        {
            Console.WriteLine("Элемент не найден.");
        }

        Console.Read();
    }
}

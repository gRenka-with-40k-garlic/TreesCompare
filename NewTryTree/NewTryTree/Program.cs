using System;
using System.Diagnostics;

public class AVLTree
{
    // Внутренний класс, представляющий узел дерева
    private class Node
    {
        public int data; // Значение узла
        public int height; // Высота узла
        public Node left; // Левый потомок
        public Node right; // Правый потомок

        public Node(int item)
        {
            data = item;
            height = 1;
            left = right = null;
        }
    }

    private Node root; // Корень дерева

    public AVLTree()
    {
        root = null;
    }

    // Метод для нахождения значения в дереве
    public bool Find(int key)
    {
        return FindRec(root, key);
    }

    // Рекурсивный метод для нахождения значения в дереве
    private bool FindRec(Node node, int key)
    {
        if (node == null)
        {
            return false; // Значение не найдено
        }
        if (key == node.data)
        {
            return true; // Значение найдено
        }
        else if (key < node.data)
        {
            return FindRec(node.left, key); // Искать влево
        }
        else
        {
            return FindRec(node.right, key); // Искать вправо
        }
    }

    // Метод для вставки значения в дерево
    public void Insert(int item)
    {
        root = InsertRec(root, item);
    }

    // Рекурсивный метод для вставки значения в дерево
    private Node InsertRec(Node node, int item)
    {
        if (node == null)
        {
            return new Node(item);
        }

        if (item < node.data)
        {
            node.left = InsertRec(node.left, item);
        }
        else if (item > node.data)
        {
            node.right = InsertRec(node.right, item);
        }
        else
        {
            return node; // Значение уже существует в дереве
        }

        // Обновить высоту узла
        node.height = 1 + Math.Max(GetHeight(node.left), GetHeight(node.right));

        // Проверить равновесие узла и выполнить необходимые повороты
        int balanceFactor = GetBalanceFactor(node);

        // Left Left Case
        if (balanceFactor > 1 && item < node.left.data)
        {
            return RightRotate(node);
        }

        // Right Right Case
        if (balanceFactor < -1 && item > node.right.data)
        {
            return LeftRotate(node);
        }

        // Left Right Case
        if (balanceFactor > 1 && item > node.left.data)
        {
            node.left = LeftRotate(node.left);
            return RightRotate(node);
        }

        // Right Left Case
        if (balanceFactor < -1 && item < node.right.data)
        {
            node.right = RightRotate(node.right);
            return LeftRotate(node);
        }

        return node;
    }

    // Метод для удаления значения из дерева
    public void Remove(int item)
    {
        root = RemoveRec(root, item);
    }

    // Рекурсивный метод для удаления значения из дерева
    private Node RemoveRec(Node node, int item)
    {
        if (node == null)
        {
            return node;
        }

        if (item < node.data)
        {
            node.left = RemoveRec(node.left, item);
        }
        else if (item > node.data)
        {
            node.right = RemoveRec(node.right, item);
        }
        else
        {
            // Узел с одним или без потомков
            if (node.left == null || node.right == null)
            {
                Node temp = node.left ?? node.right;
                if (temp == null)
                {
                    // Узел без потомков
                    temp = node;
                    node = null;
                }
                else
                {
                    // Узел с одним потомком
                    node = temp;
                }

                temp = null;
            }
            else
            {
                // Узел с двумя потомками
                Node temp = FindMin(node.right);
                node.data = temp.data;
                node.right = RemoveRec(node.right, temp.data);
            }
        }

        if (node == null)
        {
            return node;
        }

        // Обновить высоту узла
        node.height = 1 + Math.Max(GetHeight(node.left), GetHeight(node.right));

        // Проверить равновесие узла и выполнить необходимые повороты
        int balanceFactor = GetBalanceFactor(node);

        // Left Left Case
        if (balanceFactor > 1 && GetBalanceFactor(node.left) >= 0)
        {
            return RightRotate(node);
        }

        // Left Right Case
        if (balanceFactor > 1 && GetBalanceFactor(node.left) < 0)
        {
            node.left = LeftRotate(node.left);
            return RightRotate(node);
        }

        // Right Right Case
        if (balanceFactor < -1 && GetBalanceFactor(node.right) <= 0)
        {
            return LeftRotate(node);
        }

        // Right Left Case
        if (balanceFactor < -1 && GetBalanceFactor(node.right) > 0)
        {
            node.right = RightRotate(node.right);
            return LeftRotate(node);
        }

        return node;
    }

    // Метод для выполнения инфиксного обхода дерева
    public void InfixTraverse()
    {
        InfixTraverseRec(root);
        Console.WriteLine();
    }

    // Рекурсивный метод для выполнения инфиксного обхода дерева
    private void InfixTraverseRec(Node node)
    {
        if (node != null)
        {
            InfixTraverseRec(node.left);
            Console.Write(node.data + " ");
            InfixTraverseRec(node.right);
        }
    }

    // Метод для выполнения префиксного обхода дерева
    public void PrefixTraverse()
    {
        PrefixTraverseRec(root);
        Console.WriteLine();
    }

    // Рекурсивный метод для выполнения префиксного обхода дерева
    private void PrefixTraverseRec(Node node)
    {
        if (node != null)
        {
            Console.Write(node.data + " ");
            PrefixTraverseRec(node.left);
            PrefixTraverseRec(node.right);
        }
    }

    // Метод для выполнения постфиксного обхода дерева
    public void PostfixTraverse()
    {
        PostfixTraverseRec(root);
        Console.WriteLine();
    }

    // Рекурсивный метод для выполнения постфиксного обхода дерева
    private void PostfixTraverseRec(Node node)
    {
        if (node != null)
        {
            PostfixTraverseRec(node.left);
            PostfixTraverseRec(node.right);
            Console.Write(node.data + " ");
        }
    }

    // Метод для получения высоты узла
    private int GetHeight(Node node)
    {
        if (node == null)
        {
            return 0;
        }
        return node.height;
    }

    // Метод для получения разницы высоты поддеревьев левого и правого потомков узла
    private int GetBalanceFactor(Node node)
    {
        if (node == null)
        {
            return 0;
        }
        return GetHeight(node.left) - GetHeight(node.right);
    }

    // Метод для выполнения левого вращения поддерева с корнем в данном узле
    private Node LeftRotate(Node node)
    {
        Node rightChild = node.right;
        Node leftGrandchild = rightChild.left;

        // Выполнение вращения
        rightChild.left = node;
        node.right = leftGrandchild;

        // Обновление высоты узлов
        node.height = 1 + Math.Max(GetHeight(node.left), GetHeight(node.right));
        rightChild.height = 1 + Math.Max(GetHeight(rightChild.left), GetHeight(rightChild.right));

        return rightChild;
    }

    // Метод для выполнения правого вращения поддерева с корнем в данном узле
    private Node RightRotate(Node node)
    {
        Node leftChild = node.left;
        Node rightGrandchild = leftChild.right;

        // Выполнение вращения
        leftChild.right = node;
        node.left = rightGrandchild;

        // Обновление высоты узлов
        node.height = 1 + Math.Max(GetHeight(node.left), GetHeight(node.right));
        leftChild.height = 1 + Math.Max(GetHeight(leftChild.left), GetHeight(leftChild.right));

        return leftChild;
    }

    // Метод для поиска узла с минимальным значением в дереве
    private Node FindMin(Node node)
    {
        Node current = node;
        while (current.left != null)
        {
            current = current.left;
        }
        return current;
    }
}

public class BinarySearchTree
{
    // Внутренний класс, представляющий узел дерева
    private class Node
    {
        public int data; // Значение узла
        public Node left; // Левый потомок
        public Node right; // Правый потомок

        public Node(int item)
        {
            data = item;
            left = right = null;
        }
    }

    private Node root; // Корень дерева

    public BinarySearchTree()
    {
        root = null;
    }

    // Метод для поиска значения в дереве
    public bool Find(int key)
    {
        Node current = root;
        while (current != null)
        {
            if (key == current.data)
            {
                return true; // Значение найдено
            }
            else if (key < current.data)
            {
                current = current.left; // Идти влево
            }
            else
            {
                current = current.right; // Идти вправо
            }
        }
        return false; // Значение не найдено
    }

    // Метод для вставки значения в дерево
    public void Insert(int item)
    {
        root = InsertRec(root, item);
    }

    // Рекурсивный метод для вставки значения в дерево
    private Node InsertRec(Node root, int item)
    {
        if (root == null)
        {
            root = new Node(item);
            return root;
        }
        if (item < root.data)
        {
            root.left = InsertRec(root.left, item);
        }
        else if (item > root.data)
        {
            root.right = InsertRec(root.right, item);
        }
        return root;
    }

    // Метод для удаления значения из дерева
    public void Remove(int key)
    {
        root = RemoveRec(root, key);
    }

    // Рекурсивный метод для удаления значения из дерева
    private Node RemoveRec(Node root, int key)
    {
        if (root == null)
        {
            return root;
        }
        if (key < root.data)
        {
            root.left = RemoveRec(root.left, key);
        }
        else if (key > root.data)
        {
            root.right = RemoveRec(root.right, key);
        }
        else
        {
            // Узел со значением для удаления найден

            // Узел с одним или без потомков
            if (root.left == null)
            {
                return root.right;
            }
            else if (root.right == null)
            {
                return root.left;
            }

            // Узел с двумя потомками: найти наименьший узел в правом поддереве и заменить текущий узел на него
            root.data = MinValue(root.right);

            // Удалить наименьший узел в правом поддереве
            root.right = RemoveRec(root.right, root.data);
        }
        return root;
    }

    // Вспомогательный метод для поиска наименьшего значения в дереве
    private int MinValue(Node root)
    {
        int minVal = root.data;
        while (root.left != null)
        {
            minVal = root.left.data;
            root = root.left;
        }
        return minVal;
    }

    // Методы для обхода дерева
    // Инфиксный обход (левый потомок, корень, правый потомок)
    public void InfixTraverse()
    {
        InfixTraverseRec(root);
        Console.WriteLine();
    }

    // Рекурсивный метод для инфиксного обхода
    private void InfixTraverseRec(Node root)
    {
        if (root != null)
        {
            InfixTraverseRec(root.left);
            Console.Write(root.data + " ");
            InfixTraverseRec(root.right);
        }
    }

    // Префиксный обход (корень, левый потомок, правый потомок)
    public void PrefixTraverse()
    {
        PrefixTraverseRec(root);
        Console.WriteLine();
    }

    // Рекурсивный метод для префиксного обхода
    private void PrefixTraverseRec(Node root)
    {
        if (root != null)
        {
            Console.Write(root.data + " ");
            PrefixTraverseRec(root.left);
            PrefixTraverseRec(root.right);
        }
    }

    // Постфиксный обход (левый потомок, правый потомок, корень)
    public void PostfixTraverse()
    {
        PostfixTraverseRec(root);
        Console.WriteLine();
    }

    // Рекурсивный метод для постфиксного обхода
    private void PostfixTraverseRec(Node root)
    {
        if (root != null)
        {
            PostfixTraverseRec(root.left);
            PostfixTraverseRec(root.right);
            Console.Write(root.data + " ");
        }
    }
}

public class QuickSort
{
    private int[] arr; 

    public QuickSort()
    {
        arr = new int[0]; 
    }

    public void Insert(int item)
    {
        Array.Resize(ref arr, arr.Length + 1);
        arr[arr.Length - 1] = item;
    }

    public bool Find(int key)
    {
        return Array.IndexOf(arr, key) != -1;
    }

    public void Remove(int item)
    {
        int index = Array.IndexOf(arr, item);

        if (index != -1)
        {
            for (int i = index; i < arr.Length - 1; i++)
            {
                arr[i] = arr[i + 1];
            }
            Array.Resize(ref arr, arr.Length - 1);
        }
    }

    public void Print()
    {
        foreach (int item in arr)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }

    public void QuickSortAlgorithm()
    {
        QuickSortRecursion(0, arr.Length - 1);
    }

    private void QuickSortRecursion(int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(low, high);
            QuickSortRecursion(low, partitionIndex - 1);
            QuickSortRecursion(partitionIndex + 1, high);
        }
    }

    private int Partition(int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;

                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
        int swapTemp = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = swapTemp;
        return i + 1;
    }
}

public class CustomArray
{
    private int[] array;
    private int size;

    public CustomArray(int capacity)
    {
        array = new int[capacity];
        size = 0;
    }

    public void Insert(int element)
    {
        if (size < array.Length)
        {
            array[size] = element;
            size++;
        }
        else
        {
            Console.WriteLine("Массив заполнен. Невозможно добавить больше элементов.");
        }
    }

    public void Remove(int element)
    {
        int index = Array.IndexOf(array, element);
        if (index != -1)
        {
            for (int i = index; i < size - 1; i++)
            {
                array[i] = array[i + 1];
            }
            size--;
        }
        else
        {
            Console.WriteLine("Элемент для удаления не найден в массиве.");
        }
    }

    public int FindbyNum(int element)
    {
        int index = Array.IndexOf(array, element);
        if (index != -1)
        {
            return index;
        }
        else
        {
            Console.WriteLine("Элемент не найден в массиве.");
            return -1;
        }
    }

    public void FindbyInd(int index)
    {
        if (index >= 0 && index < size)
        {
            Console.WriteLine("Элемент по индексу " + index + ": " + array[index]);
        }
        else
        {
            Console.WriteLine("В массиве нет элемента с таким индексом");
        }
    }

    public void Print()
    {
        foreach (int item in array)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Stopwatch sw = new Stopwatch();

        Console.WriteLine("AVL дерево");
        AVLTree avlTree = new AVLTree();

        avlTree.Insert(50);
        avlTree.Insert(30);
        avlTree.Insert(70);
        avlTree.Insert(20);
        avlTree.Insert(40);
        avlTree.Insert(60);

        //int keyToFind = new Random().Next(100);
        //bool FindAWLres = avlTree.Find(keyToFind);
        //Console.WriteLine($"Метод AWLTree.Find вернул: {FindAWLres}");

        sw.Start();
        avlTree.Find(90); 
        sw.Stop();
        Console.WriteLine($"Время выполнения метода AWLTree.Find: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        avlTree.Insert(80);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода AWLTree.Insert: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        avlTree.Remove(30);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода AWLTree.Remove: {sw.Elapsed}");
        sw.Reset();

        Console.WriteLine("Инфиксный обход:");
        sw.Start();
        avlTree.InfixTraverse(); 
        sw.Stop();
        Console.WriteLine($"Время выполнения метода AWLTree.InfixTraverse: {sw.Elapsed}");
        sw.Reset();

        Console.WriteLine("Префиксный обход:");
        sw.Start();
        avlTree.PrefixTraverse(); 
        sw.Stop();
        Console.WriteLine($"Время выполнения метода AWLTree.PrefixTraverse: {sw.Elapsed}");
        sw.Reset();

        Console.WriteLine("Постфиксный обход:");
        sw.Start();
        avlTree.PostfixTraverse(); 
        sw.Stop();
        Console.WriteLine($"Время выполнения метода AWLTree.PostfixTraverse: {sw.Elapsed}");
        sw.Reset();


        Console.WriteLine("\nБинарное дерево поиска");
        BinarySearchTree binarySearchTree = new BinarySearchTree();

        binarySearchTree.Insert(50);
        binarySearchTree.Insert(30);
        binarySearchTree.Insert(70);
        binarySearchTree.Insert(20);
        binarySearchTree.Insert(40);
        binarySearchTree.Insert(60);

        sw.Start();
        binarySearchTree.Find(90);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода BinarySearchTree.Find: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        binarySearchTree.Insert(80);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода binarySearchTree.Insert: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        binarySearchTree.Remove(30);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода binarySearchTree.Remove: {sw.Elapsed}");
        sw.Reset();

        Console.WriteLine("Инфиксный обход (в порядке возрастания):");
        sw.Start();
        binarySearchTree.InfixTraverse(); 
        sw.Stop();
        Console.WriteLine($"Время выполнения метода binarySearchTree.InfixTraverse: {sw.Elapsed}");
        sw.Reset();

        Console.WriteLine("Префиксный обход:");
        sw.Start();
        avlTree.PrefixTraverse(); 
        sw.Stop();
        Console.WriteLine($"Время выполнения метода binarySearchTree.PrefixTraverse: {sw.Elapsed}");
        sw.Reset();

        Console.WriteLine("Постфиксный обход:");
        sw.Start();
        binarySearchTree.PostfixTraverse(); 
        sw.Stop();
        Console.WriteLine($"Время выполнения метода binarySearchTree.PostfixTraverse: {sw.Elapsed}");
        sw.Reset();


        Console.WriteLine("\nОтсортированный QuickSort массив");
        QuickSort quickSort = new QuickSort();

        quickSort.Insert(50);
        quickSort.Insert(30);
        quickSort.Insert(70);
        quickSort.Insert(20);
        quickSort.Insert(40);
        quickSort.Insert(60);

        sw.Start();
        quickSort.Find(90);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода quickSort.Find: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        quickSort.Insert(80);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода quickSort.Insert: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        quickSort.Remove(30);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода quickSort.Remove: {sw.Elapsed}");
        sw.Reset();

        Console.WriteLine("Отсортированный QuickSort массив");
        sw.Start();
        quickSort.Print(); 
        sw.Stop();
        Console.WriteLine($"Время выполнения метода quickSort.Print: {sw.Elapsed}");
        sw.Reset();


        Console.WriteLine("\nНе отсортированный CustomArray массив");
        CustomArray сustomArray = new CustomArray(7);

        сustomArray.Insert(50);
        сustomArray.Insert(30);
        сustomArray.Insert(70);
        сustomArray.Insert(20);
        сustomArray.Insert(40);
        сustomArray.Insert(60);

        sw.Start();
        сustomArray.Insert(80);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода CustomArray.Insert: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        сustomArray.Remove(30);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода CustomArray.Remove: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        сustomArray.FindbyNum(90);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода CustomArray.FindbyNum: {sw.Elapsed}");
        sw.Reset();

        sw.Start();
        сustomArray.FindbyInd(1);
        sw.Stop();
        Console.WriteLine($"Время выполнения метода CustomArray.FindbyNum: {sw.Elapsed}");
        sw.Reset();

        Console.WriteLine("CustomArray массив:");
        sw.Start();
        сustomArray.Print();
        sw.Stop();
        Console.WriteLine($"Время выполнения метода CustomArray.Print: {sw.Elapsed}");
        sw.Reset();
    }
}
using Lab6Math;
using System.Text;
using Menu;
using System.Diagnostics;

static void fun(int[,] adjacencyMatrix, string name)
{
    var thisPath = Directory.GetCurrentDirectory() + "\\";
    string dotFilePath = name + "graph.dot";
    string imageFilePath = name + "graph.png";

    // Создаем файл .dot для описания графа
    using (var sw = new StreamWriter(dotFilePath))
    {
        sw.WriteLine("graph G {");
        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
        {          
            for (int j = i + 1; j < adjacencyMatrix.GetLength(1); j++)
            {
                if (adjacencyMatrix[i, j] > 0)
                {
                    sw.WriteLine($"{i} -- {j} [label=\"{adjacencyMatrix[i, j]}\"];");
                }
            }
        }
        sw.WriteLine("}");
    }

    // Запускаем Graphviz для создания изображения
    var startInfo = new ProcessStartInfo
    {
        FileName = "dot",
        Arguments = $"-Tpng {dotFilePath} -o {imageFilePath}",
        UseShellExecute = true
    };
    Process.Start(startInfo);
    System.Threading.Thread.Sleep(1000);
    
    // Запускаем изображение
    startInfo = new ProcessStartInfo
    {
        FileName = thisPath + imageFilePath,
        UseShellExecute = true
    };

    Process.Start(startInfo);

    Console.WriteLine($"Граф создан и сохранен как {imageFilePath}");
}

static string[] GetNameTxtFiles()
{
    var folderPath = Directory.GetCurrentDirectory();

    string[] files = Directory.GetFiles(folderPath, "*.txt");

    for (int i = 0; i < files.Length; i++)
    {
        files[i] = Path.GetFileName(files[i]);
    }

    return files;
}

static int[,] ReadMatrixFromFile(string filename)
{
    string[] lines = File.ReadAllLines(filename);
    int rows = lines.Length;
    int columns = lines[0].Split(' ').Length;
    int[,] matrix = new int[rows, columns];

    for (int i = 0; i < rows; i++)
    {
        string[] values = lines[i].Split(' ');

        for (int j = 0; j < columns; j++)
        {
            matrix[i, j] = int.Parse(values[j]);
        }
    }

    return matrix;
}

static int[,]? GetMatrixFromFolder()
{
    var files = GetNameTxtFiles();
    var folderPath = Directory.GetCurrentDirectory();

    int[,]? result = null;

    var dialog = new Dialog("Выберите файлы из данной папки");
    foreach (var file in files)
    {
        var fullPath = Path.Combine(folderPath, file);
        dialog.AddOption(file, () => { result = ReadMatrixFromFile(fullPath); dialog.Close(); });
    }
    dialog.AddOption("Выход", () => dialog.Close());

    dialog.Start();

    return result;

}

static void PrintResult(List<Edge> minSpanningTree, string message = "\tМинимальный остов: ")
{
    Console.WriteLine(message);

    var result = 0;
    foreach (var edge in minSpanningTree)
    {
        Console.WriteLine($"{edge.Source} - {edge.Destination}, вес: {edge.Weight}");
        result += edge.Weight;
    }

    Console.WriteLine("\nВес минимального остова: " + result);
}

static void PrintKruskalAlgorithm(int[,] matrix)
{
    List<Edge> minSpanningTree = Kruskal.KruskalMST(matrix);
    PrintResult(minSpanningTree, "\n\tМинимальный остов по алгоритму Краскала:"); 
}

static void PrintPrimaAlgorithm(int[, ] matrix)
{
    var minSpanningTree = Prima.PrimMST(matrix);
    PrintResult(minSpanningTree, "\n\tМинимальный остов по алгоритму Прима:");
}

static void Print(int[,]? matrix)
{
    if(matrix == null) 
        return;
    
    Console.WriteLine(GetString(matrix));
    fun(matrix, "GRAPH.txt");
    PrintPrimaAlgorithm(matrix);
    PrintKruskalAlgorithm(matrix);
}

static string GetString(int[,] matrix)
{
    var stringBuilder = new StringBuilder();
    int width = matrix.GetLength(1);
    int height = matrix.GetLength(0);
    stringBuilder.AppendLine("┌" + new string('─', width * 4 - 1) + "┐");
    for (int i = 0; i < height; i++)
    {
        stringBuilder.Append("│ ");
        for (int j = 0; j < width; j++)
        {
            if (matrix[j, i] == 0)
                stringBuilder.Append($"X │ ");
            else
                stringBuilder.Append($"{matrix[i, j]} │ ");
        }
        stringBuilder.AppendLine();

        if (i == height - 1)
        {
            stringBuilder.AppendLine("└" + new string('─', width * 4 - 1) + "┘");
        }
        else
        {
            stringBuilder.AppendLine("├" + new string('─', width * 4 - 1) + "┤");
        }
    }
    return stringBuilder.ToString();
}


int[,]? matrix = null;

var dialog = new Dialog("Алгоритмы Краскаля и Прима");
dialog.AddOption("Создание матрицы смежности", () => matrix = GetMatrixFromFolder(), true);
dialog.AddOption("Вывод алгоритмов", () => Print(matrix));
dialog.Start();



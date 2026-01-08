using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    private static List<Task> tasks = new List<Task>();
    private const string FilePath = "tasks.json";

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // Для корректного отображения русского текста
        LoadTasks();

        while (true)
        {
            Console.Clear();
            DisplayTasks();
            DisplayMenu();

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    CompleteTask();
                    break;
                case "3":
                    DeleteTask();
                    break;
                case "4":
                    SaveTasks();
                    Console.WriteLine("Данные сохранены. До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void DisplayTasks()
    {
        Console.WriteLine("=== ВАШ ЕЖЕДНЕВНИК ===\n");
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст.\n");
        }
        else
        {
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
            Console.WriteLine();
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("Меню:");
        Console.WriteLine("1. Добавить задачу");
        Console.WriteLine("2. Отметить задачу как выполненную");
        Console.WriteLine("3. Удалить задачу");
        Console.WriteLine("4. Выйти и сохранить");
        Console.Write("\nВыберите действие: ");
    }

    static void AddTask()
    {
        Console.Clear();
        Console.WriteLine("=== Добавление новой задачи ===");
        Console.Write("Введите описание задачи: ");
        string? description = Console.ReadLine().Trim();

        if (!string.IsNullOrEmpty(description))
        {
            int newId = tasks.Count > 0 ? tasks[^1].Id + 1 : 1;
            var task = new Task
            {
                Id = newId,
                Description = description,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };
            tasks.Add(task);
            Console.WriteLine("Задача успешно добавлена!");
        }
        else
        {
            Console.WriteLine("Описание не может быть пустым!");
        }

        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void CompleteTask()
    {
        Console.Clear();
        DisplayTasks();
        Console.Write("Введите номер задачи для отметки как выполненной: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = true;
                Console.WriteLine("Задача отмечена как выполненная!");
            }
            else
            {
                Console.WriteLine("Задача с таким номером не найдена.");
            }
        }
        else
        {
            Console.WriteLine("Неверный формат номера.");
        }

        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }

    static void DeleteTask()
    {
        Console.Clear();
        DisplayTasks();
        Console.Write("Введите номер задачи для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);
                // Переприсваиваем ID, чтобы не было пропусков
                for (int i = 0; i < tasks.Count; i++)
                {
                    tasks[i].Id = i + 1;
                }
                Console.WriteLine("Задача удалена!");
            }
            else
            {
                Console.WriteLine("Задача не найдена.");
            }
        }
        else
        {
            Console.WriteLine("Неверный формат.");
        }

        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }

    static void LoadTasks()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                tasks = JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                tasks = new List<Task>();
            }
        }
    }

    static void SaveTasks()
    {
        try
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }
}
// Класс для представления одной задачи
public class Task
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }

    public override string ToString()
    {
        string status = IsCompleted ? "[✓]" : "[ ]";
        return $"{Id}. {status} {Description} (создана: {CreatedAt:dd.MM.yyyy HH:mm})";
    }
}

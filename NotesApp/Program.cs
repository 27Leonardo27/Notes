using System.Diagnostics;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Security.Authentication;
using System.Reflection;
using System.Xml.Serialization;

namespace NotesApp;

internal class Program
{
    public static string userName;
    public static string pathUser;
    public static List<string> note;

    static void Main(string[] args)
    {
        bool isAuthentication = false;
        do
        {
            Console.Write("Введите своё имя: ");
            userName = Console.ReadLine();
            isAuthentication = Authentication(userName);
        }
        while (!isAuthentication);


        while (isAuthentication)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========  Меню  ========");
            Console.WriteLine("1. Создать заметку");
            Console.WriteLine("2. Удалить заметку");
            Console.WriteLine("3. Просмотреть заметки");
            Console.WriteLine("4. Выйти");
            Console.Write("\nВыберите действие:");
            Console.ForegroundColor = ConsoleColor.White;

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddNote();
                    break;

                case "2":
                    ShowNotes();
                    if (note.Count > 0)
                    {
                        DeleteNote();
                    }
                    break;

                case "3":
                    ShowNotes();
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Некорректный выбор, попробуйте снова!");
                    break;

            }
        }

        static bool Authentication(string userName)   //пустого имени не должно быть
        {
            pathUser = $"{userName}_notes.txt";

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Имя не может быть пустым!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }


            if (!File.Exists(pathUser))
            {
                Console.WriteLine("Профиль создан!");
                note = new List<string>();
            }
            else
            {
                Console.WriteLine("Вы зашли в профиль!");
            }
            return true;
        }

        static void AddNote()   //исключение на пустую заметку
        {
            Console.Write("Введите текст заметки: ");
            var _note = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(_note))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Поле не может быть пустым!");
                Console.ForegroundColor = ConsoleColor.White;
                AddNote();
            }
            else
            {
                note.Add(_note);
                File.WriteAllLines(pathUser, note);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Заметка создана!\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void DeleteNote()     //корректный номер заметки для делита
        {
            int _choice;
            Console.Write("Введите номер заметки которую хотите удалить: ");
            int.TryParse(Console.ReadLine(), out _choice);

            if (_choice > 0 && _choice <= note.Count)
            {
                note.RemoveAt(_choice - 1);
                File.WriteAllLines(pathUser, note);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nЗаметка удалена!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введите корректный номер заметки!");
                Console.ForegroundColor = ConsoleColor.White;
                ShowNotes();
                DeleteNote();
                return;
            }
        }

        static void ShowNotes()
        {
            if (!File.Exists(pathUser))
            {
                Console.WriteLine("Заметки не найдены!");
                return;
            }

            note = File.ReadAllLines(pathUser).ToList();

            if (note.Count == 0)
            {
                Console.WriteLine("Заметки не найдены!");
                return;
            }

            Console.WriteLine("\tВаши заметки:");

            for (var i = 0; i < note.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {note[i]}");
            }
        }
    }
}

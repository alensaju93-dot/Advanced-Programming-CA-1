using System;
using System.Collections.Generic;
using System.Globalization;

namespace ContactRegistryApp
{
    class Entry
    {
        public int Id { get; set; }
        public string Given { get; set; }
        public string Surname { get; set; }
        public string Organisation { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }

        public string Label => $"{Given} {Surname}";

        public override string ToString()
        {
            return $"ID        : {Id}\n" +
                   $"Name      : {Label}\n" +
                   $"Company   : {Organisation}\n" +
                   $"Phone     : {Phone}\n" +
                   $"Email     : {Email}\n" +
                   $"DOB       : {Dob:dd-MM-yyyy}";
        }
    }

    class Registry
    {
        private Dictionary<int, Entry> records = new Dictionary<int, Entry>();
        private int nextId = 1;

        public Registry()
        {
            LoadInitialData();
        }

        public void Insert(Entry e)
        {
            e.Id = nextId++;
            records[e.Id] = e;
        }

        public Entry Fetch(int id)
        {
            return records.ContainsKey(id) ? records[id] : null;
        }

        public Entry FindByPhone(string phone)
        {
            foreach (var r in records.Values)
                if (r.Phone == phone)
                    return r;
            return null;
        }

        public bool Remove(int id)
        {
            return records.Remove(id);
        }

        public void ShowSummary()
        {
            if (records.Count == 0)
            {
                Console.WriteLine("No entries available.");
                return;
            }

            Console.WriteLine("ID | Name                  | Phone     | Email");
            Console.WriteLine("----------------------------------------------------");

            foreach (var r in records.Values)
            {
                Console.WriteLine($"{r.Id,2} | {r.Label.PadRight(20)} | {r.Phone,-9} | {r.Email}");
            }
        }

        private void LoadInitialData()
        {
            string[] names = { "Adam", "Beth", "Connor", "Daisy", "Evan", "Fiona", "George", "Holly", "Ian", "Julia" };

            for (int i = 0; i < names.Length; i++)
            {
                Insert(new Entry
                {
                    Given = names[i],
                    Surname = $"User{i + 1}",
                    Organisation = "SampleOrg",
                    Phone = "89" + (5000000 + i),
                    Email = $"{names[i].ToLower()}@sample.com",
                    Dob = DateTime.Now.AddYears(-22).AddDays(i * 15)
                });
            }

            while (records.Count < 20)
            {
                Insert(new Entry
                {
                    Given = "Auto",
                    Surname = $"Gen{records.Count + 1}",
                    Organisation = "AutoCorp",
                    Phone = "88" + (6000000 + records.Count),
                    Email = $"auto{records.Count + 1}@mail.com",
                    Dob = DateTime.Now.AddYears(-30)
                });
            }
        }
    }

    class Program
    {
        static Registry book = new Registry();

        static void Main()
        {
            Console.WriteLine("=== Contact Registry System ===");

            while (true)
            {
                Console.WriteLine("\n1) Add");
                Console.WriteLine("2) List");
                Console.WriteLine("3) View");
                Console.WriteLine("4) Delete");
                Console.WriteLine("0) Exit");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1": AddFlow(); break;
                    case "2": book.ShowSummary(); break;
                    case "3": ViewFlow(); break;
                    case "4": DeleteFlow(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        static void AddFlow()
        {
            Console.Write("First name: ");
            var g = Console.ReadLine();

            Console.Write("Last name: ");
            var s = Console.ReadLine();

            Console.Write("Organisation: ");
            var o = Console.ReadLine();

            string p;
            while (true)
            {
                Console.Write("Phone (9 digits): ");
                p = Console.ReadLine();
                if (IsPhoneValid(p)) break;
                Console.WriteLine("Phone must be exactly 9 digits.");
            }

            Console.Write("Email: ");
            var e = Console.ReadLine();

            DateTime d;
            while (true)
            {
                Console.Write("DOB (dd-MM-yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                    break;
                Console.WriteLine("Invalid date format.");
            }

            book.Insert(new Entry
            {
                Given = g,
                Surname = s,
                Organisation = o,
                Phone = p,
                Email = e,
                Dob = d
            });

            Console.WriteLine("Contact added.");
        }

        static void ViewFlow()
        {
            Console.Write("Enter ID or phone: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int id))
            {
                var r = book.Fetch(id);
                Console.WriteLine(r == null ? "Not found." : r);
            }
            else
            {
                var r = book.FindByPhone(input);
                Console.WriteLine(r == null ? "Not found." : r);
            }
        }

        static void DeleteFlow()
        {
            Console.Write("Enter ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.WriteLine(book.Remove(id) ? "Deleted." : "ID not found.");
        }

        static bool IsPhoneValid(string p)
        {
            if (string.IsNullOrWhiteSpace(p) || p.Length != 9) return false;
            foreach (char c in p)
                if (!char.IsDigit(c)) return false;
            return true;
        }
    }
}
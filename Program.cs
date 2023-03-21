using System.ComponentModel;

namespace dtp6_contacts
{
    class MainClass
    {
        public static List<Person> contactList = new List<Person>();
        public class Person
        {
            private string persname, surname, birthdate;
            private List<string> phone = new List<string>();
            private List<string> address = new List<string>();
            public string Persname { get { return persname; } set { persname = value; } }
            public string Surname { get { return surname; } set { surname = value; } }
            public List<string> Phone { get { return phone; } set { phone = value; } }
            public List<string> Address { get { return address; } set { address = value; } }
            public string Birthdate { get { return birthdate; } set { birthdate = value; } }
        }
        public static void Main(string[] args)
        {
            string lastFileName = "address.lis";
            string[] commandLine;
            Welcome();
            do
            {
                Console.Write($"> ");
                commandLine = Console.ReadLine().Split(' ');
                if (commandLine[0] == "quit")
                {
                    Console.WriteLine("Thank you and good day!");
                }
                else if (commandLine[0] == "load")
                {
                    if (commandLine.Length < 2)
                    {
                        lastFileName = load();
                    }
                    else
                    {
                        lastFileName = loadFile(commandLine);
                    }
                }
                else if (commandLine[0] == "save")
                {
                    if (commandLine.Length < 2)
                    {
                        save(lastFileName);
                    }
                    else
                    {
                        // NYI!
                        Console.WriteLine("Not yet implemented: save /file/");
                    }
                }
                else if (commandLine[0] == "new")
                {
                    newEntry(commandLine);
                }
                else if (commandLine[0] == "help")
                {
                    help();
                }
                else if (commandLine[0] == "delete")
                {
                    delete(commandLine);
                }
                else if (commandLine[0] == "edit")
                {
                    Edit(commandLine);
                }
                else if (commandLine[0] == "list")
                {
                    printList(commandLine);
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{commandLine[0]}'");
                }
            } while (commandLine[0] != "quit");
        }

        private static void delete(string[] commandLine)
        {
            if (commandLine.Length < 2)
            {
                contactList.Clear();
            }
            else
            {
                string person = commandLine[1];
                foreach (Person p in contactList)
                {
                    if (string.Equals(p.Persname, person, StringComparison.OrdinalIgnoreCase))
                    {
                        contactList.Remove(p);
                        break;
                    }
                }
            }
        }

        private static void Edit(string[] commandLine)
        {
            string person = commandLine[1];
            foreach (Person p in contactList)
            {
                if (string.Equals(p.Persname, person, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("You need to enter all info on this person again to edit the post.");
                    Console.Write("Updated personal name: ");
                    p.Persname = Console.ReadLine();
                    Console.Write("Updated surname: ");
                    p.Surname = Console.ReadLine();
                    Console.Write("Updated phones, if more than one separate them with a ',': ");
                    p.Phone = Console.ReadLine().Split(',').ToList();
                    Console.Write("Updated address, if more than one seperate them with a ',': ");
                    p.Address = Console.ReadLine().Split(',').ToList();
                    Console.Write("Updated birthdate: ");
                    p.Birthdate = Console.ReadLine();
                    contactList.Add(p);
                    break;
                }

            }
        }

        private static void printList(string[] commandLine)
        {
            if (commandLine.Length < 2)
            {
                foreach (Person p in contactList)
                {
                    string phone = string.Join(";", p.Phone);
                    string address = string.Join(";", p.Address);
                    Console.WriteLine($"{p.Persname}|{p.Surname}|{phone}|{address}|{p.Birthdate}");
                }
            }
            else
            {
                string person = commandLine[1];
                foreach (Person p in contactList)
                {
                    if (string.Equals(p.Persname, person, StringComparison.OrdinalIgnoreCase))
                    {
                        string phone = string.Join(";", p.Phone);
                        string address = string.Join(";", p.Address);
                        Console.WriteLine($"{p.Persname}|{p.Surname}|{phone}|{address}|{p.Birthdate}");
                    }
                }
            }
        }

        private static void newEntry(string[] commandLine)
        {
            Person ny = new();
            if (commandLine.Length < 2)
            {
                Console.Write("Personal name: ");
                ny.Persname = Console.ReadLine();
                Console.Write("Surname: ");
                ny.Surname = Console.ReadLine();
                Console.Write("Phone, if more than one separate them with a ',': ");
                ny.Phone = Console.ReadLine().Split(',').ToList();
                Console.Write("Address, if more than one seperate them with a ',': ");
                ny.Address = Console.ReadLine().Split(',').ToList();
                Console.Write("Birthdate: ");
                ny.Birthdate = Console.ReadLine();
                contactList.Add(ny);
            }
            else
            {
                ny.Persname = commandLine[1];
                ny.Surname = commandLine[2];
                contactList.Add(ny);
            }
        }
        private static void help()
        {
            Console.WriteLine("Avaliable commands: \n" +
                "  delete       - emtpy the contact list\n" +
                "  delete /persname/ /surname/ - delete a person\n" +
                "  load        - load contact list data from the file address.lis\n" +
                "  load /file/ - load contact list data from the file\n" +
                "  new        - create new person\n" +
                "  new /persname/ /surname/ - create new person with personal name and surname\n" +
                "  quit        - quit the program\n" +
                "  save         - save contact list data to the file previously loaded\n" +
                "  save /file/ - save contact list data to the file\n");
        }
        private static void save(string lastFileName)
        {
            using (StreamWriter outfile = new StreamWriter(lastFileName))
            {
                foreach (Person p in contactList)
                {
                    string phone = string.Join(";", p.Phone);
                    string address = string.Join(";", p.Address);
                    outfile.WriteLine($"{p.Persname}|{p.Surname}|{phone}|{address}|{p.Birthdate}");
                }
            }
        }
        private static string loadFile(string[] commandLine)
        {
            string lastFileName = commandLine[1];
            using (StreamReader infile = new StreamReader(lastFileName))
            {
                string line;
                while ((line = infile.ReadLine()) != null)
                {
                    List<string> attrs = line.Split('|').ToList();
                    Person p = new Person();
                    p.Persname = attrs[0];
                    p.Surname = attrs[1];
                    p.Phone = attrs[2].Split(';').ToList();             //Konvertera till lista
                    p.Address = attrs[3].Split(';').ToList();           //Konvertera till lista
                    p.Birthdate = attrs[4];
                    contactList.Add(p);
                }
            }

            return lastFileName;
        }
        private static string load()
        {
            string lastFileName = "address.lis";
            using (StreamReader infile = new StreamReader(lastFileName))
            {
                string line;
                while ((line = infile.ReadLine()) != null)
                {
                    List<string> attrs = line.Split('|').ToList();
                    Person p = new Person();
                    p.Persname = attrs[0];
                    p.Surname = attrs[1];
                    p.Phone = attrs[2].Split(';').ToList();
                    p.Address = attrs[3].Split(';').ToList();
                    p.Birthdate = attrs[4];
                    contactList.Add(p);
                }
            }
            return lastFileName;
        }
        private static void Welcome()
        {
            Console.WriteLine("Hello and welcome to the contact list\n" +
                "Avaliable commands: \n" +
                "  load        - load contact list data from the file address.lis\n" +
                "  load /file/ - load contact list data from the file\n" +
                "  new        - create new person\n" +
                "  new /persname/ /surname/ - create new person with personal name and surname\n" +
                "  quit        - quit the program\n" +
                "  save         - save contact list data to the file previously loaded\n" +
                "  save /file/ - save contact list data to the file\n");
        }
    }
}

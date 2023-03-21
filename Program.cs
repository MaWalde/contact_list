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
                    // NYI!
                    Console.WriteLine("Not yet implemented: safe quit");
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
                else
                {
                    Console.WriteLine($"Unknown command: '{commandLine[0]}'");
                }
            } while (commandLine[0] != "quit");
        }

        private static void newEntry(string[] commandLine)
        {
            if (commandLine.Length < 2)
            {
                Console.Write("personal name: ");
                string persname = Console.ReadLine();
                Console.Write("surname: ");
                string surname = Console.ReadLine();
                Console.Write("phone, if more than one separate them with ',': ");
                string[] phone = Console.ReadLine().Split(',');
            }
            else
            {
                // NYI!
                Console.WriteLine("Not yet implemented: new /person/");
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

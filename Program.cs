﻿using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;

namespace dtp7_contact_list
{
    class MainClass
    {
        static List<Person> contactList = new List<Person>();
        class Person
        {
            public string persname, surname; public int birthdate;
            public List<string> phone;
            public List<string> address;
            public Person() { }
            public Person(string persname, string surname)
            {
                this.persname = persname; this.surname = surname;
            }
            public List<string> Phone
            {
                get { return phone; }
                set { phone = value; }
            }
            public List<string> Address
            {
                get { return address; }
                set { address = value; }
            }
            public string PhoneList
            {
                get { return String.Join(";", phone); }
                private set { }
            }
            public string AddressList
            {
                get { return String.Join(";", address); }
                private set { }
            }
            public int Birthdate
            {
                get { return birthdate; }
                set { birthdate = value; }
            }
            public void Print()
            {
                string phoneList = String.Join(", ", phone);
                string addressList = String.Join(", ", address);
                Console.WriteLine($"{persname} {surname}; {phoneList}; {addressList}; {birthdate}");
            }
        }
        public static void Main(string[] args)
        {
            string lastFileName = GetUserDirectory("address.lis");
            string[] commandLine;
            PrintHelpMessage();
            do
            {
                Console.Write($"> ");
                commandLine = Console.ReadLine().Split(' ');
                if (commandLine[0] == "quit")
                {
                    Console.WriteLine("Goodbye!");
                }
                // NYI: IMPORTANT
                else if (commandLine[0] == "delete")
                {
                    if (commandLine.Length == 1)
                    {
                        contactList.Clear();
                    }
                    else if (commandLine.Length == 3)
                    {
                        //TODO: Lägg till utskrift om personen inte finns
                        DeleteAllPersons(commandLine[1], commandLine[2]);
                    }
                    else
                    {
                        Console.WriteLine("Usage:");
                        Console.WriteLine("  delete                      - empty the contact list");
                        Console.WriteLine("  delete /persname/ /surname/ - delete a person");
                    }
                }
                else if (commandLine[0] == "list")
                {
                    if (commandLine.Length == 1)
                    {
                        ListContactList();
                    }
                    else
                    {
                        Console.WriteLine("Usage:");
                        Console.WriteLine("  list                        - list the contact list");
                    }
                }
                else if (commandLine[0] == "load")
                {
                    if (commandLine.Length == 1)
                    {
                        lastFileName = GetUserDirectory("address.lis");
                        try
                        {
                            LoadContactListFromFile(lastFileName); //TODO: Fixa utskrift för antal tilllagda rader
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("The file {0} cannot be found", lastFileName);
                        }
                    }
                    else if (commandLine.Length == 2)
                    {
                        lastFileName = GetUserDirectory(commandLine[1]); // commandLine[1] is the first argument
                        // FIXME: Throws System.IO.FileNotFoundException: 
                        LoadContactListFromFile(lastFileName);
                    }
                    else
                    {
                        Console.WriteLine("Usage:");
                        Console.WriteLine("  load                        - load contact list data from the file address.lis");
                        Console.WriteLine("  load /file/                 - load contact list data from the file");
                    }
                }
                else if (commandLine[0] == "save")
                {
                    if (commandLine.Length == 1)
                    {
                        SaveContactListToFile(lastFileName);
                    }
                    else if (commandLine.Length == 2)
                    {
                        SaveContactListToFile(commandLine[1]); // commandLine[1] is the first argument
                    }
                    else
                    {
                        Console.WriteLine("Usage:");
                        Console.WriteLine("  save                        - save contact list data to the file previously loaded");
                        Console.WriteLine("  save /file/                 - save contact list data to the file");
                    }
                }
                else if (commandLine[0] == "new")
                {
                    if (commandLine.Length == 1)
                    {
                        Console.Write("personal name: ");
                        string persname = Console.ReadLine();
                        Console.Write("surname: ");
                        string surname = Console.ReadLine();
                        AddAndSetupNewPerson(persname, surname);
                    }
                    else if (commandLine.Length == 3)
                    {
                        AddAndSetupNewPerson(commandLine[1], commandLine[2]);
                    }
                    else
                    {
                        Console.WriteLine("Usage:");
                        Console.WriteLine("  new                         - create new person");
                        Console.WriteLine("  new /persname/ /surname/    - create new person with personal name and surname");
                    }
                }
                else if (commandLine[0] == "help")
                {
                    PrintHelpMessage();
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{commandLine[0]}'");
                }
            } while (commandLine[0] != "quit");
        }

        private static void DeleteAllPersons(string persname, string surname)
        {
            int found;
            do
            {
                found = -1;
                for (int i = 0; i < contactList.Count; i++)
                {
                    if (contactList[i].persname == persname && contactList[i].surname == surname)
                    {
                        found = i; break; // breaks the for loop
                    }
                }
                if (found == -1)
                {
                    Console.WriteLine($"{persname} {surname} does not exist");
                    break; // breaks the do loop
                }
                contactList.RemoveAt(found);
            } while (true);
        }

        private static void ListContactList()
        {
            foreach (Person p in contactList)
            {
                if (p != null)
                    p.Print();
            }
        }

        private static void AddAndSetupNewPerson(string persname, string surname)
        {
            Person newPerson = new Person(persname, surname);
            Console.WriteLine("Add multiple phones, separate numbers with ',':");
            Console.Write("  phone: ");
            List<string> phone = Console.ReadLine().Split(',').ToList();
            newPerson.Phone = phone;
            Console.WriteLine("Add multiple addresses, separate addresses with ',':");
            Console.Write("  address: ");
            List<string> address = Console.ReadLine().Split(',').ToList();
            newPerson.Address = address;
            Console.Write("birth date in numbers: ");
            try
            {
                int birthdate = Convert.ToInt32(Console.ReadLine());
                newPerson.Birthdate = birthdate;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Birth date must be in numbers!");
            }
            contactList.Add(newPerson);
        }

        private static string GetUserDirectory(string path)
        {
            return $"{System.Environment.GetEnvironmentVariable("USERPROFILE")}\\{path}";
        }

        private static void SaveContactListToFile(string lastFileName)
        {
            using (StreamWriter outfile = new StreamWriter(lastFileName))
            {
                foreach (Person p in contactList)
                {
                    if (p != null)
                        outfile.WriteLine($"{p.persname}|{p.surname}|{p.PhoneList}|{p.AddressList}|{p.birthdate}");
                }
            }
        }

        private static void LoadContactListFromFile(string lastFileName)
        {
            try
            {
                using (StreamReader infile = new StreamReader(lastFileName))

                {
                    int count = 0;
                    string line;
                    while ((line = infile.ReadLine()) != null)
                    {
                        LoadContact(line); // Also prints the line loaded
                        count++;
                    }
                    Console.WriteLine($"{count} entrys added");
                }
            }
            catch (FileNotFoundException) { Console.WriteLine($"The file {lastFileName} cannot be found"); }
        }

        private static void LoadContact(string lineFromAddressFile)
        {
            string[] attrs = lineFromAddressFile.Split('|');
            Person newPerson = new Person();
            newPerson.persname = attrs[0];
            newPerson.surname = attrs[1];
            newPerson.phone = new List<string>(attrs[2].Split(';'));
            newPerson.address = new List<string>(attrs[3].Split(';'));
            newPerson.birthdate = Convert.ToInt32(attrs[4]);
            contactList.Add(newPerson);
        }
        private static void PrintHelpMessage()
        {
            Console.WriteLine("Avaliable commands: ");
            Console.WriteLine("  delete                      - empty the contact list");
            Console.WriteLine("  delete /persname/ /surname/ - delete a person");
            Console.WriteLine("  list                        - list the contact list");
            Console.WriteLine("  load                        - load contact list data from the file address.lis");
            Console.WriteLine("  load /file/                 - load contact list data from the file");
            Console.WriteLine("  new                         - create new person");
            Console.WriteLine("  new /persname/ /surname/    - create new person with personal name and surname");
            Console.WriteLine("  quit                        - quit the program");
            Console.WriteLine("  save                        - save contact list data to the file previously loaded");
            Console.WriteLine("  save /file/                 - save contact list data to the file");
            Console.WriteLine();
        }
    }
}
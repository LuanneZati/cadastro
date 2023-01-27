using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CadaastroProjeto
{
    class Program
    {
        static string path = "C:\\Users\\Luanne\\Documents\\C#\\notations\\data.txt";
        static string email = "Email: ";
        static string pass = "Pass: ";
        static string delimiterStart;
        static string delimiterEnd;
        static string tagEmail;
        static string tagPass;
        public struct dataRegistration
        {
            public string email;
            public string pass;
        }
        //Get user data, put in list and store in text file
        public static void RecordUser(ref List<dataRegistration> UserList)
        {
            dataRegistration UserRecord;
            UserRecord = new dataRegistration();
            UserRecord.email = "";
            UserRecord.pass = "";
            Console.Clear();
            Console.Write(email);
            UserRecord.email = Console.ReadLine();

            Console.Write(pass);
            while (true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else
                {
                    UserRecord.pass += tecla.KeyChar;
                }
            }
            UserList.Add(UserRecord);
            Console.WriteLine("\nSucess!");
            StoreData(UserList);
            Console.ReadKey();
        }
        //Store data in text file
        static void StoreData(List<dataRegistration> UserList)
        {
            bool exist = File.Exists(path);
            try
            {
                string contentFile = "";
                foreach (dataRegistration item in UserList)
                {
                    contentFile += delimiterStart + "\n";
                    contentFile += tagEmail + item.email + "\n";
                    contentFile += tagPass + item.pass + "\n";
                    contentFile += delimiterEnd + "\n";
                }
                File.WriteAllText(path, contentFile);
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: " + e.Message);
            } 
        }
        //when starting the program every data in text file are load to list
        static void LoadData(List<dataRegistration> UserList)
        {
            try
            {
                if (File.Exists(path))
                {
                    string[] contentFile = File.ReadAllLines(path);
                    dataRegistration dataReg;
                    dataReg.email = "";
                    dataReg.pass = "";
                    foreach (string line in contentFile)
                    {
                        if (line.Contains(delimiterStart))
                            continue;
                        if (line.Contains(delimiterEnd))
                            UserList.Add(dataReg);
                        if (line.Contains(tagEmail))
                            dataReg.email = line.Replace(tagEmail, "");
                        if (line.Contains(tagPass))
                            dataReg.pass = line.Replace(tagPass, "");
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
            }
        }
        //Show data that is in text file
        static void ShowData()
        {
            Console.Clear();
            try
            {
                if(File.Exists(path))
                {
                    Console.WriteLine(File.ReadAllText(path));
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: " + e.Message);
            }
            Console.ReadKey();
        }
        //Update text file data
        static void UploadData(List<dataRegistration> UserList)
        {
            try
            {
                FileStream FS = File.Create(path);
                FS.Close();
                StreamWriter SW = new StreamWriter(path);
                string contentFile = "";
                foreach (dataRegistration item in UserList)
                {
                    contentFile += delimiterStart + "\n";
                    contentFile += tagEmail + item.email + "\n";
                    contentFile += tagPass + item.pass + "\n";
                    contentFile += delimiterEnd + "\n";
                }
                SW.WriteLine(contentFile);
                SW.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: " + e.Message);
            }
        }
        //Delete user-supplied record
        static void DeleteData(ref List<dataRegistration> UserList)
        {
            int count = 0;
            Console.Clear();
            try
            {
                dataRegistration aux = new dataRegistration();
                Console.Write("Enter the email you want to delete\n- if you digit \"all\" every records will be deleted\n= ");
                aux.email = Console.ReadLine();

                if(aux.email.ToUpper() == "ALL")
                {
                    for (int i = UserList.Count - 1; i >= 0; i--)
                    {
                        UserList.RemoveAt(i);    
                    }
                    Console.WriteLine("Every Records have been deleted!");
                    File.Delete(path);
                    UploadData(UserList);
                }
                else
                {
                    for(int i=UserList.Count-1; i>=0; i--)
                    {
                        if (UserList[i].email.ToUpper() == aux.email.ToUpper())
                        {
                            UserList.RemoveAt(i);
                            count++;
                        }
                    }
                    if(count == 0)
                    {
                        Console.WriteLine("Record not found!");
                    }
                    else
                    {
                        Console.WriteLine("Deleted!");
                        File.Delete(path);
                        UploadData(UserList);
                        count = 0;
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
        static void Main(string[] args)
        {
            List<dataRegistration> UserList = new List<dataRegistration>();
            int option;
            delimiterStart = "##### START #####";
            delimiterEnd = "##### END #####";
            tagEmail = "EMAIL: ";
            tagPass = "PASS: ";
            //Load data's in text file
            LoadData(UserList);

            do
            {
                Console.Clear();
                Console.WriteLine("1- Records\n2- Show Records\n3- Delete Records\n4- Exit");
                Console.Write("Your opção: ");
                option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        RecordUser(ref UserList);
                        break;
                    case 2:
                        ShowData();
                        break;
                    case 3:
                        DeleteData(ref UserList);
                        break;
                    case 4:
                        Console.WriteLine("Bye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option..");
                        Console.ReadKey();
                        break;
                }

            } while (option != 4);
        }
    }
}
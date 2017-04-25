using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodreadsConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage is: GoodreadsConsoleApplication <Input File> <Output File>");
                Environment.Exit(0);
            }

            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(args[0]);
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter(args[1]);

            String readLine = "";
            String writeLine = "";
            String readAuthor = "";
            String readTitle = "";
            String readStandard = "";

            readLine = "Title,Author,ISBN";
            WriteFile(sw, readLine);

            try
            {
                //Read the first line of text
                readLine = sr.ReadLine();

                //Continue to read until you reach end of file
                while (readLine != null)
                {
                    //write the line to console window
                    //Console.WriteLine(line);
                    //Read the next line
                    readLine = sr.ReadLine();

                    if (readLine == null)
                        break;

                    if (readLine.Contains("TITLE"))
                    {
                        int index = readLine.IndexOf("TITLE");
                        readTitle = readLine.Substring(index + 5).Trim();

                        index = readTitle.IndexOf("/");

                        if (index > 0)
                        {
                            int indexEnd = readTitle.Length;
                            readTitle = readTitle.Remove(index, indexEnd - index);                            
                        }

                        readTitle = readTitle.Replace(".", "");
                        readTitle = readTitle.Replace(",", "");

                        readTitle = readTitle + ",";

                        readTitle = RemoveBrackets(readTitle);
                    }

                    if (readLine.Contains("AUTHOR"))
                    {
                        int index = readLine.IndexOf("AUTHOR");                        
                        readAuthor = readLine.Substring(index + 6).Trim();
                        readAuthor = readAuthor.Replace(",", "");
                        readAuthor = readAuthor.Replace(".", "");
                        readAuthor = readAuthor + ",";

                        readAuthor = RemoveBrackets(readAuthor);
                    }

                    if (readLine.Contains("STANDARD"))
                    {
                        int index = readLine.IndexOf("STANDARD");
                        readStandard = readStandard + readLine.Substring(index + 10).Trim();

                        readStandard = readStandard.Replace(".", "");
                        readStandard = readStandard.Replace(":", "");
                        readStandard = readStandard.Replace(";", "");

                        readStandard = RemoveBrackets(readStandard);
                        readStandard = RemoveBrackets(readStandard);

                        writeLine = readTitle + readAuthor + readStandard;
                        WriteFile(sw, writeLine);

                        readStandard = "";
                    }

                    if (readLine.Contains("Record"))
                    {
                        writeLine = "";
                        readAuthor = "";
                        readTitle = "";
                        readStandard = "";                        
                    }
                }

                //WriteFile(sw, writeLine);  
                              
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            finally
            {
                //Console.WriteLine("Executing finally block.");
                //close the file
                sr.Close();
               
                //Close the file
                sw.Close();

                Console.WriteLine("Finished - press any key");
                Console.ReadLine();
            }            
        }

        public static void WriteFile(StreamWriter sw , String line)
        {
            try
            {
                //Write a line of text
                sw.WriteLine(line);

                //Write a second line of text
                //sw.WriteLine("From the StreamWriter class");               
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            finally
            {
                //Console.WriteLine("Executing finally block.");                
            }
        }

        public static String RemoveBrackets(String line)
        {
            int index = line.IndexOf("(");
            int indexEnd = 0;

            if (index > 0)
            {
                indexEnd = line.IndexOf(")");

                if (indexEnd > 0)
                {
                    int indexNum = indexEnd - index;
                    line = line.Remove (index, indexNum + 1);
                }
            }

            return line;

        }
    }
}

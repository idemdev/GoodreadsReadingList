using System;
using System.IO;

namespace GoodreadsConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // e.g. input record            

            /* Record 1 of xxx
            LOCATIONS Central City & Central City Adults
            AUTHOR Hill, A. J. (Alvin Joseph)
            TITLE Under pressure: the final voyage of Submarine S-Five / A.J.
                   Hill.
            PUB INFO New York: Free Press, c2002.
            DESCRIPT viii, 239 p., [8]
                    p.of plates : ill. ; 23 cm.
            NOTE Includes index.
            SUBJECT S-5 (Submarine)
            SUBJECT Submarine disasters -- North Atlantic Ocean.
            SUBJECT Submarine disasters -- United States.
            SUBJECT      Search and rescue operations -- Atlantic Ocean.
            SUBJECT Cooke, Charles Maynard.
            SUBJECT United States.Navy.Submarine forces.
            SUBJECT Nonfiction.
            ADD TITLE    The final voyage of submarine S-Five.
            STANDARD #   0743236777.
            1 > Central City basement n 910.91634 HIL AVAILABLE */

            // e.g. output record       

            // Title,Author,ISBN
            // The final voyage of submarine S - Five,Hill A J ,0743236777

            if (args.Length < 2)
            {
                Console.WriteLine("Usage is: GoodreadsConsoleApplication <Input File> <Output File>");
                Environment.Exit(0);
            }

            // Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(args[0]);
            // Pass the file path and filename to the StreamWriter Constructor
            // Output file format is csv
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
                // Read the first line of text
                readLine = sr.ReadLine();

                // Continue to read until you reach end of file
                while (readLine != null)
                {
                    // Write the line to console window
                   
                    // Read the next line
                    readLine = sr.ReadLine();

                    if (readLine == null)
                        break;

                    if (readLine.Contains("TITLE"))
                    {
                        int index = readLine.IndexOf("TITLE");
                        readTitle = readLine.Substring(index + 5).Trim();

                        // Remove extra text after "/"
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
                        // Can be more than on instance of "STANDARD"
                        // Each instance is a seperate write line

                        int index = readLine.IndexOf("STANDARD");
                        readStandard = readStandard + readLine.Substring(index + 10).Trim();

                        readStandard = readStandard.Replace(".", "");
                        readStandard = readStandard.Replace(":", "");
                        readStandard = readStandard.Replace(";", "");

                        // Can be two sets of brackets
                        readStandard = RemoveBrackets(readStandard);
                        readStandard = RemoveBrackets(readStandard);

                        writeLine = readTitle + readAuthor + readStandard;
                        WriteFile(sw, writeLine);

                        readStandard = "";
                    }

                    // "Record" starts a new book structure

                    if (readLine.Contains("Record"))
                    {
                        writeLine = "";
                        readAuthor = "";
                        readTitle = "";
                        readStandard = "";                        
                    }
                }               
            }

            catch (Exception e)
            {
                Console.WriteLine("Read Exception: " + e.Message);
            }

            finally
            {
                // Close the file
                sr.Close();
               
                // Close the file
                sw.Close();

                Console.WriteLine("Finished - press any key");
                Console.ReadLine();
            }            
        }

        public static void WriteFile(StreamWriter sw , String line)
        {
            try
            {
                // Write a line of text
                sw.WriteLine(line);                          
            }

            catch (Exception e)
            {
                Console.WriteLine("Write Exception: " + e.Message);
            }            
        }

        public static String RemoveBrackets(String line)
        {
            // Remove "( xxx )"

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

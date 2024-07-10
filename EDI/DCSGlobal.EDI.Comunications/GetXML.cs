using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Xml;


namespace DCSGlobal.EDI.Comunications
{

    public class XMLConfig
    {
        static void ReadConfig()
        {

            try
            {

                using (XmlReader reader = XmlReader.Create("perls.xml"))
                {
                    while (reader.Read())
                    {
                        // Only detect start elements.
                        if (reader.IsStartElement())
                        {
                            // Get element name and switch on it.
                            switch (reader.Name)
                            {
                                case "perls":
                                    // Detect this element.
                                    Console.WriteLine("Start <perls> element.");
                                    break;
                                case "article":
                                    // Detect this article element.
                                    Console.WriteLine("Start <article> element.");
                                    // Search for the attribute name on this current node.
                                    string attribute = reader["name"];
                                    if (attribute != null)
                                    {
                                        Console.WriteLine("  Has attribute name: " + attribute);
                                    }
                                    // Next read will contain text.
                                    if (reader.Read())
                                    {
                                        Console.WriteLine("  Text node: " + reader.Value.Trim());
                                    }
                                    break;
                            }
                        }

                    }
                }
            }
            catch(XmlException xml)
            {


            }
            catch(Exception ex)
            { 
            
            
            }
            
            Console.WriteLine("All Done :-)");
            Console.ReadLine();
        }
        static void WriteConfig()
        { }
        static void GetFromDB()
        { }



    }
}
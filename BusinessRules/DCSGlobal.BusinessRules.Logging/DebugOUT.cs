using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class DebugOUT
    {

        public void WriteDebugLine(int DebugLevel, string DebugLine)
        {

            switch (DebugLevel)
            {
                case 0:

                case 1:
                    Debug.WriteLine(DebugLine);
                    
                    break;


                case 2:
                    Console.WriteLine(DebugLine);
                    Debug.WriteLine(DebugLine);
                   
                    break;

                default:
                  

                break;

            }



        }


    }
}

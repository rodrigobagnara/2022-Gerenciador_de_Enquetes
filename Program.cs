using System;
using System.IO;
using System.Collections.Generic;

namespace Gerenciador_de_Enquetes
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Gerenciador de Enquetes Rev. 1.0 Beta");

            // Inicia a execução do programa.
            SurveyUI ui = new SurveyUI();
            ui.Start();
        }


    }
}



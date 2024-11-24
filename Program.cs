/*
 * @fileoverview    {Program}
 *
 * @version         2.0
 *
 * @author          Dyson Arley Parra Tilano <dysontilano@gmail.com>
 *
 * @copyright       Dyson Parra
 * @see             github.com/DysonParra
 *
 * History
 * @version 1.0     Implementation done.
 * @version 2.0     Documentation added.
 */
using System;

namespace Program {

    /**
    * TODO: Description of {@code Application}.
    *
    * @author Dyson Parra
    */
    public class Program {

        /**
        * Entrada principal del sistema.
        *
        * @param args argumentos de la linea de comandos.
        */
        public static void Main(String[] args) {
            Console.WriteLine($"\nStart date: {DateTime.Now}\n");
            Console.WriteLine($"\nResult: " + AppTester.StartTesting(args));
            Console.WriteLine($"\nEnd date:   {DateTime.Now}");
        }

    }
}
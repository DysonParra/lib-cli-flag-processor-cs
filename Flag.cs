/*
 * @fileoverview    {Flag}
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
    * TODO: Definición de {@code Flag}.
    *
    * @author Dyson Parra
    */
    public class Flag {

        /* Nombre de la flag */
        public String? Name {get; set;}
        /* Valor de la flag */
        public String? Value {get; set;}
        /* Si es una flag requerida */
        public bool Required {get; set;}

        /**
        * Obtiene el valor en {String} del objeto actual.
        *
        * @return un {String} con la representación del objeto.
        */
        public override String ToString() {
            String result = "";
            if (Required)
                result += "*";
            else
                result += " ";

            if (Value == null)
                result += Name;
            else
                result += Name + " = " + Value;

            return result;
        }

    }
}
using System;
using System.Collections.Generic;

namespace Gerenciador_de_Enquetes
{
    class OptionScore : IComparable<OptionScore> // Votos de uma opção
    {
        public Option Option { get; private set; } // Opção.

        public int Count { get; private set; } // Número de votos.

        public OptionScore(Option option, int score) // Construtor
        {
            this.Option = option;
            this.Count = score;
        }

        public int CompareTo(OptionScore other) // Define a comparação como ordem decrescente de votos.
        {
            // Se duas opções tiverem o mesmo número e votos, usa o critério de ordem alfabética do texto.
            int comp = -Count.CompareTo(other.Count);

            if (comp == 0) 
            {
                return Option.Text.CompareTo(other.Option.Text); // Comparação alfabética do texto da opção.
            }

            return comp;
        }

    }
}

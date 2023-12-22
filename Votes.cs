using System;
using System.Collections.Generic;
using System.IO;

namespace Gerenciador_de_Enquetes
{
    partial class Survey // Survey é quebrada em duas partes pois a class Votes é muito grande, assim facilitando a leitura do código.
    {

        // Apenas a enquete necessita saber da existencia da class Votes.
        private class Votes : IStorable
        {
            private Survey survey; // Enquete associada.

            private Dictionary<Option, int> votes = new Dictionary<Option, int>(); // Dicionário que mapeia uma opção da enquete para um número de votos.

            public int VoteCount { get; private set; } // Contador de votos da enquete.

            public Votes(Survey survey) // Construtor.
            {
                this.survey = survey;
            }

            public void AddVote(Option option) // Adiciona um voto a enquete
            {
                int count;

                if (votes.TryGetValue(option, out count))
                {
                    // Se a opção já teve algum voto, incrementa o número de votos.
                    count++;
                    votes[option] = count;
                }
                else
                {
                    // Se a opção ainda não havia sido votada, considera 1 voto.
                    votes[option] = 1;
                }

                VoteCount++; // Incrementa o número total de votos da enquete.
            }

            public List<OptionScore> CalculateScores(bool sort = true) // Calcula os votos da enquete
            {
                List<OptionScore> scores = new List<OptionScore>();

                foreach (KeyValuePair<Option, int> entry in votes)
                {
                    scores.Add(new OptionScore(entry.Key, entry.Value));
                }

                if (sort) // Se a ordenação ainda for true, faça...
                {
                    scores.Sort(); // Ordena a lista se for necessario.
                }

                return scores;
            }

            public void Save(BinaryWriter writer)
            {
                // Grava o tamanho do dicionário
                writer.Write(votes.Count);

                foreach(KeyValuePair<Option, int> entry in votes)
                {
                    // Grava cada um dos elementos do dicionário: a opção e depois o número de votos.
                    Option option = entry.Key;
                    int numVotes = entry.Value;

                    option.Save(writer); // Chama o Save() de Option para gravar a opção.

                    writer.Write(numVotes);
                }
            }

            public void Load(BinaryReader reader)
            {
                int count = reader.ReadInt32(); // Lê o tamanho do dicionário.

                for(int i = 0; i < count; i++) // Intera criando as opções e seus respectivos votos.
                {
                    Option option = new Option();
                    option.Load(reader); // Chama o Load() de Option para ler a opção.
                    int numVotes = reader.ReadInt32();
                    VoteCount += numVotes; // Acumula o número total de votos.
                    votes.Add(option, numVotes);
                }
            }

        }
    }
}

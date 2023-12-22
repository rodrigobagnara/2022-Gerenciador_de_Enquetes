using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Gerenciador_de_Enquetes
{
    partial class Survey : IStorable
    {
        // Dicionário que mapeia um ID de uma opção para uma opção.
        private IDictionary<string, Option> options = new Dictionary<string, Option>();

        // Referencia objeto responsável por calcular os votos.
        private Votes votes;

        public string Question { get; set; } // Questão da enquete

        public int VoteCount // Contador de votos da enquete.
        {
            get
            {
                return votes.VoteCount; // Delega a chamada para VoteCount do objeto Votes.
            }
        }

        public Survey() // Método construtor de Survey.
        {
            votes = new Votes(this); // Instancia o objeto que calcula os votos.
        }

        // Adiciona ou altera uma opção da enquete. Se o ID ainda não existe, ele adiciona, se não ele altera.
        public void SetOption(string id, string text)
        {
            // Cria a opção, convertendo o ID para maiúsculo.
            Option option = new Option();
            option.Id = id.ToUpper();
            option.Text = text;

            if (!options.ContainsKey(id)) 
            {
                // Adiciona se o ID não existe.
                options.Add(id, option);
            }
            else
            {
                // Altera se o ID já existe.
                options[id] = option;
            }

        }

        public string GetFormattedSurvey() //Retorna a enquete em um formato de string.
        {
            // Usa um StringBuilder para evitar concatenação de strings.
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Question); // 1º mostra a pergunta.

            foreach (Option option in options.Values) // 2º mostra as opções.
            {
                sb.Append(option.Id).Append(" - ").AppendLine(option.Text);
            }

            return sb.ToString();
        }

        public bool Vote(out Option option, out string vote) // Vota na enquete, através da digitação da opção no console.
        {
            
            vote = Console.ReadLine(); // Lê o voto...

            vote = vote.ToUpper(); // Converte o voto para maiúsculo.

            bool valid = options.TryGetValue(vote, out option); // Busca o objeto no dicionário.

            if (valid) 
            {
                votes.AddVote(option); // Caso tenha encontrado, computa o voto.
            }

            return valid;
        }

        public List<OptionScore> CalculateScores(bool sort = true) // Calcula os votos da enquete.
        {
            return votes.CalculateScores(sort); // Delega o cálculo para o objeto Votes.
        }

        public void Save(BinaryWriter writer)
        {
            // Salva a questão, o numero de opções e depois cada uma das opções...
            writer.Write(Question);
            writer.Write(options.Count);

            foreach(Option option in options.Values)
            {
                option.Save(writer); // Chama o Save() de option para salvar a opção.
            }

            votes.Save(writer); // Salva os votos da enquete.
        }

        public void Load(BinaryReader reader)
        {
            Question = reader.ReadString(); // Carrega a questão da enquete e depois cada uma das opções.

            options = new Dictionary<string, Option>();
            int count = reader.ReadInt32();

            for(int i = 0; i < count; i++)
            {
                Option option = new Option();
                option.Load(reader); // Chama o Load() de Option para ler a opção.
                options[option.Id] = option;
            }

            votes.Load(reader); // Carrega os votos da enquete.
        }
    }
}

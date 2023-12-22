using System;
using System.Collections.Generic;
using System.IO;

namespace Gerenciador_de_Enquetes
{
    class SurveyUI // Gêrencia a interface gráfica da aplicação
    {

        private Survey survey; // Enquete ativa.

        private string surveyFile; // Arquivo associado a enquete.

        public void Start() // Inicia a execução da aplicação.
        {
            while (true)
            {
                string option = ShowMainMenu(); // Mostra o menu principal (O retorno é a opção escolhida).

                if (option == "1")
                {
                    // Cria uma enquete e mostra o menu da enquete.
                    ShowCreateMenu();
                    ShowSurveyMenu();
                }
                else if (option == "2")
                {
                    // Carrega uma enquete e mostra o menu da enquete.
                    ShowLoadMenu();
                    ShowSurveyMenu();
                }
                else if (option == "3")
                {
                    return; // Sair da aplicação (sai do método Start()).
                }
            }
        }

        private string ShowMainMenu() // Mostra o menu principal.
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-- Menu Principal -- \n");
                Console.WriteLine("1 - Criar uma nova enquete.");
                Console.WriteLine("2 - Carregar uma enquete...");
                Console.WriteLine("3 - Sair! \n\n");
                Console.WriteLine("O que deseja fazer?");

                string option = Console.ReadLine(); // Captura o valor digitado pelo usuario.

                if (option != "1" && option != "2" && option != "3")
                {
                    continue; // Enquanto a opção digitada for inválida, fica no loop.
                }

                return option;
            }
        }

        private void ShowCreateMenu() // Mostra o menu de criar enquete
        {
            survey = new Survey();
            surveyFile = null;

            Console.Clear();
            Console.WriteLine("-- 1: Criar uma Nova Enquete -- \n");

            while (true)
            {
                Console.WriteLine("Pergunta: ");
                string question = Console.ReadLine(); // Solicita a pergunta da enquete.
                if(!String.IsNullOrEmpty(question))
                {
                    survey.Question = question;
                    break;
                }
            }

            int numOptions;
            while (true)
            {
                Console.WriteLine("\nQuantas opções a pergunta vai ter? "); // Solicita o número de opções

                try
                {
                    numOptions = int.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    // Não faz nada se ocorrer exceção...
                }
            }

            // Solicita cada uma das opções (ID e texto).
            for (int i = 0; i < numOptions; i++)
            {
                string id;
                string text;

                while (true)
                {
                    Console.Write("\nID da opção {0}: ", i + 1);
                    id = Console.ReadLine();
                    if (!String.IsNullOrEmpty(id))
                    {
                        break;
                    }
                }

                while (true)
                {
                    Console.Write("\nTexto da opção {0}: ", i + 1);
                    text = Console.ReadLine();
                    if (!String.IsNullOrEmpty(text))
                    {
                        break;
                    }
                }

                survey.SetOption(id, text); // Adiciona a opção a enquete.
            }

            // Mostra a enquete.
            Console.WriteLine("Opção adicionadas com sucesso! Veja a enquete: \n");
            Console.WriteLine(survey.GetFormattedSurvey());

            while (true)
            {
                // Solicita um arquivo para gravação da nova enquete.
                Console.WriteLine("Digite o caminho do arquivo para salvar a enquete: ");
                string filePath = Console.ReadLine();

                if (!String.IsNullOrWhiteSpace(filePath))
                {
                    try
                    {
                        SurveyIO.SaveToFile(survey, filePath); // Salva a enquete no arquivo.
                        surveyFile = filePath;
                        break;
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("Ocorreu um erro ao salvar o arquivo: {0}", e.Message);
                    }
                }
            }

            Console.WriteLine("Enquete salva em \"{0}\". Pressione ENTER para continuar...", surveyFile);
            Console.ReadLine();
        }

        private void ShowLoadMenu() // Mostra o menu de carregamento de enquete.
        {
            survey = new Survey();

            Console.Clear();
            Console.WriteLine("-- 2: Carregar uma Enquete -- \n");

            while (true)
            {
                Console.WriteLine("Digite o nome do arquivo da enquete: "); // Solicita o caminho onde a enquete está gravada.
                string filePath = Console.ReadLine();

                if (!String.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        SurveyIO.LoadFromFile(survey, filePath); // Carrega a enquete do arquivo.
                        surveyFile = filePath;
                        Console.WriteLine("A enquente foi carregada com sucesso! Pressione ENTER para continuar...");
                        Console.ReadLine();
                        break;
                    }
                    catch (IOException e) 
                    {
                        Console.WriteLine("Ocorreu um erro ao abrir o arquivo: {0}", e.Message);
                    }
                }
            }
        }

        private void ShowSurveyMenu() // Mostra o menu de enquete, onde é possivel votar ou ver o resultado.
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-- Menu de Enquete -- \n");
                Console.WriteLine("Enquete ativa: \"{0}\" \n", survey.Question);
                Console.WriteLine("Número de votos: \"{0}\" \n", survey.VoteCount);

                Console.WriteLine("1 - Votar na Enquete.");
                Console.WriteLine("2 - Ver Resultados da Enquete.");
                Console.WriteLine("3 - Voltar ao Menu Principal.");
                Console.WriteLine("Escolha uma Opção => ");
                string option = Console.ReadLine();

                if (option == "1")
                {
                    ShowVoteMenu(); // Votar na enquete.
                }
                else if (option == "2") 
                {
                    ShowSurveyResults(); // Mostrar resultador da enquete.
                }
                else if (option == "3") 
                {
                    break; // Voltar para o menu principal.
                }
                else
                {
                    continue; // Enquanto a opção digitada for inválida, fica no loop.
                }
            }

            return;
        }

        private void ShowVoteMenu() // Mostra o menu de votação na enquete.
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-- Votar -- \n");


                Console.WriteLine("Quantidade de votos: {0}\n", survey.VoteCount);
                Console.WriteLine(survey.GetFormattedSurvey());
                Console.WriteLine("Escolha uma opção: ");

                Option option;
                string vote;

                bool valid = survey.Vote(out option, out vote); // Solicita o voto.

                if (valid)
                {
                    Console.WriteLine("Obrigado pelo seu voto! Deseja continuar? (S/N):");
                    string yn = Console.ReadLine();

                    if (yn != "S" && yn != "s")
                    {
                        break;
                    }
                }
            }

            SurveyIO.SaveToFile(survey, surveyFile); // Ao final da votação, salva a enquete no arquivo associado.
            Console.Write("Fim da votação. Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        private void ShowSurveyResults() // Mostra o resultado da enquete.
        {
            Console.Clear();
            Console.WriteLine("-- Resultado da enquete -- \n");

            List<OptionScore> scores = survey.CalculateScores(); // Calcula o resultado.

            Console.WriteLine("{0, -3} | {1, -5}", "Opção", "Votos");
            Console.WriteLine("\n");

            foreach (OptionScore score in scores)
            {
                Console.WriteLine("{0,-3}{1,-20} | {2,5}", score.Option.Id, score.Option.Text, score.Count);
            }

            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }

    }
}

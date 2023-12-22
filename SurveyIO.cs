using System;
using System.IO;

namespace Gerenciador_de_Enquetes
{
    static class SurveyIO // Agrupa operações de I/O da aplicação (38min)
    {

        public static void SaveToFile(IStorable obj, string filePath) // Salva um objeto para um arquivo.
        {
            FileInfo file = new FileInfo(filePath);

            using (BinaryWriter writer = new BinaryWriter(file.OpenWrite())) // o using chama o método dispose ao final do método.
            {
                obj.Save(writer);
            }
        }

        public static void LoadFromFile(IStorable obj, string filePath) // Carrega um objeto com dados de um arquivo.
        {
            FileInfo file = new FileInfo(filePath);

            using (BinaryReader reader = new BinaryReader(file.OpenRead()))
            {
                obj.Load(reader);
            }
        }
    }

    interface IStorable // Interface que define os métodos de salvar e carregar dados usando arquivos.
    {
        void Save(BinaryWriter writer); // Grava os dados.

        void Load(BinaryReader reader); // Lê os dados.
    }
}

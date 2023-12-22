using System;
using System.IO;

namespace Gerenciador_de_Enquetes
{
    internal class Option : IStorable, IEquatable<Option>
    {
        public string Id { get; set; } // ID da opção (o que deve ser digitado para a opção).

        public string Text { get; set; } // Texto associado a opção.

        public void Save(BinaryWriter writer) // Determina como o objeto será salvo.
        {
            writer.Write(Id);
            writer.Write(Text);
        }

        public void Load(BinaryReader reader) // Determina como o objeto será lido.
        {
            Id = reader.ReadString();
            Text = reader.ReadString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Option);
        }

        public bool Equals(Option other) // Método da interface IEquatable<T>.
        {
            if(other == null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

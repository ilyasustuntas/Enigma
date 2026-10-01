using System;
using System.Security.Cryptography;

namespace Enigma
{
    public class Rotors
    {
        private List<Rotor> rotors = new List<Rotor>();
        public string alphabet = "";

        public string M_rotorProcess(string text)
        {
            Console.WriteLine("işlem başladı");
            resetAllRotors();
            Reflector reflector = new Reflector(alphabet);
            string returntext = "";

            for (int i = 0; i < text.Length; i++)
            {
                string letter = text[i].ToString();
                if (alphabet.IndexOf(letter, StringComparison.Ordinal) == -1)
                {
                    continue;
                }
                Console.WriteLine("HARF İŞLENMEYE BAŞLADI: " + letter);
                string processing_char = letter;
                foreach (Rotor rot in rotors)
                {
                    Console.WriteLine("rotorlardan ilk geçiş sağlandı");
                    processing_char = rot.rotorProcess(processing_char);
                }
                Console.WriteLine("reflektörden geçildi");
                processing_char = reflector.reflectorProcess(processing_char);

                for (int b = rotors.Count - 1; b >= 0; b--)
                {
                    Console.WriteLine("rotorlar tesine geçiliyor");
                    Rotor rot = rotors[b];

                    processing_char = rot.rotorUnprocess(processing_char);
                }
                rotateAllRotors();
                returntext += processing_char;
                Console.WriteLine("işlenen harf sonuç metnine eklendi");
            }
            return "SONUÇ :" + returntext + ": budur";
        }

        public void addRotor(string value)
        {
            rotors.Add(new Rotor(alphabet, value));
            Console.WriteLine("rotor eklendi: " + value);
        }
        public void addAlphabet(string Alphabet)
        {
            alphabet = Alphabet;
            Console.WriteLine("alfabe eklendi: " + Alphabet);
        }
        private void rotateAllRotors()
        {
            foreach (Rotor rot in rotors)
            {
                rot.rotate();
            }
        }
        private void resetAllRotors()
        {
            foreach (Rotor rot in rotors)
            {
                rot.resetRotor();
            }
            Console.WriteLine("tüm rotorlar sıfırlandı");
        }
    }



    internal class Rotor
    {
        string rotorAlphabet = "";
        public string value = "";
        public string CurrentValue = "";

        public Rotor(string alphabet, string Value) // <------- constructor methot
        {
            rotorAlphabet = alphabet;
            value = Value;
            CurrentValue = Value;
        }

        public string rotorProcess(string letter)
        {
            int index = rotorAlphabet.IndexOf(letter, StringComparison.Ordinal);
            string result = Convert.ToString(CurrentValue[index]);
            return result;
        }
        public string rotorUnprocess(string letter)
        {
            int index = CurrentValue.IndexOf(letter, StringComparison.Ordinal);
            string result = Convert.ToString(rotorAlphabet[index]);
            return result;
        }

        public void resetRotor()
        {
            CurrentValue = value;
        }

        public void rotate()
        {
            CurrentValue = CurrentValue[^1] + CurrentValue[..^1];
        }
    }


    internal class Reflector
    {
        string reflector_alphabet = "";
        string reflector_reversed_alphabet = "";
        public Reflector(string alphabet)
        {
            reflector_alphabet = alphabet;
            reflector_reversed_alphabet = new string(alphabet.Reverse().ToArray());
        }

        public string reflectorProcess(string letter)
        {
            int index = reflector_alphabet.IndexOf(letter, StringComparison.Ordinal);
            return Convert.ToString(reflector_reversed_alphabet[index]);
        }
    }
}
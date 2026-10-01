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
            resetAllRotors();
            Reflector reflector = new Reflector(alphabet);
            string returntext = "";
            text = text.ToUpper();

            for (int i = 0; i < text.Length; i++)
            {
                string letter = text[i].ToString();
                if (alphabet.IndexOf(letter) == -1)
                {
                    continue;
                }
                string processing_char = letter;
                foreach (Rotor rot in rotors)
                {
                    processing_char = rot.rotorProcess(processing_char);
                }

                processing_char = reflector.reflectorProcess(processing_char);

                for (int b = rotors.Count - 1; b >= 0; b--)
                {
                    Rotor rot = rotors[b];

                    processing_char = rot.rotorUnprocess(processing_char);
                }
                rotateAllRotors();
                returntext += processing_char;
            }
            return ":" + returntext + ":";
        }

        public void addRotor(string value)
        {
            rotors.Add(new Rotor(alphabet, value));
        }
        public void addAlphabet(string Alphabet)
        {
            alphabet = Alphabet.ToUpper();
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


    internal class Reflector //abcdef   <--- example
                             //fedcba
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
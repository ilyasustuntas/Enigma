namespace Enigma
{
    internal class PlugBoard
    {
        List<string> p1 = new List<string>();
        List<string> p2 = new List<string>();


        public string boardProcess(string text)
        {
            string returntext = "";
            string processing_char = "";
            foreach (char letter in text)
            {
                processing_char = letter.ToString();
                if (p1.Contains(processing_char))
                {
                    returntext += p2[p1.IndexOf(processing_char)];
                }
                else if (p2.Contains(processing_char))
                {
                    returntext += p1[p2.IndexOf(processing_char)];
                }
                else
                {
                    returntext += processing_char;
                }
            }
            return returntext;
        }


        public bool addPlug(string l1, string l2)
        {
            if (!p1.Contains(l1) && !p1.Contains(l2) && !p2.Contains(l1) && !p2.Contains(l2))
            {
                p1.Add(l1);
                p2.Add(l2);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
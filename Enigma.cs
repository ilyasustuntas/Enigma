namespace Enigma;

public class Enigma
{
    string default_Alphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜWVXYZ ";
    List<string> Rotors = ["ÇRDMPH ĞCUZEÜXJİIKŞOÖLVNYWAGTSBF", "NKÜYFBXEWLHAVTĞÖJİUZ GÇORPMSIDŞC", "WOISGXUTRAÜVÖĞKZ FMNŞBDYÇİHPCEJL"];
    Rotors rs = new Rotors();
    PlugBoard ps = new PlugBoard();
    bool isChanged = false;

    public Enigma()
    {
        setup();
    }

    public string process(string text)// <------- main function
    {
        if (isChanged)
        {
            setup();
            isChanged = false;
        }
        text = ps.boardProcess(text);
        text = rs.M_rotorProcess(text);
        text = ps.boardProcess(text);
        return text;
    }
    private void setup()
    {
        rs = new Rotors();
        rs.addAlphabet(default_Alphabet);
        foreach (string rot in Rotors)
        {
            rs.addRotor(rot);
        }
    }

    public void setAlphabet(string alphabet)
    {
        default_Alphabet = alphabet;
        isChanged = true;
    }
    public void cleanRotors()
    {
        Rotors.Clear();
        isChanged = true;
    }
    public void addRotor(string rotor)
    {
        Rotors.Add(rotor);
        isChanged = true;
    }
    public void cleanPlugboard()
    {
        ps = new PlugBoard();
    }
    public bool addPlugToBoard(string s1, string s2)
    {
        return ps.addPlug(s1, s2);
    }
}
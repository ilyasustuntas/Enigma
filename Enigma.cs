namespace Enigma;

public class Enigma
{
    string default_Alphabet = "";
    List<string> Rotors = new List<string>();
    Rotors rs = new Rotors();
    Plugborad ps = new Plugborad();
    bool isChanged = false;

    public string process(string text)// <------- main function
    {
        text = ps.boardProcess(text);
        text = rs.M_rotorProcess(text);
        text = ps.boardProcess(text);
        return text;
    }
    public void setup()
    {
        rs.addAlphabet(default_Alphabet);
        foreach (string rot in Rotors)
        {
            rs.addRotor(rot);
        }
    }

    public void setAlphabet(string alphabet)
    {
        default_Alphabet = alphabet;
    }
    public void cleanRotors()
    {
        rs = new Rotors();
    }
    public void addRotor(string rotor)
    {
        Rotors.Add(rotor);
    }
    public void cleanPlugboard()
    {
        ps = new Plugborad();
    }
    public bool addPlugToBoard(string s1, string s2)
    {
        return ps.addPlug(s1, s2);
    }
}
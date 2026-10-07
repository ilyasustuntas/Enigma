# Enigma Machine for C#

A small, customizable Enigma-style cipher machine written in C#. It ships as a single `.dll` that you can reference from any .NET project. You can tweak the alphabet, the rotors and the plugboard, and the same machine both encrypts and decrypts, just like the original.

> ## ⚠️ Disclaimer
>
> **This is a simple hobby project, developed by a single person for fun and learning.**
>
> - It has **not** been audited or reviewed by security professionals.
> - The Enigma design is historically broken, and this implementation is a simplified variation of it.
> - **Do NOT use it to protect sensitive, private, official, financial or otherwise important data.**
> - For anything serious, use a modern, well-reviewed cryptography library (for example AES-GCM from `System.Security.Cryptography`).
>
> The project is provided *as is*, without warranty of any kind.

---

## Table of Contents

- [Features](#features)
- [How It Works](#how-it-works)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [API Reference](#api-reference)
- [Customizing the Machine](#customizing-the-machine)
- [Important Notes and Limitations](#important-notes-and-limitations)
- [Project Structure](#project-structure)

---

## Features

- Symmetric: running the output through the machine again with the same settings gives back the original text
- Three rotors by default, and you can add as many as you like
- Plugboard for swapping pairs of letters
- Custom alphabets, including Turkish letters (the default alphabet contains them)
- A character never encrypts to itself, as in the real Enigma
- Characters outside the alphabet pass through unchanged
- Tiny API: one public class, `EnigmaMachine`

## How It Works

Every character that belongs to the alphabet takes this path:

```
input → Plugboard → Rotor 1 → Rotor 2 → Rotor 3 → Reflector
                                                      │
output ← Plugboard ← Rotor 1 ← Rotor 2 ← Rotor 3 ←────┘
```

1. **Plugboard** swaps letters in pairs (for example `A ↔ K`). It is applied once before and once after the rotors.
2. **Rotors** substitute each letter using their wiring. The signal goes through the rotors in the order they were added, reaches the reflector, then goes back through the rotors in reverse order.
3. **Reflector** pairs each letter with its mirror position in the alphabet (first ↔ last, second ↔ second-to-last, and so on). This is why the alphabet length **must be even**.
4. **Stepping**: after every character the rotors turn. Rotor 1 turns after every character, Rotor 2 once every 3 characters, Rotor 3 once every 9 characters, and so on.

At the start of every `process()` call the rotors are reset to their initial positions. Each call is therefore independent, and encrypting and decrypting always line up.

**Default alphabet** (32 characters, the last one is a space):

```
ABCÇDEFGĞHIİJKLMNOÖPQRSŞTUÜWVXYZ '
```

There is space between Z and ' and space suported

---

## Installation

### Requirements

- .NET 8.0 or newer (the library uses C# 12 language features)

### Download the DLL (recommended)

1. Go to the [**Releases**] page of this repository.
2. Download the latest `Enigma.dll`.
3. Put it somewhere in your project, for example in a `libs` folder.
4. Add a reference to it.

**Visual Studio**

> Right-click **Dependencies** → **Add Project Reference…** → **Browse…** → select `Enigma.dll`.

**`.csproj` file / .NET CLI**

```xml
<ItemGroup>
  <Reference Include="Enigma">
    <HintPath>libs\Enigma.dll</HintPath>
  </Reference>
</ItemGroup>
```

5. Add the namespace to your code:

```csharp
using Enigma;
```

---

## Quick Start

```csharp
using Enigma;

var enigma = new EnigmaMachine();

// Optional: connect letters on the plugboard (each letter can be used only once)
enigma.addPlugToBoard("A", "K");
enigma.addPlugToBoard("M", "Z");

// Encrypt
string encrypted = enigma.process("HELLO WORLD");

// Decrypt: same machine (or any machine with identical settings), same method
string decrypted = enigma.process(encrypted);

Console.WriteLine(decrypted); // HELLO WORLD
```

> **Use UPPERCASE text.** The machine does not change the case of your input. Lowercase letters are not in the alphabet, so they pass through unencrypted.

---

## API Reference

All of these are members of `EnigmaMachine`.

| Member | Description |
|---|---|
| `EnigmaMachine()` | Creates a machine with the default alphabet, three default rotors and an empty plugboard. |
| `string process(string text)` | Encrypts or decrypts `text`. The operation is symmetric. |
| `bool addPlugToBoard(string a, string b)` | Connects two letters on the plugboard. Returns `true` on success, or `false` if either letter is already plugged. |
| `void cleanPlugboard()` | Removes all plugboard connections. |
| `void setAlphabet(string alphabet)` | Replaces the alphabet. You must also replace the rotors. |
| `void cleanRotors()` | Removes all rotors, including the defaults. |
| `void addRotor(string wiring)` | Adds a rotor. The order in which you add rotors matters. |

Changes to the alphabet or the rotors take effect on the next `process()` call.

---

## Customizing the Machine

The "key" of your machine is the combination of **alphabet + rotor wirings + rotor order + plugboard pairs**. Both sides must use exactly the same settings.

```csharp
using Enigma;

var enigma = new EnigmaMachine();

// 1. Use a plain English alphabet (26 letters, even length, if you want to use space you must add)
enigma.setAlphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

// 2. Remove the default rotors and add your own, you can add as much as you want.
enigma.cleanRotors();
enigma.addRotor("EKMFLGDQVZNTOWYHXUSPAIBRCJ"); // rotors must be a combination of your alphabet.
enigma.addRotor("AJDKSIRUXBLHWTMCQGZNPYFVOE");
enigma.addRotor("BDFHJLCPRTXVZNYEIWGAKMUSQO");

// 3. Configure the plugboard
enigma.cleanPlugboard();
enigma.addPlugToBoard("A", "B");
enigma.addPlugToBoard("C", "D");

string encrypted = enigma.process("HELLO WORLD"); // the space is not in this alphabet, so it passes through
string decrypted = enigma.process(encrypted);
```

### Rules for alphabets and rotors

- The alphabet must have an **even number** of characters.
- Every character in the alphabet must be **unique**.
- Every rotor wiring must be the **same length** as the alphabet and contain **every alphabet character exactly once**. In other words, it is a shuffled version of the alphabet.
- If you change the alphabet, you **must** replace the rotors too. The default rotors only fit the default alphabet.
- Write the alphabet and the rotors in **uppercase**. The alphabet is converted with `ToUpper()`, but rotor wirings are used as given.
- Only plug letters that are part of the alphabet.
- Plugboard letters are single characters.

A rotor wiring that breaks these rules usually causes an exception such as `IndexOutOfRangeException` or produces text that cannot be decrypted.

---

## Important Notes and Limitations

- **Not secure.** See the disclaimer at the top. The default rotor wirings are public in the source code, so the default configuration offers no secrecy at all.
- **Simplified stepping.** The rotor stepping is a custom design and differs from the historical Enigma, which used 26 positions and turnover notches.
- **Fixed reflector.** The reflector is always the reversed alphabet and cannot be configured.
- **Case sensitive.** Input should be uppercase, as described above.
- **No input validation.** Invalid rotor or alphabet settings are not checked and will throw exceptions when used.
- **Not thread-safe.** Use one `EnigmaMachine` instance per thread.
- **No state between calls.** Rotor positions reset on every `process()` call, so process a whole message in one call.

---

## Project Structure

| Class | Visibility | Role |
|---|---|---|
| `EnigmaMachine` | `public` | The entry point. Combines the plugboard, rotors and reflector. |
| `PlugBoard` | `internal` | Swaps letter pairs. |
| `Rotors` | `internal` | Manages the rotor stack, stepping and the reflector pass. |
| `Rotor` | `internal` | A single rotor with its wiring and position. |
| `Reflector` | `internal` | Mirrors each letter to the opposite end of the alphabet. |

---

## License

This project is provided as is, without warranty of any kind. Use it at your own risk.

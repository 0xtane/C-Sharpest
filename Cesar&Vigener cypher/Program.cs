using System;
using System.Text;

// Using demo code from gitlab:https://git.akaver.com/ics0031-2020f/course-materials/-/blob/master/demos/HW01Demo/ConsoleApp01/Program.cs
// as a template for whole homework, caesar encrypt was available
// adding decrypt and vigenere (e+d) option

namespace Homework1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ics0031-2020f Andres.Kaver HW01");

            var userInput = "";
            do
            {
                Console.WriteLine();
                Console.WriteLine("1) Cesar cipher");
                Console.WriteLine("2) Vigenere cipher");
                Console.WriteLine("X) Exit");
                Console.Write(">");

                userInput = Console.ReadLine()?.ToLower();

                switch (userInput)
                {
                    case "1":
                        Cesar();
                        break;
                    case "2":
                        Vigenere();
                        break;
                    case "x":
                        Console.WriteLine("closing down...");
                        break;
                    default:
                        Console.WriteLine($"Don't have this '{userInput}' as an option!");
                        break;
                }
            } while (userInput != "x");
        }


        static void Cesar()
        {
            Console.WriteLine("Cesar Cipher");

            // byte per character
            // 0-255
            // 0-127 - latin
            // 128-255 - change what you want
            // ABCD - A 189, B - 195, C 196, D 202
            // unicode 
            // AÄÖÜLA❌
            
            var userInput = "";
            var key = 0;
            do
            {
                Console.Write("Please enter your shift amount (or X to cancel):");
                userInput = Console.ReadLine()?.ToLower().Trim();
                if (userInput != "x")
                {
                    if (int.TryParse(userInput, out var userValue))
                    {
                        key = userValue % 255;
                        if (key == 0)
                        {
                            Console.WriteLine("multiples of 255 is no cipher, this would not do anything!");
                        }
                        else
                        {
                           Console.WriteLine($"Cesar key is: {key}");
                        }
                    }
                }

            } while (key == 0 && userInput != "x");

            if (userInput == "x") return;
            
 
            Console.Write("Please enter your plaintext:");
            var plainText = Console.ReadLine();
            if (plainText != null)
            {
                Console.WriteLine($"length of text: {plainText.Length}");
                
                ShowEncoding(plainText, Encoding.Default);
                
                // choose to encrypt or decrypt
                Console.Write("1 - for encrypt, 2 - for decrypt (or X to go back):");
                userInput = Console.ReadLine()?.ToLower();

                switch (userInput)
                {
                    case "1":
                        //encrypt()
                        var encryptedBytes = CesarEncryptString(plainText, (byte) key, Encoding.Default);
                
                        Console.Write("Encrypted bytes: ");
                        foreach (var encryptedByte in encryptedBytes)
                        {
                            Console.Write(encryptedByte + " ");
                        }

                        Console.WriteLine("base64: " + System.Convert.ToBase64String(encryptedBytes));
                        
                        break;
                    case "2":
                        //decrypt() - todo
                        var decryptedBytes = CesarDecryptString(plainText, (byte) key, Encoding.Default);
                
                        Console.Write("Decrypted bytes: ");
                        foreach (var decryptedByte in decryptedBytes)
                        {
                            Console.Write(decryptedByte + " ");
                        }

                        Console.WriteLine("text(UTF-8): " + Encoding.Default.GetString(decryptedBytes));
                        
                        
                        break;
                    case "x":
                        Console.WriteLine("closing down...");
                        break;
                    default:
                        Console.WriteLine($"Don't have this '{userInput}' as an option!");
                        break;
                }
                
                // end of chooosing
                
                /*
                ShowEncoding(plainText, Encoding.UTF7);
                ShowEncoding(plainText, Encoding.UTF8);
                ShowEncoding(plainText, Encoding.UTF32);
                ShowEncoding(plainText, Encoding.Unicode);
                ShowEncoding(plainText, Encoding.ASCII);
                ShowEncoding(plainText, Encoding.Default); // most likely UTF-8
                */
                
            }
            else
            {
                Console.WriteLine("Plaintext is null!");
            }


        }

        static void Vigenere()
        {
            Console.WriteLine("Vigenere Cipher");

           
            

            Console.Write("Please enter your plaintext:");
            var plainText = Console.ReadLine();
            Console.Write("Please enter your key:");
            var key = Console.ReadLine();
            
            if (plainText != null && key != null)
            {
                Console.WriteLine($"length of text: {plainText.Length}");
                Console.WriteLine($"length of key: {key.Length}");

                ShowEncoding(plainText, Encoding.Default);

                // choose to encrypt or decrypt
                Console.Write("1 - for encrypt, 2 - for decrypt (or X to go back):");
                var userInput = Console.ReadLine()?.ToLower();

                switch (userInput)
                {
                    case "1":
                        //encrypt()
                        Console.WriteLine(VigenereEncryptString(plainText,key,Encoding.Default));

                        break;
                    case "2":
                        //decrypt()
                        Console.WriteLine(VigenereDecryptString(plainText,key,Encoding.Default));

                        break;
                    case "x":
                        Console.WriteLine("closing down...");
                        break;
                    default:
                        Console.WriteLine($"Don't have this '{userInput}' as an option!");
                        break;
                }

            }
            else
            {
                Console.WriteLine("Plaintext or key is null!");
            }
        }

        static string VigenereEncryptString(string input, string key, Encoding encoding)
        {
            var inputBytes = encoding.GetBytes(input);
            var keyBytes = encoding.GetBytes(key);

            return Convert.ToBase64String(VigenereEncrypt(inputBytes, keyBytes));
        }

        static byte[] VigenereEncrypt(byte[] input, byte[] key)
        {
            var result = new byte[input.Length];
            
            // make the key the size of the plaintext
            
            var resizedKey = new byte[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                resizedKey[i] = key[i % key.Length];
            }
            // //

            for (var i = 0; i < input.Length; i++)
            {

                var newCharValue = (input[i] + resizedKey[i]) % byte.MaxValue;
                if (newCharValue == 0)
                {
                    newCharValue = byte.MaxValue;
                }
                result[i] = (byte) newCharValue;
            }
            return result;
        }
        
        
        static string VigenereDecryptString(string input, string key, Encoding encoding)
        {
            var inputBytes = Convert.FromBase64String(input);
            var keyBytes = encoding.GetBytes(key);

            return encoding.GetString(VigenereDecrypt(inputBytes, keyBytes));
        }
        
        static byte[] VigenereDecrypt(byte[] input, byte[] key)
        {
            var result = new byte[input.Length];
            
            // make the key the size of the plaintext
            
            var resizedKey = new byte[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                resizedKey[i] = key[i % key.Length];
            }
            // //

            for (var i = 0; i < input.Length; i++)
            {

                var newCharValue = (input[i] - resizedKey[i]) % byte.MaxValue;
                if (newCharValue == 0)
                {
                    newCharValue = byte.MaxValue;
                }
                result[i] = (byte) newCharValue;
            }
            return result;
        }

        static byte[] CesarEncryptString(string input, byte shiftAmount, Encoding encoding)
        {
            var inputBytes = encoding.GetBytes(input);
            return CesarEncrypt(inputBytes, shiftAmount);
        }
        static byte[] CesarDecryptString(string input, byte shiftAmount, Encoding encoding) // todo
        {
            var inputBytes = Convert.FromBase64String(input);
            return CesarDecrypt(inputBytes, shiftAmount);
        }

        static byte[] CesarEncrypt(byte[] input, byte shiftAmount)
        {
            var result = new byte[input.Length];
            
            if (shiftAmount == 0)
            {
                // no shifting needed, just create deep copy
                for (var i = 0; i < input.Length; i++)
                {
                    result[i] = input[i];
                }
            }
            else
            {
                for (int i = 0; i < input.Length; i++)
                {
                    var newCharValue = (input[i] + shiftAmount);
                    if (newCharValue > byte.MaxValue)
                    {
                        newCharValue = newCharValue - byte.MaxValue;
                    }

                    result[i] = (byte)newCharValue; // drop the first 3 bytes of int, just use the last one
                }
            }
            return result;
        }

        static byte[] CesarDecrypt(byte[] input, byte shiftAmount) // todo
        {
            var result = new byte[input.Length];
            
            //omitting the shiftamount == 0 part because we dont allow shiftkey to be 0 in the first place
            for (int i = 0; i < input.Length; i++)
            {
                var newCharValue = (input[i] - shiftAmount);
                if (shiftAmount > input[i])
                {
                    newCharValue = (shiftAmount - input[i]);
                }

                result[i] = (byte)newCharValue; // drop the first 3 bytes of int, just use the last one
            }
            return result;
        }

      
        static void ShowEncoding(string text, Encoding encoding)
        {
            Console.WriteLine(encoding.EncodingName);
            
            Console.Write("Preamble ");
            foreach (var preambleByte in encoding.Preamble)
            {
                Console.Write(preambleByte + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write($"{text[i]} "); 
                foreach (var byteValue in encoding.GetBytes(text.Substring(i,1)))
                {
                    Console.Write(byteValue + " ");
                }
            }

            Console.WriteLine();
        }
    }
}

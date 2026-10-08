using System;

class Program
{
    static void Main(string[] args)
    {
        JwtCreator creator = new JwtCreator();
        string token = creator.CreateJwt();
        Console.WriteLine($"Generated Token:\n{token}\n");

        JwtDecoder decoder = new JwtDecoder();
        decoder.DecodeJwt(token);
    }
}

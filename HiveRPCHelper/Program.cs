﻿using System.IO;
using System;
using System.Threading;
using DiscordRPC;
class ReadFromFile
{
    public static void Main()
    {
        var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string OnixClientFolder = localappdata + "/Packages/Microsoft.MinecraftUWP_8wekyb3d8bbwe/RoamingState/OnixClient/Scripts/Data/";

        string username = File.ReadAllText(OnixClientFolder + "HiveRPCHelperUsername.txt");
        string gamemode = File.ReadAllText(OnixClientFolder + "HiveRPCHelperGamemode.txt");



        RPC.Setup();
        RPC.Start("Playing "+gamemode, "As " + username);

        while (true)
        {
            Thread.Sleep(2750);

            string newGamemode = File.ReadAllText(OnixClientFolder + "HiveRPCHelperGamemode.txt");
            string newUsername = File.ReadAllText(OnixClientFolder + "HiveRPCHelperUsername.txt");

            RPC.Client.UpdateDetails(newGamemode);
            RPC.Client.UpdateDetails(newUsername);
            Console.Clear();
            Console.WriteLine("As " + newUsername);
            Console.WriteLine("Updated to " + newGamemode);

        }
    }
}

public static class RPC
{
    public static DiscordRpcClient Client;
    public static RichPresence LastPresence;

    public static void Setup()

    {
        Client = new DiscordRpcClient("990689374054256691");
        Client.Initialize();
    }

    public static void Start(string Details, string State)
    {

        var presence = new RichPresence()
        {
            Details = Details,
            State = State,
            Assets = new Assets()

            {
                LargeImageKey = "hive-logo",
                LargeImageText = "Playing on the Hive"
            }
        };

        LastPresence = presence;
        presence.Timestamps = new Timestamps()
        {
            Start = DateTime.UtcNow
        };

        Client.SetPresence(presence);

    }

    public static void Update(string Details)
    {
        var presence = LastPresence;

        presence.Details = Details;

        Client.SetPresence(presence);
    }
}
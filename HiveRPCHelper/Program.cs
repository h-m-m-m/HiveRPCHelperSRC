using System.IO;
using System;
using System.Threading;
using DiscordRPC;

class ReadFromFile
{
    public static void Main()
    {
        try {
            var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string OnixClientFolder = localappdata + "/Packages/Microsoft.MinecraftUWP_8wekyb3d8bbwe/RoamingState/OnixClient/Scripts/Data/";

            string username = File.ReadAllText(OnixClientFolder + "HiveRPCHelperUsername.txt");
            string gamemode = File.ReadAllText(OnixClientFolder + "HiveRPCHelperGamemode.txt");
    
            RPC.Setup();
            RPC.Start(gamemode, username);

            while (true)
            {
                Thread.Sleep(2500);

                string newGamemode = File.ReadAllText(OnixClientFolder + "HiveRPCHelperGamemode.txt");
                string newUsername = File.ReadAllText(OnixClientFolder + "HiveRPCHelperUsername.txt");

                RPC.Client.UpdateState(newUsername);
                RPC.Client.UpdateDetails(newGamemode);
                Console.Clear();
                Console.WriteLine(newUsername);
                Console.WriteLine(newGamemode);

            }
        } catch (exception E) {
            System.Windows.Messagebox.Show()(E.Message);
            Console.ReadKey();
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
            },
            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            };
        };

        LastPresence = presence;

        Client.SetPresence(presence);

    }
}

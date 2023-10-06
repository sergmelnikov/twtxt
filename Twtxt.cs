using System.Text;
using System.Text.Json;
using twtxt.Models;

namespace twtxt;

public class Twtxt
{
    private readonly string _textFile = "twtxt.txt";
    private readonly string _configFile = ".twtxtconfig";

    public void MakeTweet(string text)
    {
        Tweet tw = new Tweet(text);
        FileInfo fi = new FileInfo(_textFile);

        if(fi.Exists)
        {
            using StreamWriter sw = fi.AppendText();
            sw.WriteLine($"{tw.AbsoluteDateTime}\t{tw.Text}");
        }
        else
        {
            using StreamWriter sw = new StreamWriter(fi.Create(), Encoding.UTF8);
            sw.WriteLine($"{tw.AbsoluteDateTime}\t{tw.Text}");
        }
    }

    public void ShowTimeline()
    {
    }

    public void ShowHelp()
    {
        Console.WriteLine(
            """
            Usage:
                    ./twtxtc [COMMAND]

            Commands:
                    tweet <text>            Adds <text> to your twtxt timeline.
                    timeline                Displays your twtxt timeline.
                    following               Gives you a list of all people you follow.
                    follow <user> <URL>     Adds the twtxt file from <URL> to your timeline.
                                            <user> defines the user name to display.
                    unfollow <user>         Removes the user with the display name <user> from your timeline.
                    help                    Displays this help screen.
            """);
    }
    public void Follow(string username, string url)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var followList = new FollowList();
        var newSource = new Source(username, url);

        if(File.Exists(_configFile))
        {
            using(FileStream fs = new FileStream(_configFile, FileMode.OpenOrCreate))
            {
                try
                {
                    followList = JsonSerializer.Deserialize<FollowList>(fs);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        using(FileStream fs = new FileStream(_configFile, FileMode.Create))
        {
            followList?.Following.Add(newSource);
            JsonSerializer.Serialize(fs, followList, options);
        }

    }
}

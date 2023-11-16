using System;
using System.Reflection;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        //OS platform
        string osPlatform = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        //.NET core version
        string dotnetCoreVersion = Assembly
            .GetEntryAssembly()?
            .GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>()?
            .FrameworkName;

        //Application version
        string applicationVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

        //Application last build time
        const int peHeaderOffset = 60;
        const int linkerTimestampOffset = 8;
        byte[] bytes = new byte[2048];
        using (FileStream file = new FileStream(Assembly.GetExecutingAssembly().Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            file.Read(bytes, 0, bytes.Length);
        }
        Int32 headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
        Int32 secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime dateTimeUTC = dt.AddSeconds(secondsSince1970);
        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUTC, TimeZoneInfo.Local);
        string applicationLastBuildTime = localTime.ToString("dd-MMM-yyyy hh:mm:sstt") + " " + TimeZoneInfo.Local.Id;

        //Outputs
        Console.WriteLine("OS Platform:                 " + osPlatform);
        Console.WriteLine("ASP.NET Core version:        " + dotnetCoreVersion);
        Console.WriteLine("Current application version: " + applicationVersion);
        Console.WriteLine("Application last built:      " + applicationLastBuildTime);


        //KEnny's code
        var osPlatform2 = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        Console.WriteLine("OS Platform:                 " + osPlatform);

        var dotnetCoreVersion2 = Assembly.GetEntryAssembly().GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>().FrameworkName;
        Console.WriteLine("ASP.NET Core version:        " + dotnetCoreVersion);

        var applicationVersion2 = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
        Console.WriteLine("Current application version: " + applicationVersion);

        var applicationLastBuildTime2 = File.GetLastWriteTime(Assembly.GetEntryAssembly().Location).ToString("dd-MMM-yyyy hh:mm:sstt");
        Console.WriteLine("Application last built:      " + applicationLastBuildTime2);
        Console.WriteLine("OS Platform:                 " + osPlatform2);
        Console.WriteLine("ASP.NET Core version:        " + dotnetCoreVersion2);
        Console.WriteLine("Current application version: " + applicationVersion2);
        Console.WriteLine("Application last built:      " + applicationLastBuildTime2);

    }
}


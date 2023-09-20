using System.Diagnostics;

public class End2End
{
    [Fact]
    public void TestReadCheep()
    {
        string output = "";
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "C:/Users/ccaas/Desktop/Kode projekter/BDSA project/Chirp/src/Chirp.CLI/Program.cs";
                    process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.dll read 17";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WorkingDirectory = "C:/Users/ccaas/Desktop/Kode projekter/BDSA project/Chirp";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    // Synchronously read the standard output of the spawned process.
                    StreamReader reader = process.StandardOutput;
                    output = reader.ReadToEnd();
                    process.WaitForExit();
                }
                string firstCheep = output.Split("\n")[0];
                // Assert
                Assert.StartsWith("hyelan", firstCheep);
                Assert.EndsWith("HALLOOOOOOOO", firstCheep);
        /*
        // Arrange
        ArrangeTestDatabase();
        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "/usr/bin/dotnet";
            process.StartInfo.Arguments = "./src/Chirp.CLI.Client/bin/Debug/net7.0/chirp.dll read 10";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        string fstCheep = output.Split("\n")[0];
        // Assert
        Assert.StartsWith("ropf", fstCheep);
        Assert.EndsWith("Hello, World!", fstCheep);
        */
    }
}
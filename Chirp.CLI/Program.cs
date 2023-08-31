using System.ComponentModel.Design;
using System.Threading.Channels;

class Program{
    public static void Main(string[] args){
        if(args.Length == 0){
            Console.WriteLine("Please choose 'read' or 'cheep'");
            return;
        }

        if(args[0] == "read"){
            read();
        }
        else if(args[0] == "cheep"){
            cheep(args[1]);
        }
    }

    private static void cheep(string message){

    }

    private static void read(){

    }
}

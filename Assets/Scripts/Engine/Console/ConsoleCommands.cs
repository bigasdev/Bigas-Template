using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConsoleCommands : MonoBehaviour{
    public abstract class ConsoleCommand{
        public string prefix;
        public string[] args;
    }
    public class CommandsCentral : ConsoleCommand{
        public CommandsCentral(string _prefix){
            prefix = _prefix;
        }
    }
    public class Command{
        private static CommandsCentral central{
            get{
                return new CommandsCentral("");
            }
        }
    }
}

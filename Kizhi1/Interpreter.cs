using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Kizhi1
{
    public class Interpreter
    {

        string command;//команда переданная интерпретатору
        
        internal TextWriter _writer;

        internal Dictionary<string, int> bank = new Dictionary<string, int>();
        private delegate void ItprCommands(string[] command);

        //Инициализация в методе InitCommandDictionary
        private Dictionary<string, ItprCommands> commandList = new Dictionary<string, ItprCommands>();
        

        string[] commands;

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
            InitCommandDictionary();
        }

        public void ExecuteLine(string command)
        {

            commands = Parser.SplitCommand(command);
            try
            {
                commandList[commands[0]](commands);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                _writer.WriteLine("Неизвестная команда (stop для выхода)");
            }

        }
        
        private void SetVar(string[] commands)
        {

            try
            {
                bank.Add(commands[1], System.Convert.ToInt16(commands[2]));
               
            }
            catch (System.ArgumentException msg)
            {
                Console.WriteLine(msg.Message);
            }
        }

        private void PrintVar(string[] commands)
        {
            try
            {
                _writer.WriteLine(bank[commands[1]]);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                _writer.WriteLine("Переменная отсутствует в памяти");
            }
        }

        private void SubVar(string[] commands)
        {
            try
            {
                bank[commands[1]] = bank[commands[1]] - Convert.ToInt16( commands[2]);
            }
            catch
            {
                _writer.WriteLine("Переменная отсутствует в памяти");
            }
        }

        private void RemVar(string[] commands)
        {
            if(bank.ContainsKey(commands[1]))
            {
                bank.Remove(commands[1]);
            }
            
            else  _writer.WriteLine("Переменная отсутствует в памяти");
            
        }
        
        private void InitCommandDictionary()
        {
            commandList.Add("set", SetVar);
            commandList.Add("print", PrintVar);
            commandList.Add("sub", SubVar);
            commandList.Add("rem", RemVar);
        }

    }
   
    internal class Parser
    {
        static internal string[] SplitCommand(string command)
        {
            while (command.Contains("  "))//удаляем лишние пробелы
            {
                command = command.Replace("  ", " ");
            }
            command = command.Trim(' ');
            return command.Split(' ');

        }
    }


}
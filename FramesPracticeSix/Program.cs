using System;
using System.Linq;

namespace FramesPracticeSix
{
  internal class Program
    {
        public static void Main(string[] args)
        {
            var executorOfInstallerInstruction = new ExecutorOfInstallerInstruction();
            var executorOfAccountCreationInstruction = new ExecutorOfAccountCreationInstruction();

            var gameInstaller = new GameInstaller(executorOfInstallerInstruction);
            var accountCreator = new AccountCreator(executorOfAccountCreationInstruction);

            var gameLauncher = new GameLauncher(gameInstaller, accountCreator);

            OutputData(executorOfInstallerInstruction, executorOfAccountCreationInstruction, gameInstaller,
                accountCreator, gameLauncher);
            
            Console.WriteLine(String.Concat(Enumerable.Repeat("*", 30)));

            executorOfInstallerInstruction.GetSlot("computerTurnedOn").Value = true;
            executorOfAccountCreationInstruction.GetSlot("internetConnected").Value = true;
            
            OutputData(executorOfInstallerInstruction, executorOfAccountCreationInstruction, gameInstaller,
                accountCreator, gameLauncher);
        }

        private static void OutputData(ExecutorOfInstallerInstruction executorOfInstallerInstruction, 
            ExecutorOfAccountCreationInstruction executorOfAccountCreationInstruction, GameInstaller gameInstaller,
            AccountCreator accountCreator, GameLauncher gameLauncher)
        {
            Console.WriteLine($"Computer turned on: {executorOfInstallerInstruction.GetSlot("computerTurnedOn").Bool}\n" +
                              $"Instruction completed: {executorOfInstallerInstruction.GetSlot("instructionCompleted").Bool}\n" +
                              $"----------\n" +
                              $"Internet connected: {executorOfAccountCreationInstruction.GetSlot("internetConnected").Bool}\n" +
                              $"Instruction completed: {executorOfAccountCreationInstruction.GetSlot("instructionCompleted").Bool}\n" +
                              $"----------\n" +
                              $"Game installed: {gameInstaller.GetSlot("gameInstalled").Bool}\n" +
                              $"Account created: {accountCreator.GetSlot("accountCreated").Bool}\n" +
                              $"Game running: {gameLauncher.GetSlot("gameRunning").Bool}");
        }
    }
}
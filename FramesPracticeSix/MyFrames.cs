using System;
using System.Collections.Generic;
using FramesPracticeSix.Utils;

namespace FramesPracticeSix
{
    /// <summary>
    /// Уровень 2.1 Выполнение инструкции установщика
    /// </summary>
    public class ExecutorOfInstallerInstruction : Frame
    {
        public ExecutorOfInstallerInstruction() : base(nameof(ExecutorOfInstallerInstruction))
        {
            Slots = new Dictionary<string, Slot>()
            {
                // Компьютер включен
                { "computerTurnedOn", Slot.BoolSlot(false) },
                // Выполнить инструкцию
                { "instructionCompleted_1", Slot.BoolSlot(false) } 
            };
        }
    }

    /// <summary>
    /// Уровень 2.2 Выполнение инструкции создания аккаунта
    /// </summary>
    public class ExecutorOfAccountCreationInstruction : Frame
    {
        public ExecutorOfAccountCreationInstruction() : base(nameof(ExecutorOfAccountCreationInstruction))
        {
            Slots = new Dictionary<string, Slot>()
            {
                // Интернет подключен
                { "internetConnected", Slot.BoolSlot(false) }, 
                // Выполнить инструкцию
                { "instructionCompleted_2", Slot.BoolSlot(false) } 
            };
        }
    }

    /// <summary>
    /// Уровень 1.1 Установка игры
    /// </summary>
    public class GameInstaller : Frame
    {
        public GameInstaller(Frame executorOfInstallerInstruction) : base(nameof(GameInstaller))
        {
            Slots = new Dictionary<string, Slot>()
            {
                // Игра установлена
                { "gameInstalled", Slot.BoolSlot(false).SetDaemon(DaemonGameInstalled()) }, 
                // Выполнение инструкции установщика
                { nameof(ExecutorOfInstallerInstruction), Slot.FrameSlot(executorOfInstallerInstruction) } 
            };
        }

        private Daemon DaemonGameInstalled()
        {
            return Daemon.CreateDaemon(TypeOfDaemonInvocation.OnChangeSlotValue, () =>
            {
                Slots[nameof(ExecutorOfInstallerInstruction)].Frame.GetSlot("gameInstalled").Value = Slots["gameRunning"].Bool;
            });
        }
    }

    /// <summary>
    /// Установка 1.2 Создания аккаунта
    /// </summary>
    public class AccountCreator : Frame
    {
        public AccountCreator(Frame executorOfAccountCreationInstruction) : base(nameof(AccountCreator))
        {
            Slots = new Dictionary<string, Slot>()
            {
                // Аккаунт создан
                { "accountCreated", Slot.BoolSlot(false) }, 
                // Выполнение инструкции создания аккаунта
                {nameof(ExecutorOfAccountCreationInstruction), Slot.FrameSlot(executorOfAccountCreationInstruction)}
            };
        }
    }

    /// <summary>
    /// Установка 0. Запуск игры
    /// </summary>
    public class GameLauncher : Frame
    {
        public GameLauncher(Frame gameInstaller, Frame accountCreator) : base(nameof(GameLauncher))
        {
            Slots = new Dictionary<string, Slot>()
            {
                { "gameRunning", Slot.BoolSlot(false).SetDaemon(DaemonGameLauncher()) }, // Аккаунт создан
                { nameof(GameInstaller), Slot.FrameSlot(gameInstaller) }, //  Установщика игры
                { nameof(AccountCreator), Slot.FrameSlot(accountCreator) } // Создатель аккаунта
            };
        }

        private Daemon DaemonGameLauncher()
        {
            return Daemon.CreateDaemon(TypeOfDaemonInvocation.OnChangeSlotValue, () =>
            {
                Slots[nameof(GameInstaller)].Frame.GetSlot("gameInstalled").Value = Slots["gameRunning"].Bool;
            });
        }
    }
}
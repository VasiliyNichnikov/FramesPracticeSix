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
                { "computerTurnedOn", Slot.BoolSlot(false).SetDaemon(GetComputerTurnedOnDaemon()) },
                // Выполнить инструкцию
                { "instructionCompleted", Slot.BoolSlot(false) } 
            };
        }

        private Daemon GetComputerTurnedOnDaemon()
        {
            return Daemon.CreateDaemon(TypeOfDaemonInvocation.OnChangeSlotValue, () =>
            {
                Slots["instructionCompleted"].Value = Slots["computerTurnedOn"].Bool;
            });
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
                { "internetConnected", Slot.BoolSlot(false).SetDaemon(GetInternetConnectedDaemon()) }, 
                // Выполнить инструкцию
                { "instructionCompleted", Slot.BoolSlot(false) } 
            };
        }

        private Daemon GetInternetConnectedDaemon()
        {
            return Daemon.CreateDaemon(TypeOfDaemonInvocation.OnChangeSlotValue, () =>
            {
                Slots["instructionCompleted"].Value = Slots["internetConnected"].Bool;
            });
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
                { "gameInstalled", Slot.BoolSlot(false) }, 
                // Выполнение инструкции установщика
                { nameof(ExecutorOfInstallerInstruction), Slot.FrameSlot(executorOfInstallerInstruction) } 
            };

            Slots[nameof(ExecutorOfInstallerInstruction)].Frame.GetSlot("instructionCompleted")
                .SetDaemon(GetInstructionCompletedDaemon());
        }

        private Daemon GetInstructionCompletedDaemon()
        {
            return Daemon.CreateDaemon(TypeOfDaemonInvocation.OnChangeSlotValue, () =>
            {
                Slots["gameInstalled"].Value = Slots[nameof(ExecutorOfInstallerInstruction)].Frame.GetSlot("instructionCompleted").Bool;
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

            Slots[nameof(ExecutorOfAccountCreationInstruction)].Frame.GetSlot("instructionCompleted")
                .SetDaemon(GetInstructionCompletedDaemon());
        }

        private Daemon GetInstructionCompletedDaemon()
        {
            return Daemon.CreateDaemon(TypeOfDaemonInvocation.OnChangeSlotValue, () =>
            {
                Slots["accountCreated"].Value = Slots[nameof(ExecutorOfAccountCreationInstruction)].Frame.GetSlot("instructionCompleted").Bool;
            });
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
                { "gameRunning", Slot.BoolSlot(false) }, // Аккаунт создан
                { nameof(GameInstaller), Slot.FrameSlot(gameInstaller) }, //  Установщика игры
                { nameof(AccountCreator), Slot.FrameSlot(accountCreator) } // Создатель аккаунта
            };

            Slots[nameof(GameInstaller)].Frame.GetSlot("gameInstalled").SetDaemon(GetGameInstallerAndAccountCreatorDaemon());
            Slots[nameof(AccountCreator)].Frame.GetSlot("accountCreated").SetDaemon(GetGameInstallerAndAccountCreatorDaemon());
        }


        private Daemon GetGameInstallerAndAccountCreatorDaemon()
        {
            return Daemon.CreateDaemon(TypeOfDaemonInvocation.OnChangeSlotValue, () =>
            {
                Slots["gameRunning"].Value = Slots[nameof(GameInstaller)].Frame.GetSlot("gameInstalled").Bool &&
                                             Slots[nameof(AccountCreator)].Frame.GetSlot("accountCreated").Bool;
            });
        }
    }
}
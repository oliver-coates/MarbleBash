using System;
using UnityEngine;

namespace MarbleBash
{


    public static class GameController
    {
        private static bool _initialised;

        // Game Initialisation order:
        // Initialise Core - Game core logic is setup
        public static Action OnInitialiseCore;

        // Initialise player - All player logic and scripts are set up
        public static Action OnInitialisePlayer;

        // Initialise player secondary - A secondary call made following the player's core classes
        // are intitialised.
        public static Action OnInitialisePlayerSecondary;

        // Initialise UI - Player UI
        public static Action OnInitialiseUI;

        // Initialise Level - When entering/exiting a level
        public static Action OnInitialiseLevel;


        public static void Initialise()
        {
            if (_initialised)
            {
                Debug.LogError("Do not initialise the game controller twice.");
                return;
            }

            _initialised = true;

            RunInitialisationOrder();
        }

        private static void RunInitialisationOrder()
        {
            OnInitialiseCore?.Invoke();

            OnInitialisePlayer?.Invoke();

            OnInitialisePlayerSecondary?.Invoke();

            OnInitialiseUI?.Invoke();

            // TODO:  This should be called in response to dungeon generation:
            OnInitialiseLevel?.Invoke();
        }
    }


}

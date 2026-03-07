/*
 * ii's Stupid Menu  Classes/Menu/ButtonInfo.cs
 * A mod menu for Gorilla Tag with over 1000+ mods
 *
 * Copyright (C) 2026  Goldentrophy Software
 * https://github.com/iiDk-the-actual/iis.Stupid.Menu
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

﻿using System;

namespace iiMenu.Classes.Menu
{
    public enum Category
    {
        Main,
        Settings,
        MenuSettings,
        RoomSettings,
        MovementSettings,
        RigSettings,
        ProjectileSettings,
        Room,
        Important,
        Safety,
        Movement,
        Rig,
        Advantage,
        Visual,
        Fun,
        RebindSettings,
        Sound,
        Projectiles,
        Networked,
        NetworkedSettings,
        Master,
        Overpowered,
        Soundboard,
        Favorites,
        MenuPresets,
        AdvantageSettings,
        VisualSettings,
        Admin,
        EnabledMods,
        Internal,
        SoundLibrary,
        Experimental,
        SafetySettings,
        Temporary,
        SoundboardSettings,
        OverpoweredSettings,
        KeybindSettings,
        PluginSettings,
        Friends,
        FriendSettings,
        FunSettings,
        Players,
        Credits,
        CustomMaps,
        ModGivers,
        ChatMessages,
        Macros,
        Detected,
        DetectedSettings,
        Achievements,
        ModList,
        Patreon,
        PatreonSettings
    }

    public class ButtonInfo
    {
        public Category category;

        public string buttonText = "-"; // Button code name, displayed in menu if overlapText is null
        public string overlapText;      // Button text displayed on menu, overrides buttonText

        public string[] aliases;        // Other terms a button can go by for searching, not displayed in menu

        public string toolTip = "This button doesn't have a tooltip/tutorial."; // Tooltip of button, a short description

        public Action method;           // Every frame before GTPlayer.LateUpdate is called
        public Action postMethod;       // Every frame after GTPlayer.LateUpdate is called

        public Action enableMethod;     // Once before method on enable
        public Action disableMethod;    // Once on disable

        public bool enabled;
        public bool isTogglable = true;

        public bool label;
        public bool incremental;
        public bool detected;

        public string customBind;
        public string rebindKey;
    }
}

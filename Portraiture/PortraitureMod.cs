﻿using StardewModdingAPI;
using StardewModdingAPI.Events;

using StardewValley;
using StardewValley.Menus;
using System.IO;

namespace Portraiture
{
    public class PortraitureMod : Mod
    {
        public IModHelper helper;
        public int tick = 0;
        private Dialogue nextDialogue;

        public override void Entry(IModHelper helper)
        {
            string customContentFolder = Path.Combine(helper.DirectoryPath, "Portraits");

            if (!Directory.Exists(customContentFolder))
            {
                Directory.CreateDirectory(customContentFolder);
            }


            this.helper = helper;
            ImageHelper.helper = helper;
            ImageHelper.monitor = Monitor;
            PortraitureDialogueBoxNew.Monitor = Monitor;
            MenuEvents.MenuClosed += MenuEvents_MenuClosed;
            MenuEvents.MenuChanged += MenuEvents_MenuChanged;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            GameEvents.EighthUpdateTick += GameEvents_EighthUpdateTick;


            
        }

        private void MenuEvents_MenuClosed(object sender, EventArgsClickableMenuClosed e)
        {
            ImageHelper.displayAlpha = 0;
            PortraitureDialogueBoxNew.animationFinished = false;
        }

        private void GameEvents_EighthUpdateTick(object sender, System.EventArgs e)
        {
            tick++;
            PortraitureDialogueBoxNew.totalTick = tick;
        }

        private void SaveEvents_AfterLoad(object sender, System.EventArgs e)
        {
            ImageHelper.displayAlpha = 0;
            ImageHelper.loadTextureFolders();
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            string key = e.KeyPressed.ToString();   

            if (key == "P" && Game1.activeClickableMenu is PortraitureDialogueBoxNew)
            {
                ImageHelper.nextFolder();
            }
        }

        private void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuChanged e)
        {

            if (Game1.activeClickableMenu is DialogueBox && (!(Game1.activeClickableMenu is PortraitureDialogueBoxNew)))
            {
                DialogueBox oldBox = (DialogueBox) Game1.activeClickableMenu;
                NPC speaker = Game1.currentSpeaker;

                if (oldBox.isPortraitBox() == true && speaker != null && speaker.Portrait != null)
                {

                    Dialogue dialogue = speaker.CurrentDialogue.Peek();

                    bool isLewis = (speaker != Game1.getCharacterFromName("Lewis"));

                    int count = 0;
                    if (isLewis && Game1.isFestival())
                    {
                        try
                        {
                            count = dialogue.getResponseOptions().Count;
                        }
                        catch { }
                    }
                    

                    if (count == 0 || !isLewis || Game1.isFestival() == false)
                    {
                        Game1.activeClickableMenu = new PortraitureDialogueBoxNew(dialogue);
                    }

                }
                
               
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System;

namespace PocketDarkSouls
{

    /*
     * Spring 2026
     * This is a single-parameter command class.
     * You may modify it to suit your needs.
     */
    public abstract class Command
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; } }
        private string _secondWord;
        public string SecondWord { get { return _secondWord; } set { _secondWord = value; } }
        private string _ThirdWord;
        public string ThirdWord { get { return _ThirdWord; } set { _ThirdWord = value; } }

        private string _ForthWord;
        public string ForthWord { get { return _ForthWord; } set { _ForthWord = value; } }

        public Command()
        {
            this.Name = "";
            this.SecondWord = null;
            this.ThirdWord = null;
        }

        public bool HasSecondWord()
        {
            return this.SecondWord != null;
        }

        public bool HasThirdWord()
        {
            return this.ThirdWord != null;
        }

        public bool HasForthWord()
        {
            return this.ThirdWord != null;
        }

        public abstract bool Execute(Player player);
    }
}

namespace FrogsAndToadsCore
{
    public class PlayChoice
    {
        internal static PlayChoice ChoiceMade(int choice)
        {
            return new PlayChoice(choice);
        }

        internal static PlayChoice NoChoice()
        {
            return new PlayChoice();
        }




        private int _choice;
        private bool _noChoiceMade;


        internal int Choice => _choice;
        internal bool NoChoiceMade => _noChoiceMade;


        private PlayChoice(int choice)
        {
            _choice = choice;
            _noChoiceMade = false;
        }

        private PlayChoice()
        {
            _noChoiceMade = true;
        }
    }
}
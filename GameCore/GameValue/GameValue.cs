using System.Collections.Generic;
using System.Text;

namespace GameCore.GameValue
{
    public class GameValue
    {
        private List<GameValue> _leftOptions;
        private List<GameValue> _rightOptions;



        internal GameValue(IEnumerable<GameValue> left, IEnumerable<GameValue> right)
        {
            _leftOptions = new List<GameValue>(left);
            _rightOptions = new List<GameValue>(right);
        }


        internal GameValue(int number)
        {
            if (number == 0)
            {
                _leftOptions = new List<GameValue>();
                _rightOptions = new List<GameValue>();
                return;
            }

            if (number > 0)
            {
                _leftOptions = new List<GameValue> { new GameValue(number - 1) };
                _rightOptions = new List<GameValue>();
            }

            if (number < 0)
            {
                _leftOptions = new List<GameValue>();
                _rightOptions = new List<GameValue> { new GameValue(number + 1) };
            }
        }



        public override string ToString()
        {
            if (_leftOptions.Count == 0 && _rightOptions.Count == 0)
                return "0";

            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            foreach (GameValue option in _leftOptions)
            {
                sb.Append(option.ToString());
            }

            sb.Append(" | ");
            foreach (GameValue option in _rightOptions)
            {
                sb.Append(option.ToString());
            }
            sb.Append(" }");

            return sb.ToString();
        }
    }
}

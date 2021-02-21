using System;

namespace Bistronger.Data
{
    public class DefaultFilter
    {
        // Paginering:
        //      Pagina i: TakeAmount = 6, SkipAmount = (i - 1) * TakeAmount
        // Geen paginering:
        //      TakeAmount = null, SkipAmount = null

        private int? _takeAmount;
        private int? _skipAmount;

        public int? TakeAmount
        {
            get => _takeAmount;
            set
            {
                _takeAmount = value;
                SkipAmount = null;
            }
        }

        public int? SkipAmount
        {
            get => _skipAmount;
            set
            {
                if (value != null)
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("SkipAmount must be greater than 0");
                    else if (TakeAmount == null)
                        value = null;
                    else if (value % TakeAmount != 0)
                        throw new ArgumentOutOfRangeException("SkipAmount must be a factor of TakeAmount.");
                }
                _skipAmount = value;
            }
        }
    }
}
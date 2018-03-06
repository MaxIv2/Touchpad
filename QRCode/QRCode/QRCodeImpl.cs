using System;
using System.Collections.Generic;
using System.Drawing;

namespace QRCode
{
    class QRCodeImpl
    {
        #region variables

        private string data;
        private int datalen;
        private int ver;
        private int size;
        private QRCode.ErrorCorrLev ErrCorr;

        #endregion

        public QRCodeImpl(string d, int s, QRCode.ErrorCorrLev Err)
        {
            this.data = d.ToUpper();
            this.datalen = data.Length;
            this.ver = 1;
            this.size = s;
            this.ErrCorr = Err;
        }

        public Bitmap Generate()
        {
            List<bool> EncodedData = Encode();

            return new Bitmap(1, 1);
        }

        public List<bool> Encode()
        {
            double num;

            List<bool> EncodedData = new List<bool> { false, false, true, false };

            AddToEncodedData(EncodedData, this.datalen, 9);

            for (int i = 0; i < this.datalen / 2 * 2; i += 2){
                num = ConvertCharToNum(data[i]) * 45 + ConvertCharToNum(data[i + 1]);
                AddToEncodedData(EncodedData, num, 11);
            }

            if (this.datalen / 2 * 2 != this.datalen){
                num = ConvertCharToNum(data[this.datalen - 1]);
                AddToEncodedData(EncodedData, num, 6);
            }

            #region Padding

            int Wantedbits = 104;

            int bitsToAdd = Wantedbits - EncodedData.Count;
            bitsToAdd = Math.Min(4, bitsToAdd);

            AddToEncodedData(EncodedData, 0, bitsToAdd);

            bitsToAdd = (Wantedbits - EncodedData.Count) % 8;

            AddToEncodedData(EncodedData, 0, bitsToAdd);

            double pad = 236;

            while (EncodedData.Count != Wantedbits){
                AddToEncodedData(EncodedData, pad, 8);

                if (pad == 236)
                    pad = 17;
                else
                    pad = 236;
            }

            #endregion

            return EncodedData;
        }

        private void AddToEncodedData(List<bool> EncodedData, double num, int bits)
        {
            for (int i = bits - 1; i >= 0; i--)
            {
                if (num >= Math.Pow(2, i))
                {
                    num = num - Math.Pow(2, i);
                    EncodedData.Add(true);
                }
                else
                    EncodedData.Add(false);
            }
        }

        private int ConvertCharToNum(Char ch)
        {
            if (ch > 47 && ch < 58)
                return ch - 48;
            if (ch > 64 && ch < 91)
                return ch - 55;
            switch (ch)
            {
                case (' '):
                    return 36;
                case ('$'):
                    return 37;
                case ('%'):
                    return 38;
                case ('*'):
                    return 39;
                case ('+'):
                    return 40;
                case ('-'):
                    return 41;
                case ('.'):
                    return 42;
                case ('/'):
                    return 43;
                case (':'):
                    return 44;
            }
            return 0;
        }
    }
}

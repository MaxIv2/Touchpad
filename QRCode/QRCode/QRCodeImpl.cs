using System;
using System.Collections.Generic;
using System.Drawing;

namespace QRCode
{
    class QRCodeImpl
    {
        #region variables

        private string data;
        private int size;
        private int Ver;
        private bool[] EncodedData;
        private int lastPosition;
        private ErrorCorrectionLevels ErrCorrLev;
        
        #endregion

        #region Static variables

        private static int[] LErrorCorrectionArr = { 19, 34, 55, 80, 108, 136, 156, 194, 232, 274, 324, 370, 428, 461, 523, 589, 647, 721, 795, 861, 932, 1006, 1094, 1174, 1276, 1370, 1468, 1531, 1631, 1735, 1843, 1955, 2071, 2191, 2306, 2434, 2566, 2702, 2812, 2956 };
        private static int[] MErrorCorrectionArr = { 16, 28, 44, 64, 86, 108, 124, 154, 182, 216, 254, 290, 334, 365, 415, 453, 507, 563, 627, 669, 714, 782, 860, 914, 1000, 1062, 1193, 1267, 1373, 1455, 1541, 1631, 1725, 1812, 1914, 1992, 2102, 2216, 2336 };
        private static int[] HErrorCorrectionArr = { 13, 22, 34, 48, 62, 76, 88, 110, 132, 154, 180, 206, 244, 261, 295, 325, 367, 397, 445, 485, 512, 568, 614, 664, 718, 754, 808, 871, 911, 985, 1033, 1115, 1171, 1231, 1286, 1354, 1426, 1502, 1582, 1666 };
        private static int[] QErrorCorrectionArr = { 9, 16, 26, 36, 46, 60, 66, 86, 100, 122, 140, 158, 180, 197, 223, 253, 283, 313, 341, 385, 406, 442, 514, 538, 596, 628, 661, 701, 745, 793, 845, 901, 961, 986, 1054, 1096, 1142, 1222, 1276 };

        public enum ErrorCorrectionLevels { L, M, H, Q };

        #endregion

        private void DeterminVersion(){
            switch (this.ErrCorrLev){

                case (ErrorCorrectionLevels.L):
                    for (int i = 1; i <= 40; i++){
                        if (LErrorCorrectionArr[i] > this.data.Length)
                            this.Ver = i;
                    }
                    break;

                case (ErrorCorrectionLevels.M):
                    for (int i = 1; i <= 40; i++){
                        if (MErrorCorrectionArr[i] > this.data.Length)
                            this.Ver = i;
                    }
                    break;

                case (ErrorCorrectionLevels.H):
                    for (int i = 1; i <= 40; i++){
                        if (HErrorCorrectionArr[i] > this.data.Length)
                            this.Ver = i;
                    }
                    break;

                case (ErrorCorrectionLevels.Q):
                    for (int i = 1; i <= 40; i++){
                        if (QErrorCorrectionArr[i] > this.data.Length)
                            this.Ver = i;
                    }
                    break;
            }
        }

        public QRCodeImpl(string D, int S, ErrorCorrectionLevels E)
        {
            this.data = D.ToUpper();
            this.size = S;
            this.ErrCorrLev = E;
            DeterminVersion();
            this.EncodedData = new bool[104];
            this.lastPosition = 0;
        }

        public Bitmap Generate()
        {
            Encode();
            AddErrorCorrection();

            return new Bitmap(1, 1);
        }

        private void AddErrorCorrection()
        {
            int[] MessagePolynomial = new int[this.EncodedData.Length / 8];
            int num;

            for (int i = 0; i < MessagePolynomial.Length; i++){
                num = 128;
                for (int j = 0; j < 8; j++){
                    if (this.EncodedData[i * 8 + j])
                        MessagePolynomial[i] += num;
                    num /= 2;
                }
            }
            int[] GeneratorPolynomial = Polynom.GenerateGeneratorPolynomial(13);
        }

        public void Encode()
        {
            double num;

            AddToEncodedData(2, 4);
            AddToEncodedData(this.data.Length, 9);

            for (int i = 0; i < this.data.Length / 2 * 2; i += 2){
                num = ConvertCharToNum(data[i]) * 45 + ConvertCharToNum(data[i + 1]);
                AddToEncodedData(num, 11);
            }

            if (this.data.Length / 2 * 2 != this.data.Length){
                num = ConvertCharToNum(data[this.data.Length - 1]);
                AddToEncodedData(num, 6);
            }

            #region Padding

            int bitsToAdd = this.EncodedData.Length - this.lastPosition;
            bitsToAdd = Math.Min(4, bitsToAdd);

            AddToEncodedData(0, bitsToAdd);

            bitsToAdd = (this.EncodedData.Length - this.lastPosition) % 8;

            AddToEncodedData(0, bitsToAdd);

            double pad = 236;

            while (this.lastPosition != this.EncodedData.Length)
            {
                AddToEncodedData(pad, 8);

                if (pad == 236)
                    pad = 17;
                else
                    pad = 236;
            }

            #endregion
        }

        private void AddToEncodedData(double num, int bits){
            for (int i = bits - 1; i >= 0; i--){
                if (num >= Math.Pow(2, i)){
                    num = num - Math.Pow(2, i);
                    this.EncodedData[this.lastPosition] = true;
                    lastPosition++;
                }else{
                    this.EncodedData[this.lastPosition] = false;
                    lastPosition++;
                }
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

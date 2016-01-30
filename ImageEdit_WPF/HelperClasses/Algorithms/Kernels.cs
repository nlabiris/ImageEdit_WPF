namespace ImageEdit_WPF.HelperClasses.Algorithms {
    public enum KernelType {
        None,
        Sobel3x3,
        Sobel5x5,
        Sobel7x7,
        Gaussian3x3,
        Gaussian5x5,
        Gaussian7x7,
        Mean3x3,
        Mean5x5,
        Mean7x7,
        LowPass3x3,
        LowPass5x5,
        Sharpen3x3,
        Sharpen5x5,
        Sharpen7x7
    }

    public static class Kernel {
        #region Fields
        private static readonly double[,] m_sobel3x3_X = new double[3, 3] {
            {-1, 0, 1},
            {-2, 0, 2},
            {-1, 0, 1}
        };

        private static readonly double[,] m_sobel3x3_Y = new double[3, 3] {
            {1, 2, 1},
            {0, 0, 0},
            {-1, -2, -1}
        };

        private static readonly double[,] m_sobel5x5_X = new double[5, 5] {
            {-1, -2, 0, 2, 1},
            {-4, -10, 0, 10, 4},
            {-7, -17, 0, 17, 7},
            {-4, -10, 0, 10, 4},
            {-1, -2, 0, 2, 1}
        };

        private static readonly double[,] m_sobel5x5_Y = new double[5, 5] {
            {1, 4, 7, 4, 1},
            {2, 10, 17, 10, 2},
            {0, 0, 0, 0, 0},
            {-2, -10, -17, -10, -2},
            {-1, -4, -7, -4, -1}
        };

        private static readonly double[,] m_sobel7x7_X = new double[7, 7] {
            {-1, -3, -3, 0, 3, 3, 1},
            {-4, -11, -13, 0, 13, 11, 4},
            {-9, -26, -30, 0, 30, 26, 9},
            {-13, -34, -40, 0, 40, 34, 13},
            {-9, -26, -30, 0, 30, 26, 9},
            {-4, -11, -13, 0, 13, 11, 4},
            {-1, -3, -3, 0, 3, 3, 1}
        };

        private static readonly double[,] m_sobel7x7_Y = new double[7, 7] {
            {1, 4, 9, 13, 9, 4, 1},
            {3, 11, 26, 34, 26, 11, 3},
            {3, 13, 30, 40, 30, 13, 3},
            {0, 0, 0, 0, 0, 0, 0},
            {-3, -13, -30, -40, -30, -13, -3},
            {-3, -11, -26, -34, -26, -11, -3},
            {-1, -4, -9, -13, -9, -4, -1}
        };

        private static readonly double[,] m_gaussian3x3 = new double[3, 3] {
            {1, 2, 1},
            {2, 4, 2},
            {1, 2, 1}
        };

        private static readonly double[,] m_gaussian5x5 = new double[5, 5] {
            {2, 4, 5, 4, 2},
            {4, 9, 12, 9, 4},
            {5, 12, 15, 12, 5},
            {4, 9, 12, 9, 4},
            {2, 4, 5, 4, 2}
        };

        private static readonly double[,] m_gaussian7x7 = new double[7, 7] {
            {1, 1, 2, 2, 2, 1, 1},
            {1, 2, 2, 4, 2, 2, 1},
            {2, 2, 4, 8, 4, 2, 2},
            {2, 4, 8, 16, 8, 4, 2},
            {2, 2, 4, 8, 4, 2, 2},
            {1, 2, 2, 4, 2, 2, 1},
            {1, 1, 2, 2, 2, 1, 1}
        };

        private static readonly double[,] m_mean3x3 = new double[3, 3] {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1}
        };

        private static readonly double[,] m_mean5x5 = new double[5, 5] {
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1}
        };

        private static readonly double[,] m_mean7x7 = new double[7, 7] {
            {1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1}
        };

        private static readonly double[,] m_lowPass3x3 = new double[3, 3] {
            {1, 2, 1},
            {2, 4, 2},
            {1, 2, 1}
        };

        private static readonly double[,] m_lowPass5x5 = new double[5, 5] {
            {1, 1, 1, 1, 1},
            {1, 4, 4, 4, 1},
            {1, 4, 12, 4, 1},
            {1, 4, 4, 4, 1},
            {1, 1, 1, 1, 1}
        };

        private static readonly double[,] m_sharpen3x3 = new double[3, 3] {
            {-1, -1, -1,},
            {-1, 9, -1},
            {-1, -1, -1}
        };

        private static readonly double[,] m_sharpen5x5 = new double[5, 5] {
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, 25, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1}
        };

        private static readonly double[,] m_sharpen7x7 = new double[7, 7] {
            {-1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, 49, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1}
        };
        #endregion

        #region Properties
        public static double[,] M_Sobel3x3_X {
            get { return m_sobel3x3_X; }
        }

        public static double[,] M_Sobel3x3_Y {
            get { return m_sobel3x3_Y; }
        }

        public static double[,] M_Sobel5x5_X {
            get { return m_sobel5x5_X; }
        }

        public static double[,] M_Sobel5x5_Y {
            get { return m_sobel5x5_Y; }
        }

        public static double[,] M_Sobel7x7_X {
            get { return m_sobel7x7_X; }
        }

        public static double[,] M_Sobel7x7_Y {
            get { return m_sobel7x7_Y; }
        }

        public static double[,] M_Gaussian3x3 {
            get { return m_gaussian3x3; }
        }

        public static double[,] M_Gaussian5x5 {
            get { return m_gaussian5x5; }
        }

        public static double[,] M_Gaussian7x7 {
            get { return m_gaussian7x7; }
        }

        public static double[,] M_Mean3x3 {
            get { return m_mean3x3; }
        }

        public static double[,] M_Mean5x5 {
            get { return m_mean5x5; }
        }

        public static double[,] M_Mean7x7 {
            get { return m_mean7x7; }
        }

        public static double[,] M_LowPass3x3 {
            get { return m_lowPass3x3; }
        }

        public static double[,] M_LowPass5x5 {
            get { return m_lowPass5x5; }
        }

        public static double[,] M_Sharpen3x3 {
            get { return m_sharpen3x3; }
        }

        public static double[,] M_Sharpen5x5 {
            get { return m_sharpen5x5; }
        }

        public static double[,] M_Sharpen7x7 {
            get { return m_sharpen7x7; }
        }
        #endregion
    }
}

namespace ImageEdit_WPF.HelperClasses.Algorithms {
    public enum EdgeFilterType {
        None,
        EdgeDetectMono,
        EdgeDetectGradient,
        Sharpen,
        SharpenGradient
    }

    public enum DerivativeLevel {
        FirstDerivative = 1,
        SecondDerivative = 2
    }
}
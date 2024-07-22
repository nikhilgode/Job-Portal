namespace JobPortal_New.HelperMethods
{
    public  class GenerateOTP
    {

        public static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

    }
}

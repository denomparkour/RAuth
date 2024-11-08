namespace RAuth.Application.Constants
{
    public interface GlobalConstants
    {
        public static string SUCCESS = "Success";
        public static string FAILED = "Failed";

        public static string USER_NOT_FOUND = "User Not Found";
        public static string VERIFY_OTP_TO_CONTINUE = "Please verify OTP to Continue";
        public static string OTP_EXPIRED = "OTP Expired. New OTP has been sent";
        public static string INVALID_OTP = "Invalid OTP";
        public static string OTP_NOT_FOUND = "OTP Not Found/Expired. Sent New OTP";
        public static string USER_ALREADY_VERIFIED = "User Already Verified";
        public static string SIGN_IN_FAILED = "Error Occured during Sign In";
        public static string SIGN_IN_SUCCESS_OAUTH = "Signed In Successfully using OAuth";
        public static string INVALID_USER = "Invalid Username/Password";

    }
}

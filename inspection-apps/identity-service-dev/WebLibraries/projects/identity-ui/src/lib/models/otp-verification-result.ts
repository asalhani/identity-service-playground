export interface OtpVerificationResultResult {
  success: boolean;
  isInvalidOtp: boolean;
  isAccountLocked: boolean;
  isOtpExpired: boolean;
  isPasswordExpired: boolean;
  changeExpiredPasswordToken: string;
}

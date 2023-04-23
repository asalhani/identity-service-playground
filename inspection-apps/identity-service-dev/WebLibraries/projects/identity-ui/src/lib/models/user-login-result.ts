import { OtpPartInput } from './opt-part-input';

export interface UserLoginResult extends OtpPartInput {
  success: boolean;
  isOtpRequired: boolean;
  isInvalidCaptcha: boolean;
  isInvalidUserInfo: boolean;
  isAccountLocked: boolean;
  isInvalidOtp: boolean;
  isPasswordExpired: boolean;
  changeExpiredPasswordToken: string;
}

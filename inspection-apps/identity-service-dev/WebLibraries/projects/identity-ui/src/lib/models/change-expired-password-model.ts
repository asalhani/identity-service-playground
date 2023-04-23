export interface ChangeExpiredPasswordModel {
  loginName: string;
  newPassword: string;
  captchaString: string;
  changePasswordToken: string;
  isInvalidCaptcha:boolean;
}

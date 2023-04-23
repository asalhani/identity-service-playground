export interface ForgotPasswordModel {
  emailToken: string;
  email: string;
  captchaString: string;
  isInvalidCaptcha:boolean;
}
